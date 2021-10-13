using System;
using System.Collections.Generic;
using Grid;
using Interfaces;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Units
{
    public class PlayerController : MonoBehaviour
    {
        private GameObject _mainCamera;// Camera der Szene
        [HideInInspector]
        public CameraBehaviour cameraBehaviour;
        [HideInInspector]
        public bool textbox;
        protected IUtility Utility;
        private bool _cameraOn;
        public GameObject attackUi;
        private bool _playerActive;
        protected bool UiActive;
        //References to the Grid
        private GridBehaviour _behaviour;
        protected GameObject Target;
        public bool unitEscaped;

        protected UnitStat PlayerStat;

        public void Start()
        {
            _behaviour = GameObject.Find("Grid").GetComponent<GridBehaviour>();
            _mainCamera = GameObject.Find("Camera");
            PlayerStat = GetComponent<UnitStat>();
            cameraBehaviour = _mainCamera.GetComponent<CameraBehaviour>();
            _behaviour.SpawnUnits(gameObject, PlayerStat.currentPosition);
            Utility = gameObject.GetComponent<IUtility>();
        }

        public void Update()
        {
            if (_cameraOn)
            {
                cameraBehaviour.CameraSwitch(gameObject);
                if (Math.Abs(Vector3.Distance(_mainCamera.transform.position, gameObject.transform.position)) <=
                    0.5f)
                {
                    _cameraOn = false;
                }
            }

            try
            {
                if (!PlayerStat.moving && PlayerStat.currentGrid.CompareTag("Exit"))
                {
                    if (PlayerStat.currentGrid.GetComponent<GridStat>().unlocked)
                    {
                        //Speichert die Daten unter health + stelle im array ab
                        int index = PlayerStat.controller.playerList.IndexOf(gameObject) + 1;
                        PlayerPrefs.SetInt("health" + index, PlayerStat.healthPoints);
                        PlayerPrefs.SetInt("Type" + index, PlayerStat.unitType);
                        print("Player " + gameObject.name + " with Health " + PlayerStat.healthPoints + " and Type " + PlayerStat.unitType + " escaped at index " + index);
                        PlayerStat.controller.playersEscaped++;
                        PlayerStat.controller.escaped = true;
                        unitEscaped = true;
                        PlayerStat.controller.NextPlayer();
                        PlayerStat.currentGrid.GetComponent<GridStat>().taken = false;
                        gameObject.SetActive(false);
                    }
                }
            }
            catch (Exception e)
            {
            }
            
            
        }

        public void Action()//Registriert den Input des Spielers
        {
            //Statusupdate
            //UIActive = pauseMenu.activeSelf;
            //Checks if the game is paused if not lets the user input mousebuttons
            if (!(UiActive || PlayerStat.moving || PlayerStat.acting || textbox))
            {
                DetectMovement();
            
                if (Input.GetMouseButtonDown(1)) // nur ein Raycast wenn die Maus betätigt wird
                {
                    if (Camera.main != null)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject != null)
                        { 
                            
                            RefreshUi(hit);
                        }
                    }
                }
            }
        }

        public void ActivatePlayer()  //Funktion setzt die Kamera des jeweiligen Spielers aktiv und überprüft ob es der begin der runde ist
        {
            _playerActive = true;
            _cameraOn = true;
        }

        public void DeactivatePlayer()
        {
            _playerActive = false;
        }
    
        public bool IsPlayerActive()
        {
            if (_playerActive)
            {
                return true;
            }
            return false;
        }

        public bool ActionsLeft()
        {
            if (PlayerStat.actions <= 0)
            {
                return false;
            }
        
            return true;
        }

        public void ResetActions()
        {
            PlayerStat.actions = 2;
            //print("Reset Actions Player Script");
        }

        protected void RefreshUi(RaycastHit target)
        {
            if (target.transform.gameObject.CompareTag("Enemy")) //Überprüft ob es ein Gegner ist
            {
                Target = target.transform.gameObject; // Speichert den Gegner selbst
                UnitStat targetStat = target.transform.gameObject.GetComponent<UnitStat>();
                int distance = Math.Abs((int) targetStat.currentPosition.x - (int) PlayerStat.currentPosition.x) +
                               Math.Abs((int) targetStat.currentPosition.y - (int) PlayerStat.currentPosition.y);
                if (distance <= PlayerStat.maxAttackRange)
                {
                    Utility.SetTarget(Target);
                    UiActive = true;
                    attackUi.SetActive(true);
                }
                else
                {
                    attackUi.SetActive(false); //Schalte im Zweifel das Hauptinterface aus
                    UiActive = false;
                }
            }
            else
            {
                attackUi.SetActive(false); //Schalte im Zweifel das Hauptinterface aus
                UiActive = false;
            }
        }

        protected void DetectMovement()
        {
            _behaviour.PossiblePaths(gameObject, PlayerStat);
            if (Input.GetMouseButtonDown(0))
            {
                if (Camera.main != null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit) && hit.transform.gameObject != null)
                    {
                        GameObject targetGrid = hit.transform.gameObject;
                        
                        if (targetGrid.CompareTag("Grid") || targetGrid.CompareTag("Exit") || targetGrid.CompareTag("Hackport"))
                        {
                            GridStat targetGridstat = hit.transform.gameObject.GetComponent<GridStat>();
                            PlayerStat.currentPosition = _behaviour.Move(gameObject,targetGridstat, PlayerStat.currentPosition, PlayerStat);
                        }
                    }
                }
            }
        }

        public void DeactivateUi()
        {
            attackUi.SetActive(false);
            UiActive = false;
        }

        public bool IsEscaped()
        {
            if (unitEscaped)
            {
                return false;
            }

            return true;
        }
    }
}

using System;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Units.Techniker
{
    public class TechnikerController : PlayerController, IController
    {
        public GameObject abilityUi;

        public new void Action()
        {
            //Statusupdate
            //UIActive = pauseMenu.activeSelf;
            //Checks if the game is paused if not lets the user input mousebuttons
            if (!(UiActive || PlayerStat.moving))
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
    
        private new void RefreshUi(RaycastHit target)
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
            else if (target.transform.gameObject.CompareTag("Player"))
            {
                Target = target.transform.gameObject; // Speichert den Gegner selbst
                UnitStat targetStat = target.transform.gameObject.GetComponent<UnitStat>();
                int distance = Math.Abs((int) targetStat.currentPosition.x - (int) PlayerStat.currentPosition.x) +
                               Math.Abs((int) targetStat.currentPosition.y - (int) PlayerStat.currentPosition.y);
                if (distance <= PlayerStat.maxAttackRange)
                {
                    Utility.SetTarget(Target);
                    UiActive = true;
                    abilityUi.SetActive(true);
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
        public new void DeactivateUi()
        {
            attackUi.SetActive(false);
            abilityUi.SetActive(false);
            UiActive = false;
        }
    }
}

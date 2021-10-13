using System;
using System.Collections.Generic;
using Interfaces;
using Units;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public List<GameObject> playerList = new List<GameObject>(); // Liste an allen aktuell existierenden Spielern
    public List<GameObject> enemyList = new List<GameObject>(); // Liste an allen aktuell existierenden Gegnern

    public Vector2[] playerSpawns = new Vector2[5];

    public GameObject[] playerTypes = new GameObject[5];

    public int playersEscaped;

    public GameObject inGameMenu;

    public bool escaped;

    public String nextScene;
    public String gameOver;

    public bool firstLevel;
    public bool textbox;

    private IController _currentPlayerController;
    private IBehaviour _currentEnemyBehaviour;

    private int _currentRound;

    private bool _done;

    private int _enemyCounter;
    private int _playerCounter;

    private int _playersDone;

    private bool _first = true;

    private void Awake()
    {
        if (firstLevel)
        {
            GameObject[] playerCache = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in playerCache)
            {
                playerList.Add(player);
            }
        }
        else
        {
            int players = PlayerPrefs.GetInt("PlayerCount");
            GameObject[] cache = new GameObject[5];

            for (int i = 1; i <= players; i++)
            {
                int type = PlayerPrefs.GetInt("Type" + i);
                cache[i] = Instantiate(playerTypes[type], gameObject.transform);
                cache[i].tag = "Player";
                UnitStat currentPlayerStat = cache[i].GetComponent<UnitStat>();
                currentPlayerStat.healthPoints = PlayerPrefs.GetInt("health" + i);
                currentPlayerStat.currentPosition = playerSpawns[i - 1];
            }
            
            playerList.Clear();
            
            foreach (GameObject player in cache)
            {
                if (player != null)
                {
                    playerList.Add(player);
                }
            }
        }

        GameObject[] enemyCache = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemyCache)
        {
            enemyList.Add(enemy);
        }

        _currentRound = 1;
    }

    private void Update()
    {
        if (_first)
        {
            foreach (GameObject entity in playerList)
            {
                entity.GetComponent<UnitStat>().SetGrid();
            }

            foreach (GameObject entity in enemyList)
            {
                entity.GetComponent<UnitStat>().SetGrid();
            }

            if (textbox)
            {
                Textbox();
            }

            _first = false;
        }

        if (inGameMenu.activeSelf) return;
        if (playerList.Count == playersEscaped)
        {
            if (escaped)
            {
                PlayerPrefs.SetInt("PlayerCount", playersEscaped);
                SceneManager.LoadScene(nextScene);
            }
            else
            {
                SceneManager.LoadScene(gameOver);
            }
        }
        else
        {
            if (_currentRound % 2 == 0) //Äußeres if-Statement um zwische Runde Spieler und Gener zu unterscheiden
            {
                //print("Enemy: " + enemyList.Count);
                if (enemyList.Count <= 0)
                {
                    _currentRound++;
                }
                else
                {
                    if (_enemyCounter < enemyList.Count) //Geht durch die Liste durch und arbeitet die Gegner reihenweise ab
                    {
                        _currentEnemyBehaviour = enemyList[_enemyCounter].GetComponent<IBehaviour>();
                        if (_currentEnemyBehaviour == null)
                        {
                            throw new Exception("Keine Gegner");
                        }

                        if (_currentEnemyBehaviour.ActionsLeft())
                        {
                            //print("EnemyCounter: " + _enemyCounter + " EnemyListCount: " + enemyList.Count);
                            SetEnemyActive(enemyList[_enemyCounter]);
                        }
                        else
                        {
                            _enemyCounter++;
                            _currentEnemyBehaviour.ResetActions();
                        }
                    }
                    else
                    {
                        _enemyCounter = 0;
                        _currentRound++;
                    }
                }

                _playerCounter = 0;
            }
            else
            {
                //print("Player: " + playerList.Count);
                _currentPlayerController = playerList[_playerCounter].GetComponent<IController>();


                if (_currentPlayerController.ActionsLeft() && _currentPlayerController.IsEscaped()) //Überprüft ob der Spieler noch aktionen übrig hat
                {
                    if (!_currentPlayerController.IsPlayerActive()) //Überprüft ob Spieler aktiv ist wenn nein setzt er ihn aktiv
                    {
                        _currentPlayerController.ActivatePlayer();
                    }

                    _playersDone = 0;
                    _currentPlayerController.Action();
                }
                else
                {
                    if (_playersDone == playerList.Count)
                    {
                        ResetAction();
                        _currentRound++;
                    }
                    else
                    {
                        NextPlayer(); //Automatische Weiterleitung an den nächsten Spieler falls dieser kein Actions punkte mehr hat
                        _playersDone++;
                    }
                }
            }
        }
    }

    private void SetEnemyActive(GameObject unit)//Kann noch verändert werden EnemyAI ist noch nicht da
    {
        unit.GetComponent<IBehaviour>().EnemyAction(playerList, enemyList);
    }

    private void ResetAction()
    {
        foreach (GameObject player in playerList)
        {
            player.GetComponent<IController>().ResetActions();
        }
    }

    public void NextPlayer()
    {
        _currentPlayerController.DeactivatePlayer();
        _playerCounter = (_playerCounter + 1) % playerList.Count; //Erhöht playerCounter um 1
    }

    public void PrevPlayer()
    {
        _currentPlayerController.DeactivatePlayer();
        _playerCounter = Math.Abs((_playerCounter - 1) % playerList.Count); //Verringert playerCounter um 1
    }
    public void Textbox()
    {
        foreach (GameObject player in  playerList)
        {
            player.GetComponent<PlayerController>().textbox = textbox;
        }

        textbox = !textbox;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keep_Between_Scenes : MonoBehaviour
{
    private GameController _controller;
    public int hp = 0;
    public int hr = 0;

    private void Start()
    {
        _controller = GameObject.Find("Battlefield").GetComponent<GameController>();
        //Setzt initiell die Werte
        PlayerPrefs.SetInt("health" + _controller.playerList.IndexOf(gameObject), hp);
        PlayerPrefs.SetInt("hackhealth" + _controller.playerList.IndexOf(gameObject), hr);
    }

    void OnDestroy()
    {
        //hp = gameObject.GetComponent<UnitStat>().healthPoints;
        //hr = gameObject.GetComponent<UnitStat>().hackLife;
        //Speichert die Daten unter health + stelle im array ab
        PlayerPrefs.SetInt("health" + _controller.playerList.IndexOf(gameObject), hp);
        PlayerPrefs.SetInt("hackhealth" + _controller.playerList.IndexOf(gameObject), hr);
    }
}
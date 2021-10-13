using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Für Szenensprung nutze UnityEngine.SceneManagemanet
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    //Used on Play_Button
    public void PlayGame()
    {
        //Lade nächste Szene in Queue
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Used on Quit_Button
    public void QuitGame()
    {
        //Quits Game, only when build
        Application.Quit();
        //Debug for test purpouses ingame
        Debug.Log("Quit!");
    }
}
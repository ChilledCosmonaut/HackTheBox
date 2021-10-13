using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour
{
    //Bool to show current state of Game
    bool GameIsPaused = false;

    //reference to UI Pause Menu UI
    public GameObject pauseMenuUI;

    //Used on Menu_Button
    public void ToMenu()
    {
        //Lade Szene 0 Main Menu
        SceneManager.LoadScene(0);
    }

    //Used on Quit_Button
    public void QuitGame()
    {
        //Quits Game, only when build
        Application.Quit();
        //Debug for test purpouses ingame
        Debug.Log("Quit!");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        //schließt UI
        pauseMenuUI.SetActive(false);
        //Lässt das Spiel weiter laufen
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        //aktiviert UI
        pauseMenuUI.SetActive(true);
        //Pausiert das Spiel
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}

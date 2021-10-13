using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Wie immer UI Sachen per "UnityEngine.UI"
using UnityEngine.UI;
//Für Szenensprung nutze UnityEngine.SceneManagemanet
using UnityEngine.SceneManagement;

public class text_menu : MonoBehaviour
{
    //reference to Option Menu
    public GameObject optionMenu;
    //reference to Main Menu
    public GameObject mainMenu;
    //Imputfeld von welchem wir "input" lesen
    public GameObject inputfield;
    //Sting im imputfield welches wir lesen
    public string input;

    // Update is called once per frame
    void Update()
    {
        input = inputfield.GetComponent<Text>().text;
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            //list for various ways to play the game
            var play = new List<string> { "play", "play game", "start", "start game" };
            //Lade nächste Szene in Queue wenn der user "play" eingibt
            if (play.Contains(input))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                Debug.Log("play");
            }

            //list for various ways to load a savegame
            var load = new List<string> { };
            //lade menü öffnen (wird implementiert, sobald wir speichern können)
            if (load.Contains(input))
            {

            }

            //list for various ways to open the options menu
            var options = new List<string> {"options", "settings", "calibration"};
            //öffne das options menü
            if (options.Contains(input))
            {
                OpenOptions();
            }

            //list for various ways to quit the game
            var quit = new List<string> {"quit", "close", "q"};
            //schließe das Spiel
            if (quit.Contains(input))
            {
                //Quits Game, only when build
                Application.Quit();
                //Debug for test purpouses ingame
                Debug.Log("Quit!");
            }
        }
    }

    void OpenOptions()
    {
        //aktiviert UI
        optionMenu.SetActive(true);
        //deaktiviert MainMenu
        mainMenu.SetActive(false);
    }

    void CloseOptions()
    {
        //deaktiviert UI
        optionMenu.SetActive(false);
        //aktiviert MainMenu
        mainMenu.SetActive(true);
    }
}
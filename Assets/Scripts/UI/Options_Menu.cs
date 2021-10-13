using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options_Menu : MonoBehaviour
{
    //reference to Option Menu
    public GameObject optionMenu;
    //reference to Main Menu
    public GameObject mainMenu;

    // Update is called once per frame
    void Update()
    {
        //Close on Escape down
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseOptions();
        }
    }

    //Used by "CLOSE" button in the options menu
    public void CloseOptions()
    {
        //schließt UI
        optionMenu.SetActive(false);
        //aktiviert MainMenu
        mainMenu.SetActive(true);
    }
}

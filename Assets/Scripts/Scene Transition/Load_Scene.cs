using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load_Scene : MonoBehaviour
{
    //Enter the desired scene's name in the textbox and off you go
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
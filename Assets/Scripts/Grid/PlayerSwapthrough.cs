using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class PlayerSwapthrough : MonoBehaviour
{

    public GameObject grid;
    GridBehaviour behaviour;
    int border;

    void Start()
    {
        behaviour = grid.GetComponent<GridBehaviour>();
    }

    /*void switchRight() //Erhöht den PlayerCounter nach Rechts
    {
        border = behaviour.Players.Count;
        behaviour.playerCounter = (behaviour.playerCounter + 1) % border;
    }

    void switchLeft() //Erhöht den Player Counter nach links
    {
        border = behaviour.Players.Count;
        behaviour.playerCounter = (behaviour.playerCounter - 1) % border;
    }*/

}

using System;
using Grid;
using Interfaces;
using UnityEngine;

namespace Units.Hacker
{
    public class HackerController : PlayerController, IController
    {
        private bool _unlocked;
        private new void Update()
        {
            base.Update();

            try
            {
                if (!PlayerStat.moving && PlayerStat.currentGrid.CompareTag("Hackport") && !_unlocked)
                {
                    GameObject exit = GameObject.FindWithTag("Exit");
                    _unlocked = true;
                    GridStat exitStat;
                    (exitStat = exit.GetComponent<GridStat>()).unlocked = true;
                    exitStat.Open(cameraBehaviour, gameObject);

                }
            }
            catch (Exception e)
            {
                //print(e);
            }

            
        }
    }
}

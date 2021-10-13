using Interfaces;
using UnityEngine;

namespace Units.Hacker
{
    public class HackerUtility : Utility, IUtility
    {
        public int hackPercentage;

        public void Ability()
        {
            GameController controller = unit.controller;
            UnitStat targetStat = target.GetComponent<UnitStat>();

            if (controller.playerList.Count >= 5)
            {
                return;
            }
            
            if (Random.Range(0, 100) <= hackPercentage)
            {
                targetStat.hackLife -= 1;
                targetStat.HackUpdate();
                //print("sucessful Hack");
            }

            if (targetStat.hackLife <= 0)
            {
                controller.enemyList.Remove(target);
                controller.playerList.Add(target);
                target.tag = "Player";
                targetStat.hackBar.SetActive(false);
                targetStat.HealthUpdate();
            }

            unit.actions -= 2;
            unit.Exit();
        }
    }
}

using Interfaces;
using UnityEngine;

namespace Units.Techniker
{
    public class TechnikerUtility : Utility, IUtility
    {
        public void Heal()
        {
            UnitStat targetStat = target.GetComponent<UnitStat>();
            targetStat.healthPoints += 2;
            targetStat.HealthUpdate();
            unit.actions -= 1;
            unit.playerController.DeactivateUi();
        }
        public void ActionsDistribution()//passes action points to another Unit
        {
            target.GetComponent<UnitStat>().actions = 2;
            unit.actions -= 2;
            unit.playerController.DeactivateUi();
        }
    }
}

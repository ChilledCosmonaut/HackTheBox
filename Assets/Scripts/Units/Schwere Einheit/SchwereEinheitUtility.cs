using Interfaces;
using UnityEngine;

namespace Units.Schwere_Einheit
{
    public class SchwereEinheitUtility : Utility, IUtility
    {
        public void CoverFire()
        {
            UnitStat targetStat = target.GetComponent<UnitStat>();
            targetStat.GetComponent<UnitStat>().actions -= 1;
            unit.actions -= 1;
            unit.playerController.DeactivateUi();
        }
    }
}

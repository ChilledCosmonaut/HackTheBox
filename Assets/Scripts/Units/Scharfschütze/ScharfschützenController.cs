using Interfaces;

namespace Units.Scharfschütze
{
    public class ScharfschützenController : PlayerController, IController
    {
        public new void ResetActions()
        {
            PlayerStat.actions = 2;
            //print("Reset Actions Player Script");
        }
    }
}

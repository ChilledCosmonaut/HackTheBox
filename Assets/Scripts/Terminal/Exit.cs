using UnityEngine;

namespace Terminal
{
    [CreateAssetMenu(menuName = "Terminal/Actions/Exit")]
    public class Exit : InputAction
    {
        public override void RespondToInput()
        {
            Application.Quit();
        }
    }
}

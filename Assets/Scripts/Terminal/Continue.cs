using UnityEngine;

namespace Terminal
{
    [CreateAssetMenu(menuName = "Terminal/Actions/Continue")]
    public class Continue : InputAction
    {
        public override void RespondToInput()
        {
            GameObject.Find("MainUI").SetActive(true);
            GameObject.Find("InGameMenu").SetActive(false);
        }
    }
}

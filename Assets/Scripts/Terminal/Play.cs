using UnityEngine;
using UnityEngine.SceneManagement;

namespace Terminal
{
    [CreateAssetMenu(menuName = "Terminal/Actions/Play")]
    public class Play : InputAction
    {
        public override void RespondToInput()
        {
            SceneManager.LoadScene("Tutorial");
        }
    }
}

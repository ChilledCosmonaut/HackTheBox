using UnityEngine;
using UnityEngine.SceneManagement;

namespace Terminal
{
    [CreateAssetMenu(menuName = "Terminal/Actions/PlayFirst")]
    public class PlayFirst : InputAction
    {
        public override void RespondToInput()
        {
            SceneManager.LoadScene("Tutorial with Explanation");
        }
    }
}

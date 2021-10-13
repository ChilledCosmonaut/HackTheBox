using UnityEngine;

namespace Terminal
{
    [CreateAssetMenu(menuName = "Terminal/Tabs")]
    public class Tab : ScriptableObject
    {
        public bool trigger;
        public InputAction action;
        /*[TextArea]*/ public string[] description;
        public Command[] commands;

        public void Action()
        {
            action.RespondToInput();
        }
    }
}

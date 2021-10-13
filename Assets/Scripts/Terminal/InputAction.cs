using UnityEngine;

namespace Terminal
{
    public abstract class InputAction : ScriptableObject
    {
        public abstract void RespondToInput();
    }
}

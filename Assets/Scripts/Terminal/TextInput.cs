using System;
using UnityEngine;
using UnityEngine.UI;

namespace Terminal
{
    public class TextInput : MonoBehaviour
    {
        public InputField inputField;

        private TerminalController _controller;

        private void Awake()
        {
            _controller = GetComponent<TerminalController>();
            inputField.onEndEdit.AddListener(AcceptStringInput);
        }

        public void AcceptStringInput(string userInput)
        {
            userInput = userInput.ToLower();
            _controller.terminalNavigation.AttemptToSwitchTabs(userInput);
            
            InputComplete();
        }

        void InputComplete()
        {
            _controller.DisplayLoggedText();
            inputField.ActivateInputField();
            inputField.text = null;
        }
    }
}

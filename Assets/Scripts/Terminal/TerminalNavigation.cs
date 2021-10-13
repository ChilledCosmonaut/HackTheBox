using System;
using System.Collections.Generic;
using UnityEngine;

namespace Terminal
{
    public class TerminalNavigation : MonoBehaviour
    {
        public Tab currentTab;
        Dictionary<string, Tab> _commandDictionary = new Dictionary<string, Tab>();
        private TerminalController _controller;

        private void Awake()
        {
            _controller = GetComponent<TerminalController>();
        }

        public void UnpackCommandsInTab()
        {
            for (int i = 0; i < currentTab.commands.Length; i++)
            {
                _commandDictionary.Add(currentTab.commands[i].commandString, currentTab.commands[i].valueTab);
                _controller.commandDescriptionTab.Add(currentTab.commands[i].commandAction);
            }
        }

        public void AttemptToSwitchTabs(string command)
        {
            if (_commandDictionary.ContainsKey(command))
            {
                if (!_commandDictionary[command].trigger)
                {
                    currentTab = _commandDictionary[command];
                    _controller.LogStringWithReturn(command);
                    _controller.DisplayConsoleText();
                }
                else
                {
                    _commandDictionary[command].Action();   
                }
            }
            else
            {
                _controller.LogStringWithReturn("command: " + command + " not Found!");
            }
        }

        public void ClearCommands()
        {
            _commandDictionary.Clear();
        }
    }
}

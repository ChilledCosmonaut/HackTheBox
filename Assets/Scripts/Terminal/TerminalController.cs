using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Terminal
{
    public class TerminalController : MonoBehaviour
    {
        public Text displayText;
        public Text prefixText;
        public string prefix;
        public InputAction[] inputActions;
        [Range(.05f,.3f)] public float delayTime;
        
        [HideInInspector] public TerminalNavigation terminalNavigation;
        [HideInInspector] public List<string> commandDescriptionTab =new List<string>();

        List<string> actionLog = new List<string>();

        private int _counter;
        
        void Awake()
        {
            terminalNavigation = GetComponent<TerminalNavigation>();
        }

        private void Start()
        {
            DisplayConsoleText();
            //DisplayLoggedText(); 
        }

        public void DisplayLoggedText()
        {
            string logAsText = string.Join("\n", actionLog.ToArray());

            displayText.text = logAsText;
        }

        public void DisplayConsoleText()
        {
            ClearCollectionsForNewTab();
            
            UnpackTab();
            
            StartCoroutine("DelayedText");

            /*string joinedCommandDescription = string.Join("\n",commandDescriptionTab.ToArray());
            
            string combinedText = terminalNavigation.currentTab.description + "\n" + joinedCommandDescription;
            
            prefixText.text = terminalNavigation.currentTab.prefix;

            LogStringWithReturn(combinedText);*/
        }

        public IEnumerator DelayedText()
        {
            while (true)
            {
                yield return new WaitForSeconds(delayTime);
                if (_counter >= commandDescriptionTab.Count + terminalNavigation.currentTab.description.Length)
                {
                    _counter = 0;
                    StopCoroutine("DelayedText");
                }
                else if (_counter < terminalNavigation.currentTab.description.Length)
                {
                    actionLog.Add(prefix + " " + terminalNavigation.currentTab.description[_counter]);
                    DisplayLoggedText();
                    _counter++;
                }
                else
                {
                    actionLog.Add(prefix + " " + commandDescriptionTab[_counter - terminalNavigation.currentTab.description.Length]);
                    DisplayLoggedText();
                    _counter++;
                }
            }
        }

        void UnpackTab()
        {
            terminalNavigation.UnpackCommandsInTab();
        }

        void ClearCollectionsForNewTab()
        {
            commandDescriptionTab.Clear();
            terminalNavigation.ClearCommands();
        }  

        public void LogStringWithReturn(string stringToAdd)
        {
            actionLog.Add(prefix + " " +stringToAdd);
        }
    }
}

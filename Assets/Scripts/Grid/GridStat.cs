using System;
using Store_Assets.Scifi.Scripts;
using UnityEngine;

namespace Grid
{
    public class GridStat : MonoBehaviour
    {
        public int visited = -1;
        public int x = 0;
        public int y = 1;
        public bool taken;
        public int status; //0 is free, 1 is blocked, 2 is cover, 3 is Exit, 4 is Hackport
        private GridBehaviour _behaviour;
        private GameObject _door;
        public GameObject[] visualizeStatus;
        public GameObject standard;
        public GameObject path;
        public GameObject possible;
        public GameObject coverUpRight;
        public GameObject coverUpLeft;
        public GameObject coverUpFront;
        public GameObject coverUpBack;
        public bool unlocked;
        public bool coverRight;
        public bool coverLeft;
        public bool coverForward;
        public bool coverBack;

        private void Start()
        {
            if (status != 1)
            {
                _behaviour = transform.parent.GetComponent<GridBehaviour>();

                switch (status)
                {
                    case 2:
                        try
                        {
                            if (_behaviour.gridArray[x - 1, y].GetComponent<GridStat>().status == 1)
                            {
                                coverRight = true;
                                coverUpRight.SetActive(true);
                            }
                        }
                        catch(Exception){}

                        try
                        {
                            if (_behaviour.gridArray[x + 1, y].GetComponent<GridStat>().status == 1)
                            {
                                coverLeft = true;
                                coverUpLeft.SetActive(true);
                            }
                        }
                        catch(Exception){}

                        try
                        {
                            if (_behaviour.gridArray[x, y - 1].GetComponent<GridStat>().status == 1)
                            {
                                coverForward = true;
                                coverUpFront.SetActive(true);
                            }
                        }
                        catch(Exception){}

                        try
                        {
                            if (_behaviour.gridArray[x, y + 1].GetComponent<GridStat>().status == 1)
                            {
                                coverBack = true;
                                coverUpBack.SetActive(true);
                            }
                        }
                        catch(Exception){}

                        visualizeStatus[2].SetActive(true);
                        break;
                    case 3:
                        gameObject.tag = "Exit";
                        _door = GameObject.FindGameObjectWithTag("Door");
                        visualizeStatus[3].SetActive(true);
                        break;
                    case 4:
                        gameObject.tag = "Hackport";
                        visualizeStatus[1].SetActive(true);
                        break;
                }
                visualizeStatus[0].SetActive(true);
            }
        }

        public void Open(CameraBehaviour cameraBehaviour, GameObject hacker)
        {
            _door.GetComponent<DoorFunctions>().Open(cameraBehaviour, hacker);
        }
    }
}

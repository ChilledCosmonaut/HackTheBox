using System;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Grid
{
    public class GridBehaviour : MonoBehaviour
    {
        public int rows = 10;
        public int colums = 10; //Legt Größe des gesamten Grids fest
        public int scale = 1; //Legt die Einzel Größe fest
        public GameObject gridPrefab; //Zu instanzierendes Objekt
        public Vector3 leftBottomLocation = new Vector3(0, 0, 0);//Linker Unterer Startwert für das grid
        private CameraBehaviour _cameraBehaviour;
        public int maxRange; //Maximale gehbare distanz für Player
        private bool _reachable;
        public GameObject movementUnit;
        private bool _movement;
        public int speed;
        public TextAsset level;
        private Quaternion _targetRotation;
        private bool _rotateNeeded = true;

        //for initiating Grid
        public GameObject[,] gridArray;
        private String[][] _gridStatus;

        //player coordinates
        public int startX;
        public int startY;

        //Goal coordinates
        public int endX;
        public int endY;

        public List<GameObject> path = new List<GameObject>();
        private static readonly int Walking = Animator.StringToHash("walking");

        // Use this for initialization
        void Start()
        {
            _gridStatus = new string[colums][];
            ReadCsvFile();
            gridArray = new GameObject[colums, rows];
            if (gridPrefab)
                GenerateGrid();
            else print("Missing gridPrefab, please assing accordingly."); // Find gameObject with name "MyText"
            _cameraBehaviour = GameObject.Find("Camera").GetComponent<CameraBehaviour>();
            //actionCounter = GameObject.Find("ActionCounter"); // Assign new string to "Text" field in that component
        }

        private void Update()
        {
            //print(path.Count);
            if (_movement)
            {
                if (path[path.Count - 1].transform.position.Equals(movementUnit.transform.position))
                {
                    path.RemoveAt(path.Count - 1);

                    if (path.Count <= 0)
                    {
                        _movement = false;
                        UnitStat stat = movementUnit.GetComponent<UnitStat>();
                        Utility utility = movementUnit.GetComponent<Utility>();
                        if (utility.anim != null)
                        {
                            utility.anim.SetBool(Walking,false);
                        }
                        stat.actions -= 1;
                        stat.moving = false;
                        movementUnit = null;
                        _rotateNeeded = true;
                    }
                }
                else
                {
                    float step = speed * Time.deltaTime; // calculate distance to move
                    //print(path[path.Count - 1].GetComponent<GridStat>().x + " " + path[path.Count - 1].GetComponent<GridStat>().y);
                    movementUnit.transform.position = Vector3.MoveTowards(movementUnit.transform.position, path[path.Count - 1].transform.position, step);
                    if (_rotateNeeded)
                    {
                        Quaternion currentRotation = transform.rotation;
                        movementUnit.transform.LookAt(path[path.Count - 1].transform);
                        var transform1 = transform;
                        _targetRotation = transform1.rotation;
                        transform1.rotation = currentRotation;
                    }
                    if (!(_targetRotation == transform.rotation))
                    {
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 800 * Time.deltaTime);
                    }
                    _cameraBehaviour.CameraSwitch(movementUnit);
                }
            }
        }
    
        public List<GameObject> PossibleTiles(GameObject unit, UnitStat stat)
        {
            startX = (int) stat.currentPosition.x;
            startY = (int) stat.currentPosition.y;
            maxRange = stat.walkSpeed;
            List<GameObject> tiles = new List<GameObject>();
            ClearPath(stat.currentGrid);
            InitialSetup();
            for (int step = 1; step < maxRange + 1; step++)
            {
                foreach (GameObject obj in gridArray)
                {
                    if (obj && obj.GetComponent<GridStat>().visited == step - 1) //Does obj exist, if so it its visited Number = 1?
                    {
                        TestFourDirections(obj.GetComponent<GridStat>().x, obj.GetComponent<GridStat>().y, step);//Überprüft ob obj in alle Richtungen existieren und schaut ob diese schon besucht sind
                        tiles.Add(obj);
                    }
                }
            }

            return tiles;
        }
    
        public void PossiblePaths(GameObject unit, UnitStat stat)
        {
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                startX = (int) stat.currentPosition.x;
                startY = (int) stat.currentPosition.y;
                maxRange = stat.walkSpeed;

                if (Physics.Raycast(ray, out hit) && hit.transform.gameObject != null && (hit.transform.gameObject.tag.Equals("Grid") || hit.transform.gameObject.tag.Equals("Exit") || hit.transform.gameObject.tag.Equals("Hackport")))
                {
                    GridStat hitcube = hit.transform.gameObject.GetComponent<GridStat>();
                    ClearPath(stat.currentGrid);
                    endX = hitcube.x;
                    endY = hitcube.y;
                    SetDistance();
                    SetPath(stat.currentGrid);
                    PaintPath();
            
                }
                else
                {
                    ClearPath(stat.currentGrid);
                    SetDistance();
                }
            }
        }
    
        public Vector2 Move(GameObject unit, GridStat hitcube, Vector2 start, UnitStat stat)
        {
            endX = hitcube.x;
            endY = hitcube.y;
            startX = (int) start.x;
            startY = (int) start.y;
            maxRange = stat.walkSpeed;
            GameObject startPoint = gridArray[startX, startY];
            SetDistance();
            SetPath(stat.currentGrid);
            //print("Unit: " + unit.name + " tries to move to: X=" + endX + " Y=" + endY);
            if (_reachable)
            {
                MoveUnit(unit);
                ClearPath(startPoint);
                Vector2 endPoint = new Vector2(endX,endY);
                startPoint.GetComponent<GridStat>().taken = false;
                hitcube.taken = true;
                stat.currentGrid = hitcube.gameObject;
                return endPoint;
            }
            else
            {
                path.Clear(); 
                return start;
            }
        }

        public List<GameObject> WayBack(GridStat hitcube, Vector2 start)
        {
            endX = hitcube.x;
            endY = hitcube.y;
            startX = (int) start.x;
            startY = (int) start.y;
            maxRange = 50;
            GameObject startPoint = gridArray[startX, startY];
            SetDistance();
            SetPath(startPoint);
            List<GameObject> tiles = new List<GameObject>();
            foreach (GameObject part in path)
            {
                tiles.Add(part);
            }
            path.Clear();
            return tiles;
        }

        /*Genertates Grid
     for 3rd Dimension add another "for loop" and 
     create another variable eg. hight
     p.s siehe Kommentar sektion des Videos effizientere Methode*/
        void GenerateGrid()
        {
            for (int i = 0; i < colums; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    GameObject obj = Instantiate(gridPrefab, new Vector3(leftBottomLocation.x + scale * i, leftBottomLocation.y, leftBottomLocation.z + scale * j), Quaternion.identity);
                    obj.transform.SetParent(gameObject.transform);
                    obj.GetComponent<GridStat>().x = i;
                    obj.GetComponent<GridStat>().y = j;
                    int cacheInt;
                    if(Int32.TryParse(_gridStatus[i][j],out cacheInt))
                    {
                        obj.GetComponent<GridStat>().status = cacheInt;
                    }
                    gridArray[i, j] = obj;
                }
            }
        }
        //Set the distance between Player and all Tiles. Player Tile is the only tile with step 0 so the Algorithm knows where to start
        void SetDistance()
        {
            InitialSetup();
            for (int step = 1; step < maxRange + 1; step++)
            {
                //print(step);
                foreach (GameObject obj in gridArray)
                {
                    if (obj && obj.GetComponent<GridStat>().visited == step - 1) //Does obj exist, if so it its visited Number = 1?
                    {
                        GridStat stat = obj.GetComponent<GridStat>();
                        TestFourDirections(stat.x, stat.y, step);//Überprüft ob obj in alle Richtungen existieren und schaut ob diese schon besucht sind
                        stat.visualizeStatus[0].SetActive(false);
                        stat.possible.SetActive(true);
                    }
                }
            }
        }

        public void InitialSetup()
        {
            //every point on the grid is now labled as "-1"
            foreach (GameObject obj in gridArray)
            {
                obj.GetComponent<GridStat>().visited = -1;
            }
            //Unit Start is now 0 ->each adjacend field will be later on labled as +1
            gridArray[startX, startY].GetComponent<GridStat>().visited = 0;
        }

        void TestFourDirections(int x, int y, int step)
        {
            if (TestDirection(x, y, -1, 1))
            {
                SetVisited(x, y + 1, step);
            }
            if (TestDirection(x, y, -1, 2))
            {
                SetVisited(x + 1, y, step);
            }
            if (TestDirection(x, y, -1, 3))
            {
                SetVisited(x, y - 1, step);
            }
            if (TestDirection(x, y, -1, 4))
            {
                SetVisited(x - 1, y, step);
            }
        }

        //Testing which point to go, originating from player is eligable for travel/ can be traversed 
        bool TestDirection(int x, int y, int step, int direction)
        {
            //1 is up, 2 is right, 3 is down, 4 is left
            switch (direction)
            {
                case 4:
                    GridStat targetGridStat4;
                    if (x - 1 > -1 && gridArray[x - 1, y] && (targetGridStat4 = gridArray[x - 1, y].GetComponent<GridStat>()).visited == step && targetGridStat4.status != 1 && !targetGridStat4.taken)
                        return true;
                    else
                    {
                        return false;
                    }

                case 3:
                    GridStat targetGridStat3;
                    if (y - 1 > -1 && gridArray[x, y - 1] && (targetGridStat3 = gridArray[x, y - 1].GetComponent<GridStat>()).visited == step && targetGridStat3.status != 1 && !targetGridStat3.taken)
                        return true;
                    else
                    {
                        return false;
                    }

                case 2:
                    GridStat targetGridStat2;
                    if (x + 1 < colums && gridArray[x + 1, y] && (targetGridStat2 = gridArray[x + 1, y].GetComponent<GridStat>()).visited == step && targetGridStat2.status != 1 && !targetGridStat2.taken)
                        return true;
                    else
                    {
                        return false;
                    }

                case 1:
                    GridStat targetGridStat1;
                    if (y + 1 < rows && gridArray[x, y + 1] && (targetGridStat1 = gridArray[x, y + 1].GetComponent<GridStat>()).visited == step && targetGridStat1.status != 1 && !targetGridStat1.taken)
                        return true;
                    else
                    {
                        return false;
                    }
            }
            return false;
        }
    
        void SetPath(GameObject startPoint)
        {
            GridStat startStat = startPoint.GetComponent<GridStat>();
            startStat.taken = false;
            int step;
            int x = endX;
            int y = endY;
            List<GameObject> tempList = new List<GameObject>();
            path.Clear();
            //Überprüft ob Ziel in GridArray enthalten und ob es unter der maximalen anzahl an schritte liegt
            //print(endX + " " +  endY);
            //print("Statuy: " + gridArray[endX, endY].GetComponent<GridStat>().visited);
            if (gridArray[endX, endY] && gridArray[endX, endY].GetComponent<GridStat>().visited > 0 && gridArray[endX, endY].GetComponent<GridStat>().visited < maxRange) 
            {
                path.Add(gridArray[x, y]);
                step = gridArray[x, y].GetComponent<GridStat>().visited - 1; //step - 1
                _reachable = true;
            }
            else
            {
                _reachable = false;
                return;
            }
            for (; step > -1; step--)
            {
                if (TestDirection(x, y, step, 1))// Letzte Zahl steht hier für die Richtung
                {
                    tempList.Add(gridArray[x, y + 1]); //Added alle Tiles die in Frage kommen
                }
                if (TestDirection(x, y, step, 2))
                {
                    tempList.Add(gridArray[x + 1, y]);
                }
                if (TestDirection(x, y, step, 3))
                {
                    tempList.Add(gridArray[x, y - 1]);
                }
                if (TestDirection(x, y, step, 4))
                {
                    tempList.Add(gridArray[x - 1, y]);
                }

                GameObject tempObj = FindClosest(gridArray[endX, endY].transform, tempList); // Sucht hier das nahliegendste Tile raus und added es zur Liste
                path.Add(tempObj);// Path ist der weg verkehrt herum
                //print("Path Count: " + path.Count);
                x = tempObj.GetComponent<GridStat>().x;
                y = tempObj.GetComponent<GridStat>().y;
                tempList.Clear();
            }

            startStat.taken = true;
        }
 
        void SetVisited(int x, int y, int step)
        {
            GameObject grid = gridArray[x, y];
            GridStat stat = grid.GetComponent<GridStat>();
            if (grid)
            {
                stat.visited = step; //setzt frei Felder auf beschritten mit ihrer respektiven Laufzahl
            }
            
        }
        GameObject FindClosest(Transform targetLocation, List<GameObject> list)
        {
            float currentDistance = scale * rows * colums;
            int indexNumber = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
                {
                    currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                    indexNumber = i;
                }
            }
            return list[indexNumber];
        }
        void PaintPath()
        {
            foreach (GameObject obj in path)
            {
                GridStat stat = obj.GetComponent<GridStat>();
                stat.visualizeStatus[0].SetActive(false);
                stat.possible.SetActive(false);
                stat.path.SetActive(true);
            }
        }
    
        void MoveUnit(GameObject unit)
        {
            unit.GetComponent<UnitStat>().moving = true;
            Utility utility = unit.GetComponent<Utility>();
            if (utility.anim != null)
            {
                utility.anim.SetBool(Walking, true);
            }
            _movement = true;
            movementUnit = unit;
        }
    
        void ClearPath(GameObject last)
        {
            foreach (GameObject obj in gridArray)
            {
                GridStat stat = obj.GetComponent<GridStat>();
            
                if (stat.status != 1)
                {
                    stat.visualizeStatus[0].SetActive(true);
                    stat.path.SetActive(false);
                    stat.possible.SetActive(false);
                }
            }

            GridStat lastStat = last.GetComponent<GridStat>();
            lastStat.visualizeStatus[0].SetActive(true);
            lastStat.path.SetActive(false);
            lastStat.possible.SetActive(false);
        }

        void ReadCsvFile()
        {
            String[] cache  = level.text.Split(new[] {"\n"}, StringSplitOptions.None);

            for (int i = 0; i < _gridStatus.Length; i++)//Geht durch jede Zeile durch bis maximal colum
            {
                _gridStatus[i] = cache[i].Split(';'); //Cache wir an den Kommas aufgeteilt und als array in gridStatus gespeichert
            }
        }

        public void SpawnUnits(GameObject unit, Vector2 goal)
        {
            unit.transform.position = new Vector3(scale * goal.x, 0.0f , scale * goal.y);
        }
    }
}
using Grid;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Units
{
    public class UnitStat : MonoBehaviour
    {
        public Vector2 currentPosition;
        [HideInInspector]
        public GameController controller;
        public int walkSpeed = 8;
        public int hackLife = 2;
        public int damage;
        public int actions = 2; //Anzahl der Aktionspunkte
        public int healthPoints;
        public int halfCover;
        public int optimalRange;
        public int maxAttackRange;
        public int unitType;
        [HideInInspector]
        public int rotationStep = 400;
        [HideInInspector]
        public bool rotation;
        [HideInInspector]
        public bool moving;
        [HideInInspector]
        public bool acting;
        [HideInInspector]
        public Quaternion targetRotation;
        public PlayerController playerController;
        public GameObject currentGrid;
        public GameObject[] playerHealth = new GameObject[5];
        public GameObject[] enemyHealth = new GameObject[5];
        public GameObject[] hackHealth = new GameObject[2];
        public GameObject hackBar;
        
        private EnemyBehaviour _ki;
        private GridBehaviour _behaviour;
    
        void Start()
        {
            _behaviour = GameObject.Find("Grid").GetComponent<GridBehaviour>();
            HealthUpdate();
            controller = GameObject.Find("Battlefield").GetComponent<GameController>();
        }

        private void Update()
        {
            if (rotation)
            {
                if (targetRotation.Equals(transform.rotation))
                {
                    rotation = false;
                }
                else
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationStep * Time.deltaTime);
                    //print(gameObject.name + " rotating");
                }
            }
        }

        public void SetGrid()
        {
            currentGrid = _behaviour.gridArray[(int) currentPosition.x, (int) currentPosition.y];
            currentGrid.GetComponent<GridStat>().taken = true;
        }

        public void HealthUpdate()
        {
            if (healthPoints <= 0)
            {
                if (gameObject.CompareTag("Player"))
                {
                    if (unitType == 0)
                    {
                        SceneManager.LoadScene(controller.gameOver);
                    }
                    else
                    {
                        controller.playerList.Remove(gameObject);
                        currentGrid.GetComponent<GridStat>().taken = false;
                        Destroy(gameObject);
                        return;
                    }
                }
                else
                {
                    controller.enemyList.Remove(gameObject);
                    currentGrid.GetComponent<GridStat>().taken = false;
                    Destroy(gameObject);
                    return;
                }
            }
            if (healthPoints > playerHealth.Length)
            {
                healthPoints = playerHealth.Length;
            }
            if (gameObject.CompareTag("Player"))
            {
                foreach (GameObject health in playerHealth)
                {
                    health.SetActive(false);
                }
                foreach (GameObject health in enemyHealth)
                {
                    health.SetActive(false);
                }
                for (int i = 0; i < healthPoints; i++)
                {
                    playerHealth[i].SetActive(true);
                }
            }
            else
            {
                foreach (GameObject health in playerHealth)
                {
                    health.SetActive(false);
                }
                foreach (GameObject health in enemyHealth)
                {
                    health.SetActive(false);
                }
                for (int i = 0; i < healthPoints; i++)
                {
                    enemyHealth[i].SetActive(true);
                }
            }
        }
        
        public void HackUpdate()
        {
            foreach (GameObject health in hackHealth)
            {
                health.SetActive(false);
            }

            for (int i = 0; i < hackLife; i++)
            {
                hackHealth[i].SetActive(true);
            }
            
            
        }
        
        public void Exit() //Setzt sicherheitshalber alles auf Null
        {
            if (gameObject.CompareTag("Player"))
            {
                playerController.DeactivateUi();
            }
        }
    }
}

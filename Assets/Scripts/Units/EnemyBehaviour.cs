using System;
using System.Collections.Generic;
using Grid;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Units
{
    public class EnemyBehaviour : MonoBehaviour
    {
        protected UnitStat stat;
        protected GridBehaviour grid;
        private IUtility _utility;
        protected List<int> Distance = new List<int>();
        protected bool inCover;
        protected bool enemyDetected;
        protected bool coverNotReachable;

        public void Start()
        {
            grid = GameObject.Find("Grid").GetComponent<GridBehaviour>();
            stat = gameObject.GetComponent<UnitStat>();
            grid.SpawnUnits(gameObject, stat.currentPosition);
            _utility = gameObject.GetComponent<IUtility>();
        }

        public void EnemyAction(List<GameObject> players, List<GameObject> enemies)
        {
            Distance.Clear();
            List<GameObject> tiles = grid.PossibleTiles(gameObject,stat);
            
            if (!(stat.moving || stat.acting))
            {
                List<GameObject> targets = SearchEnemies(players);

                if (targets.Count > 0)
                {
                    List<GameObject> cover = FindCover(tiles);
                
                    if (!inCover)
                    {
                        MoveToCover(cover, targets);
                    }
                    else
                    {
                        AttackUnit(targets);
                    }
                }
                else
                {
                    stat.actions -= 2;
                }
            }
        }

        public bool ActionsLeft()
        {
            
            if (stat.actions <= 0)
            {
                return false;
            }

            return true;
        }

        public void ResetActions()
        {
            stat.actions = 2;
            enemyDetected = false;
            coverNotReachable = false;
        }

        protected List<GameObject> SearchEnemies(List<GameObject> players)
        {
            List<GameObject> targets = new List<GameObject>();

            for (int i = 0; i < players.Count; i++)
            {

                UnitStat playerstat = players[i].GetComponent<UnitStat>();

                Distance.Add((Math.Abs((int) (stat.currentPosition.x - playerstat.currentPosition.x))) +
                             (Math.Abs((int) (stat.currentPosition.y - playerstat.currentPosition.y))));

                if (Distance[i] < stat.maxAttackRange)
                {
                    targets.Add(players[i]);
                }
            }

            if (targets.Count > 0)
            {
                enemyDetected = true;
            }

            return targets;
        }

        protected List<GameObject> FindCover(List<GameObject> tiles)
        {
            List<GameObject> cover = new List<GameObject>();

            foreach (GameObject tile in tiles)
            {
                if (tile.GetComponent<GridStat>().status == 2)
                {
                    cover.Add(tile);
                }
            }

            return cover;
        }

        protected void MoveToCover(List<GameObject> cover, List<GameObject> targets)
        {
            if (cover.Count == 0) return;

            bool toRight = false;
            bool toLeft = false;
            bool toFront = false;
            bool toBack = false;

            foreach (GameObject unit in targets)
            {
                UnitStat unitStat = unit.GetComponent<UnitStat>();
                int horizontal = (int) unitStat.currentPosition.x - (int) stat.currentPosition.x;
                int vertical = (int) unitStat.currentPosition.y - (int) stat.currentPosition.y;

                if (horizontal > 0)
                {
                    toRight = true;
                }
                if (horizontal < 0)
                {
                    toLeft = true;
                }
                if (vertical > 0)
                {
                    toFront = true;
                }
                if (vertical < 0)
                {
                    toBack = true;
                }
            }

            GridStat bestCover = null;
            int amountCover = -1;

            foreach (GameObject tile in cover)
            {
                GridStat tileStat = tile.GetComponent<GridStat>();
                int currentAmount = 0;

                if (tileStat.coverLeft && toRight)
                {
                    currentAmount++;
                }
                if (tileStat.coverRight && toLeft)
                {
                    currentAmount++;
                }
                if (tileStat.coverBack && toFront)
                {
                    currentAmount++;
                }
                if (tileStat.coverForward && toBack)
                {
                    currentAmount++;
                }

                if (currentAmount > amountCover)
                {
                    amountCover = currentAmount;
                    bestCover = tileStat;
                }
            }

            if (bestCover != null)
            {
                Vector2 cache = grid.Move(gameObject, bestCover, stat.currentPosition, stat);
                
                if (!cache.Equals(stat.currentPosition))
                {
                    inCover = true;
                    stat.currentPosition = cache;
                }
                else
                {
                    coverNotReachable = true;
                }
            }
        }

        protected void AttackUnit(List<GameObject> players)
        {
            int currentLowest = Int32.MaxValue;
            GameObject currentTarget = null;
        
            for (int i = 0; i < players.Count; i++)
            {
                if (Distance[i] < stat.maxAttackRange)
                {
                    UnitStat targetStat = players[i].GetComponent<UnitStat>();
                    if (targetStat.healthPoints < currentLowest)
                    {
                        currentLowest = targetStat.healthPoints;
                        currentTarget = players[i];
                    }
                }
            }
            _utility.SetTarget(currentTarget);
            _utility.Attack();
        }
    }
}

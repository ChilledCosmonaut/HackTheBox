using System;
using System.CodeDom;
using System.Collections.Generic;
using Grid;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Units.Scharfschütze
{
    public class ScharfschützenBehaviour : EnemyBehaviour, IBehaviour
    {
        public int dangerZone;
        private List<GameObject> _toClose = new List<GameObject>();

        public new void EnemyAction(List<GameObject> players, List<GameObject> enemies)
        {
            if (!stat.moving)
            {
                print("action Sniper");
                Distance.Clear();
                List<GameObject> targets = SearchEnemies(players);

                if (targets.Count == 0)
                {
                    stat.actions -= 1;
                }
                else
                {
                    for (int i = 0; i < Distance.Count; i++)
                    {
                        if (Distance[i] <= dangerZone)
                        {
                            _toClose.Add(players[i]);
                        }
                    }

                    if (_toClose.Count >= 0)
                    {
                        List<GameObject> cache = grid.PossibleTiles(gameObject, stat);
                        
                        List<Vector2> possibleTiles = new List<Vector2>();

                        foreach (GameObject tile in cache)
                        {
                            
                            GridStat tileStat = tile.GetComponent<GridStat>();
                            possibleTiles.Add(new Vector2(tileStat.x, tileStat.y));
                        }

                        /*for (int i = (int) Stat.currentPosition.x-Stat.walkSpeed; i < (int)Stat.currentPosition.x+Stat.walkSpeed; i++)
                        {
                            for (int j = -(Stat.walkSpeed-Math.Abs(i)); j < Stat.walkSpeed-Math.Abs(i); j++)
                            {
                                possibleTiles.Add(new Vector2(i,j));
                            }
                        }*/

                        foreach (GameObject danger in _toClose)
                        {
                            UnitStat reference = danger.GetComponent<UnitStat>();

                            for (int i = (int) reference.currentPosition.x - reference.walkSpeed;
                                i < (int) reference.currentPosition.x + reference.walkSpeed;
                                i++)
                            {
                                for (int j = (int) reference.currentPosition.y - (reference.walkSpeed - Math.Abs(i));
                                    j < reference.walkSpeed - Math.Abs(i);
                                    j++)
                                {
                                    possibleTiles.Remove(new Vector2(i, j));
                                }
                            }
                        }

                        if (possibleTiles.Count != 0)
                        {
                            GridStat targetStat;
                            do
                            {
                                int newPos = Random.Range(0, possibleTiles.Count - 1);
                                GameObject targetPos = grid.gridArray[(int) possibleTiles[newPos].x,
                                    (int) possibleTiles[newPos].y];
                                targetStat = targetPos.GetComponent<GridStat>();
                            } while (targetStat.status == 1);

                            Vector2 cacheVector2 = grid.Move(gameObject, targetStat, stat.currentPosition, stat);

                            if (cacheVector2.Equals(stat.currentPosition))
                            {
                                AttackUnit(players);
                                print("attack");
                            }
                            else
                            {
                                stat.currentPosition = cacheVector2;
                                print("Move");
                            }
                        }
                    }
                    else
                    {
                        AttackUnit(players);
                        print("Attack");
                    }

                    _toClose.Clear();
                }
            }
        }
        
        public new void ResetActions()
        {
            stat.actions = 1;
            enemyDetected = false;
            coverNotReachable = false;
        }
    }
}

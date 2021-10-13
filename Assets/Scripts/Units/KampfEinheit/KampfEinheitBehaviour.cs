using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Units.KampfEinheit
{
    public class KampfEinheitBehaviour : EnemyBehaviour , IBehaviour
    {
        private int _patrolCounter;
        public List<Vector2> patrol;
        private Vector2 _wayBack;
        private bool _calculating;
        
        
        public new void EnemyAction(List<GameObject> players, List<GameObject> enemies)
        {
            Distance.Clear();
            List<GameObject> tiles = grid.PossibleTiles(gameObject,stat);
            
            if (!(stat.moving || stat.acting  || _calculating))
            {
                _calculating = true;
                //print(players.Count);
                List<GameObject> targets = SearchEnemies(players);
                //print("Data: " + tiles.Count + " " + targets.Count + " from " + gameObject);
                
                if (targets.Count != 0)
                {
                    List<GameObject> cover = FindCover(tiles);
                    
                    //print("Data: " + tiles.Count + " " + targets.Count + " " + cover.Count + " from " + gameObject);
                
                    if (!inCover && cover.Count > 0 && !coverNotReachable)
                    {
                        MoveToCover(cover, targets);
                        //print("Move to Cover " + gameObject.name);
                        _calculating = false;
                    }
                    else
                    {
                        AttackUnit(players);
                        //print("Attack " + gameObject.name);
                        _calculating = false;
                    }
                }
                else if(!enemyDetected)
                {
                    Patrol();
                    //print("Patrol " + gameObject.name);
                    _calculating = false;
                }
                else
                {
                    stat.actions -= 2;
                    _calculating = false;
                }
            }
        }

        private void Patrol()
        {
            if (patrol.Count <= 1)
            {
                //print("No Patrol route assigned");
                stat.actions -= 2;
                return;
            }

            GridStat nextGrid = grid.gridArray[(int) patrol[_patrolCounter].x, (int) patrol[_patrolCounter].y]
                .GetComponent<GridStat>();

            if (nextGrid.taken == true)
            {
                stat.actions -= 2;
                return;
            }

            //print("patrol coordinates: " + patrol[_patrolCounter].x + " " + patrol[_patrolCounter].y);

            Vector2 cache = grid.Move(gameObject, nextGrid, stat.currentPosition, stat);

            if (cache.Equals(stat.currentPosition))
            {
                //print("Could not find a way");
                List<GameObject> tiles = grid.WayBack(nextGrid, stat.currentPosition);
                GridStat tileStat = tiles[tiles.Count - stat.walkSpeed].GetComponent<GridStat>();
                _wayBack = new Vector2(tileStat.x, tileStat.y);
                //print(_wayBack.Count);
                GetBack();
            }
            else
            {
               // print("Could find a way");
                stat.currentPosition = cache;
                stat.actions -= 1;
                _patrolCounter = (_patrolCounter + 1) % patrol.Count;
            }
        }

        private void GetBack()
        {
            //print("From Unit: " + gameObject.name + " with WaybackCounter: " + _waybackCounter);
                
            GridStat nextGrid = grid.gridArray[(int) _wayBack.x, (int) _wayBack.y]
                .GetComponent<GridStat>();
                
            stat.currentPosition = grid.Move(gameObject, nextGrid, stat.currentPosition, stat);
            stat.actions -= 1;
        }
    }
}

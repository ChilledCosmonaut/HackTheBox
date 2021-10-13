using System;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Techniker
{
    public class TechnikerBehaviour : EnemyBehaviour, IBehaviour
    {
        private TechnikerUtility _utility;

        new void Start()
        {
            _utility = gameObject.GetComponent<TechnikerUtility>();
            base.Start();
        }

        public new void EnemyAction(List<GameObject> players, List<GameObject> enemies)
        {
            if (!stat.moving)
            {
                List<GameObject> targets = SearchEnemies(players);

                List<GameObject> friendlyTargets = Searchfriendlies(enemies);

                List<GameObject> tiles = grid.PossibleTiles(gameObject, stat);
                
                
                if (friendlyTargets.Count > 0)
                {
                    GameObject lowest = friendlyTargets[0];
                    UnitStat lowestStat = friendlyTargets[0].GetComponent<UnitStat>();

                    for (int i = 1; i < targets.Count; i++)
                    {
                        UnitStat currentStat = targets[i].GetComponent<UnitStat>();

                        if (currentStat.healthPoints < lowestStat.healthPoints)
                        {
                            lowest = targets[i];
                            lowestStat = currentStat;
                        }
                    }

                    _utility.SetTarget(lowest);
                    _utility.Heal();
                }
                else if (targets.Count != 0)
                {
                    List<GameObject> cover = FindCover(tiles);

                    if (!inCover)
                    {
                        MoveToCover(cover, targets);
                    }
                    else
                    {
                        AttackUnit(players);
                    }
                }
                else
                {
                    stat.actions -= 2;
                }
            }
        }

        private List<GameObject> Searchfriendlies(List<GameObject> enemies)
        {
            List<GameObject> targets = new List<GameObject>();

            for (int i = 0; i < enemies.Count; i++)
            {
                UnitStat stat = enemies[i].GetComponent<UnitStat>();

                Distance.Add((Math.Abs((int) (base.stat.currentPosition.x - stat.currentPosition.x))) +
                             (Math.Abs((int) (base.stat.currentPosition.y - stat.currentPosition.y))));

                if (Distance[i] < stat.maxAttackRange)
                {
                    targets.Add(enemies[i]);
                }
            }

            return targets;
        }
    }
}



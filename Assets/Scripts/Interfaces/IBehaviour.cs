using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBehaviour
{
    void EnemyAction(List<GameObject> players,List<GameObject> enemies);

    bool ActionsLeft();

    void ResetActions();
}

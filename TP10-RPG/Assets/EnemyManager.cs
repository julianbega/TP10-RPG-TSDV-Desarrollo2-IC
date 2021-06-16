using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] List<EnemyController> enemies;

    void Start()
    {
        foreach (var enemy in enemies)
        {
            enemy.OnDeath += RemoveEnemy;
        }
    }

    void RemoveEnemy(EnemyController e)
    {
        enemies.Remove(e);
        if(enemies.Count == 0)
        {
            Debug.Log("End Game");
        }
    }

}

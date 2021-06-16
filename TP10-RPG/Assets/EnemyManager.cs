using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] List<EnemyController> enemies;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var enemy in enemies)
        {
            enemy.OnDeath += RemoveEnemy;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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

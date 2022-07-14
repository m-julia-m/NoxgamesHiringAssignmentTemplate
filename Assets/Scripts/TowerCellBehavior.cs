using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCellBehavior : MonoBehaviour
{
    private GameObject enemy = null;

    public Vector3 GetPlayerPosition()
    {
        return transform.Find("PlayerPosition").position;
    }

    public Vector3 GetEnemyPosition()
    {
        return transform.Find("EnemyPosition").position;
    }

    public void SetEnemy(GameObject enemyCharacter)
    {
        enemy = enemyCharacter;

        if (enemy != null)
        {
            enemy.transform.parent = transform;
            enemy.transform.position = GetEnemyPosition();
        }
    }
    public GameObject GetEnemy()
    {
        return enemy;
    }
}

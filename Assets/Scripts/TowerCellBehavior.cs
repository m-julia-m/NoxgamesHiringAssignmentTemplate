using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TowerCellBehavior : MonoBehaviour
{
    public GameObject enemy;

    public Vector3 GetPlayerPosition()
    {
        return transform.Find("PlayerPosition").position;
    }

    public Vector3 GetEnemyPosition()
    {
        return transform.Find("EnemyPosition").position;
    }

    public GameObject GetEnemy()
    {
        return transform.Find("EnemyCharacter").gameObject;
    }

    //public void AssignCharacter(GameObject character)
    //{
    //    if (character.tag == "Player")
    //    {
    //        AssignPlayerCharacter(character);
    //    }
    //    else if (character.tag == "Enemy")
    //    {
    //        AssignEnemyCharacter(character);
    //    }
    //}

    /// <summary>
    /// Move enemy character to this tower cell.
    /// </summary>
    /// <param name="enemyCharacter">Enemy character to move here.</param>
    //public void AssignEnemyCharacter(GameObject enemyCharacter)
    //{
    //    enemy = enemyCharacter;
    //    enemy.transform.parent = this.transform;
    //    DOTween.Sequence().Append(enemy.transform.DOMove(transform.Find("EnemyPosition").position, 2)).WaitForCompletion();
    //}

    /// <summary>
    /// Move player character to this tower cell.
    /// </summary>
    /// <param name="playerCharacter">Player character to move here.</param>
    public void AssignPlayerCharacter(GameObject playerCharacter)
    {
        DOTween.Sequence().Append(playerCharacter.transform.DOMove(transform.Find("PlayerPosition").position, 2)).WaitForCompletion();
    }

}

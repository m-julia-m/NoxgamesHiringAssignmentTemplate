using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CharacterBehavior : MonoBehaviour
{
    private int levelPoints = 1;
    private TextMeshPro levelLabel;

    // Start is called before the first frame update
    void Start()
    {
        UpdateLevelLabel();
    }

    void UpdateLevelLabel()
    {
        levelLabel = GetComponentInChildren<TextMeshPro>();
        levelLabel.SetText(levelPoints.ToString());
    }

    public void SetLevel(int newLevelPoints)
    {
        levelPoints = newLevelPoints;
        UpdateLevelLabel();
    }

    public int GetLevel()
    {
        return levelPoints;
    }

    // Return true if this character has a higher level and won.
    /// <summary>
    /// Fight enemy, transfer level points from higher-level opponent to lower level opponent.
    /// In case of level equivalence, enemy wins.
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns>True if this opponent won, false if enemy opponent won.</returns>
    public bool FightOpponent(GameObject enemy)
    {
        int enemyLevelPoints = enemy.GetComponent<CharacterBehavior>().GetLevel();

        // FightAnimation().WaitForCompletion();

        if (enemyLevelPoints < levelPoints)
        {
            int temp = levelPoints;
            SetLevel(levelPoints + enemyLevelPoints);
            enemy.GetComponent<CharacterBehavior>().SetLevel(enemyLevelPoints - temp);
            
            return true;
        }
        else
        {
            int temp = levelPoints;
            SetLevel(levelPoints - enemyLevelPoints);
            enemy.GetComponent<CharacterBehavior>().SetLevel(enemyLevelPoints + temp);

            return false;
        }
    }

    //private Sequence FightAnimation()
    //{
    //    Sequence mySequence = DOTween.Sequence().Append(transform.DOShakeScale(1, 1, 1, 0.5f, true));
    //    return mySequence;

    //}
}

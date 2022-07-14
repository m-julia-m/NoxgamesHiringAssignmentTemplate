using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterBehavior : MonoBehaviour
{
    private int levelPoints = 1;
    private TextMeshPro levelLabel;

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

    /// <summary>
    /// Fight opponent - transfer level points from higher-level character to lower level character.
    /// In case of level equivalence, enemy wins.
    /// </summary>
    /// <param name="opponent">Character to fight.</param>
    /// <returns>True if this character won, false if opponent won.</returns>
    public bool FightOpponent(GameObject opponent)
    {
        int enemyLevelPoints = opponent.GetComponent<CharacterBehavior>().GetLevel();

        if (enemyLevelPoints < levelPoints)
        {
            int temp = levelPoints;
            SetLevel(levelPoints + enemyLevelPoints);
            opponent.GetComponent<CharacterBehavior>().SetLevel(enemyLevelPoints - temp);
            
            return true;
        }
        else
        {
            int temp = levelPoints;
            SetLevel(levelPoints - enemyLevelPoints);
            opponent.GetComponent<CharacterBehavior>().SetLevel(enemyLevelPoints + temp);

            return false;
        }
    }
}

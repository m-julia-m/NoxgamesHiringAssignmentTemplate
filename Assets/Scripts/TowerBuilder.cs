using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TowerBuilder : MonoBehaviour
{
    public GameObject towerBottomPf;
    public GameObject towerCellPf;
    public GameObject towerTopPf;

    private GameObject towerBottom;
    private GameObject towerTop;
    private List<GameObject> towerCells = new List<GameObject>();

    public void AddTowerCell()
    {
        Vector3 position = PositionOnTopOf(HasTowerCells() ? GetTopTowerCell() : towerBottom, towerCellPf);
        towerCells.Add(Instantiate(towerCellPf, position, Quaternion.identity));

        UpdateTowerTopPosition();
    }

    public void UpdateTowerTopPosition()
    {
        towerTop.transform.position = PositionOnTopOf(HasTowerCells() ? GetTopTowerCell() : towerBottom, towerTop);
    }

    public void RemoveTowerCell(GameObject towerCell)
    {
        // Move upper tower cellss down.
        for (int i = towerCells.Count - 1; i > towerCells.IndexOf(towerCell); i--)
        {
            towerCells[i].transform.position = towerCells[i - 1].transform.position;
        }

        // Remove and destroy tower cell.
        towerCells.Remove(towerCell);
        Destroy(towerCell);

        // Move tower top.
        UpdateTowerTopPosition();
    }

    Vector3 PositionOnTopOf(GameObject baseObject, GameObject newObject)
    {
        Vector3 position = baseObject.transform.position;
        position.y += baseObject.GetComponent<SpriteRenderer>().size.y / 2;
        position.y += newObject.GetComponent<SpriteRenderer>().size.y / 2;
        return position;
    }

    public void BuildTower(Vector3 position, int cellCount)
    {
        towerBottom = Instantiate(towerBottomPf, position, Quaternion.identity);
        towerTop = Instantiate(towerTopPf, PositionOnTopOf(towerBottom, towerTopPf), Quaternion.identity);

        for (int i = 0; i < cellCount; i++)
        {
            AddTowerCell();
        }
    }

    public void AssignPlayerCharacter(GameObject character)
    {
        towerCells[0].GetComponent<TowerCellBehavior>().AssignPlayerCharacter(character);
    }

    public void AssignEnemyCharacters(List<GameObject> characters)
    {
        for (int i = 0; i < Mathf.Min(towerCells.Count, characters.Count); i++)
        {
            characters[i].transform.parent = towerCells[i].transform;
            characters[i].transform.position = towerCells[i].GetComponent<TowerCellBehavior>().GetEnemyPosition();
        }
    }

    public IEnumerable<GameObject> GetTowerCells()
    {
        return towerCells;
    }

    public GameObject GetTopTowerCell()
    {
        return HasTowerCells() ? towerCells[towerCells.Count - 1] : null;
    }

    public bool HasTowerCells()
    {
        return towerCells.Count > 0;
    }
}

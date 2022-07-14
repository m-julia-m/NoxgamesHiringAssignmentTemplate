using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    public GameObject towerBottomPf;
    public GameObject towerCellPf;
    public GameObject towerTopPf;

    private GameObject towerBottom;
    private GameObject towerTop;
    private List<GameObject> towerCells;

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

    /// <summary>
    /// Remove and destroy tower cell. Update position of other tower parts accordingly.
    /// </summary>
    /// <param name="towerCell">Tower cell to remove.</param>
    public void RemoveTowerCell(GameObject towerCell)
    {
        // Move upper tower cells down.
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

    /// <summary>
    /// Calculates position for new object based on position of the base object accroding to their sprites,
    /// so that when rendered, new object sits on top of the base object.
    /// Assumes that transform points are in the center of the sprites.
    /// </summary>
    /// <param name="baseObject">Base - bottom object.</param>
    /// <param name="newObject">New - top object.</param>
    /// <returns>Position for new object on top of base object.</returns>
    Vector3 PositionOnTopOf(GameObject baseObject, GameObject newObject)
    {
        Vector3 position = baseObject.transform.position;
        position.y += baseObject.GetComponent<SpriteRenderer>().size.y / 2;
        position.y += newObject.GetComponent<SpriteRenderer>().size.y / 2;
        return position;
    }

    /// <summary>
    /// Instantiate a tower botoom, tower cells and tower top on specified position.
    /// </summary>
    /// <param name="position">Position of tower bottom part.</param>
    /// <param name="cellCount">Number of tower cells.</param>
    public void BuildTower(Vector3 position, int cellCount)
    {
        towerBottom = Instantiate(towerBottomPf, position, Quaternion.identity);
        towerTop = Instantiate(towerTopPf, PositionOnTopOf(towerBottom, towerTopPf), Quaternion.identity);
        towerCells = new List<GameObject>();

        for (int i = 0; i < cellCount; i++)
        {
            AddTowerCell();
        }
    }

    public List<GameObject> GetTowerCells()
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

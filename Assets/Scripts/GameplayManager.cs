using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class GameplayManager : MonoBehaviour
{
    // UI Screens.
    public GameObject gameOverScreen;
    public GameObject levelClearedScreen;

    // Gameplay content.
    public GameObject playerCharacterPf;
    public GameObject enemyCharacterPf;
    public List<AudioSource> FightSounds;

    public Vector3 playerTowerPosition;
    public int playerTowerStartingCellCount;

    public Vector3 enemyTowerPosition;
    public int enemyTowerStartingCellCount;

    private TowerBuilder enemyTowerBuilder;
    private TowerBuilder playerTowerBuilder;

    private GameObject player;

    /// <summary>
    /// When true, player can tap tower cells to move his player character.
    /// </summary>
    private bool tapControlActive = false;

    public void InitialSetup()
    {
        int startingPlayerLevel = Random.Range(2, 10);

        // Build player tower
        playerTowerBuilder = GameObject.Find("PlayerTowerBuilder").GetComponent<TowerBuilder>();
        playerTowerBuilder.BuildTower(playerTowerPosition, playerTowerStartingCellCount);
        SpawnPlayer(startingPlayerLevel);


        // Build enemy tower
        enemyTowerBuilder = GameObject.Find("EnemyTowerBuilder").GetComponent<TowerBuilder>();
        enemyTowerBuilder.BuildTower(enemyTowerPosition, enemyTowerStartingCellCount);
        SpawnEnemies(startingPlayerLevel);

        // Activate controls.
        tapControlActive = true;
    }

    void Update()
    {
        PlayerTouchControl();
        PlayerMouseControl();
    }

    void PlayerMouseControl()
    {
        if (tapControlActive && Input.GetMouseButtonDown(0))
        {
            ProcessTap(Input.mousePosition);
        }
    }

    void PlayerTouchControl()
    {
        if (tapControlActive && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ProcessTap(Input.touches[0].position);
        }
    }

    void ProcessTap(Vector3 tapPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(tapPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "EnemyTowerCell")
            {
                tapControlActive = false;

                Vector3 dest = hit.transform.gameObject.GetComponent<TowerCellBehavior>().GetPlayerPosition();
                MovePlayerAnimation(dest).OnComplete(() =>
                {
                    Fight(hit.transform.gameObject);
                });
            }
        }
    }
    
    Sequence MovePlayerAnimation(Vector3 dest)
    {
        return DOTween.Sequence()
            .Append(player.transform.DOMove(dest, 1));
    }

    Sequence FightAnimation(GameObject player, GameObject enemy)
    {
        return DOTween.Sequence()
                .Append(player.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0), 1, 1, 0.5f))
                .Join(enemy.transform.DOPunchScale(new Vector3(0.3f, 0.2f, 0), 1, 1, 0.5f));

    }

    void Fight(GameObject towerCell)
    {
        tapControlActive = false;

        GameObject enemy = towerCell.GetComponent<TowerCellBehavior>().GetEnemy();
        if (enemy == null)
        {
            return;
        }

        bool playerWon = player.GetComponent<CharacterBehavior>().FightOpponent(enemy);

        FightSounds[Random.Range(0, FightSounds.Count)].Play();
        FightAnimation(player, enemy).OnComplete(() =>
        {
            if (playerWon)
            {
                // Transfer tower cell.
                enemyTowerBuilder.RemoveTowerCell(towerCell);
                playerTowerBuilder.AddTowerCell();

                // Move player to player tower.
                Vector3 dest = playerTowerBuilder.GetTopTowerCell().GetComponent<TowerCellBehavior>().GetPlayerPosition();
                MovePlayerAnimation(dest);
                tapControlActive = true;

                // Check winning condition.
                if (!enemyTowerBuilder.HasTowerCells())
                {
                    tapControlActive = false;
                    levelClearedScreen.SetActive(true);
                }
            }
            else
            {
                tapControlActive = false;
                gameOverScreen.SetActive(true);
            }
        });
    }

    /// <summary>
    /// Create player character of given level and move him to player tower.
    /// </summary>
    /// <param name="level">Starting player level.</param>
    void SpawnPlayer(int level)
    {
        player = Instantiate(playerCharacterPf);
        player.GetComponent<CharacterBehavior>().SetLevel(level);

        Vector3 dest = playerTowerBuilder.GetTopTowerCell().GetComponent<TowerCellBehavior>().GetPlayerPosition();
        MovePlayerAnimation(dest);
    }

    /// <summary>
    /// Spawns enemies in enemy tower cells. Enemy levels are generated based on player level to allow winning conditions.
    /// </summary>
    /// <param name="playerLevel">Starting player level.</param>
    void SpawnEnemies(int playerLevel)
    {
        List<GameObject> towerCells = enemyTowerBuilder.GetTowerCells();
        List<int> emptyCellsIndices = Enumerable.Range(0, towerCells.Count).ToList();

        for (int i = 0; i < towerCells.Count; i++)
        {
            // Calculate new enemy level.
            int enemyLevel = playerLevel - Random.Range(1, playerLevel / 2);
            playerLevel += enemyLevel;

            // Create enemy.
            GameObject enemy = Instantiate(enemyCharacterPf);
            enemy.GetComponent<CharacterBehavior>().SetLevel(enemyLevel);

            // Assign to a random empty enemy tower cell.
            int rand = Random.Range(0, emptyCellsIndices.Count);
            int chosenCellIndex = emptyCellsIndices[rand];
            towerCells[chosenCellIndex].GetComponent<TowerCellBehavior>().SetEnemy(enemy);
            emptyCellsIndices.RemoveAt(rand);
        }
    }

    public void SetTapControlActive(bool value)
    {
        tapControlActive = value;
    }
}

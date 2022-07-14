using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameplayManager : MonoBehaviour
{
    public GameObject playerCharacterPf;
    public GameObject enemyCharacterPf;

    public Vector3 playerTowerPosition;
    public int playerTowerStartingCellCount;

    public Vector3 enemyTowerPosition;
    public int enemyTowerStartingCellCount;

    private TowerBuilder enemyTowerBuilder;
    private TowerBuilder playerTowerBuilder;
    private GameObject player;
    private bool touchActive = false;

    // Start is called before the first frame update
    void Start()
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

        touchActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerTouchControl();
    }

    void PlayerTouchControl()
    {
        if (touchActive && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "EnemyTowerCell")
                {
                    touchActive = false;
                    Vector3 dest = hit.transform.gameObject.GetComponent<TowerCellBehavior>().GetPlayerPosition();
                    player.transform.DOMove(dest, 1).OnComplete(() =>
                    {
                        Fight(hit.transform.gameObject);
                        touchActive = true;
                    });
                }

                else if (hit.transform.tag == "PlayerTowerCell")
                {
                    touchActive = false;
                    Vector3 dest = hit.transform.gameObject.GetComponent<TowerCellBehavior>().GetPlayerPosition();
                    player.transform.DOMove(dest, 1).OnComplete(() =>
                    {
                        touchActive = true;
                    });
                }   
            }
        }
    }
    Sequence MovePlayerAnimation(Vector3 dest)
    {
        Sequence mySequence = DOTween.Sequence().Append(player.transform.DOMove(dest, 1));
        return mySequence;
    }

    Sequence FightAnimation(GameObject player, GameObject enemy)
    {
        return DOTween.Sequence()
                .Append(player.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0), 1, 1, 0.5f))
                .Join(enemy.transform.DOPunchScale(new Vector3(0.3f, 0.2f, 0), 1, 1, 0.5f));

    }

    void Fight(GameObject towerCell)
    {
        GameObject enemy = towerCell.GetComponent<TowerCellBehavior>().GetEnemy();

        if (enemy != null)
        {
            bool playerWon = player.GetComponent<CharacterBehavior>().FightOpponent(enemy);
            
            FightAnimation(player, enemy).OnComplete(() =>
            {
                if (playerWon)
                {
                    enemyTowerBuilder.RemoveTowerCell(towerCell);
                    playerTowerBuilder.AddTowerCell();

                    Vector3 dest = playerTowerBuilder.GetTopTowerCell().GetComponent<TowerCellBehavior>().GetPlayerPosition();
                    MovePlayerAnimation(dest);

                    if (!enemyTowerBuilder.HasTowerCells())
                    {
                        Debug.Log("Player won, CONGRATULATIONS.");
                    }
                }
                else
                {
                    Debug.Log("Player lost, GAME OVER.");
                }

            });
        }      
    }

    void SpawnPlayer(int level)
    {
        player = Instantiate(playerCharacterPf);
        player.GetComponent<CharacterBehavior>().SetLevel(level);
        Vector3 dest = playerTowerBuilder.GetTopTowerCell().GetComponent<TowerCellBehavior>().GetPlayerPosition();
        MovePlayerAnimation(dest);
    }

    void SpawnEnemies(int playerLevel)
    {
        List<GameObject> enemies = new List<GameObject>();
        IEnumerable<GameObject> enemyTowerCells = enemyTowerBuilder.GetTowerCells();

        // Generate enemy for each enemy tower cell based on playerLevel.
        foreach (GameObject towerCell in enemyTowerCells)
        {
            int enemyLevel = playerLevel - Random.Range(1, playerLevel / 2);
            playerLevel += enemyLevel;

            GameObject enemy = Instantiate(enemyCharacterPf);
            enemy.name = enemyCharacterPf.name; // Name is used in TowerCellBehavior to find enemy.
            enemy.GetComponent<CharacterBehavior>().SetLevel(enemyLevel);
            enemies.Add(enemy);
        }

        Shuffle(enemies, enemies.Count);

        // Asign enemies to tower cells.
        enemyTowerBuilder.AssignEnemyCharacters(enemies);
    }

    void Shuffle<T>(IList<T> list, int swapCount)
    {
        for (int i = 0; i < swapCount; i++)
        {
            Swap(list, Random.Range(0, list.Count), Random.Range(0, list.Count));
        }
    }

    void Swap<T>(IList<T> list, int i, int j)
    {
        var temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }

}

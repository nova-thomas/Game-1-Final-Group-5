using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject lizardPrefab;
    public GameObject plaguePrefab;
    public GameObject golemPrefab;
    public GameObject dragonPrefab;

    public int maxMembers;

    public enum EnemyType
    {
        Lizard,
        Plague,
        Golem,
        Dragon
    }

    [SerializeField]
    private EnemyType enemyToSpawn;

    [SerializeField]
    private float respawnDelay = 5f;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        SpawnEnemy();
    }

    void Update()
    {
        if (AllEnemiesDead())
        {
            StartCoroutine(RespawnAfterDelay());
        }
    }

    private void SpawnEnemy()
    {
        ClearSpawnedEnemies();
        GameObject prefab = GetPrefabForEnemyType();

        if (prefab != null)
        {
            if (enemyToSpawn == EnemyType.Lizard || enemyToSpawn == EnemyType.Plague)
            {
                
                for (int i = 0; i < maxMembers; i++)
                {
                    Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-4f, 4f), 0, Random.Range(-4f, 4f));
                    GameObject member = Instantiate(prefab, spawnPosition, transform.rotation);
                    spawnedEnemies.Add(member);
                }
            }
            else
            {
                GameObject enemy = Instantiate(prefab, transform.position, transform.rotation);
                spawnedEnemies.Add(enemy);
            }
        }
    }

    private GameObject GetPrefabForEnemyType()
    {
        switch (enemyToSpawn)
        {
            case EnemyType.Lizard:
                return lizardPrefab;
            case EnemyType.Plague:
                return plaguePrefab;
            case EnemyType.Golem:
                return golemPrefab;
            case EnemyType.Dragon:
                return dragonPrefab;
            default:
                return null;
        }
    }

    private bool AllEnemiesDead()
    {
        spawnedEnemies.RemoveAll(enemy => enemy == null);
        return spawnedEnemies.Count == 0;
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnEnemy();
    }

    private void ClearSpawnedEnemies()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        spawnedEnemies.Clear();
    }
}

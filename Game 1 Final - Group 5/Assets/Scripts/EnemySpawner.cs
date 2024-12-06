using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    private bool respawning = false; // Ensure coroutine isn't called multiple times

    void Start()
    {
        SpawnEnemy();
    }

    void Update()
    {
        if (AllEnemiesDead() && !respawning)
        {
            respawning = true;
            StartCoroutine(RespawnAfterDelay());
        }
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        ClearSpawnedEnemies();
        SpawnEnemy();
        respawning = false; // Allow future respawns
    }

    private void SpawnEnemy()
    {
        GameObject prefab = GetPrefabForEnemyType();
        if (prefab == null) return;

        if (enemyToSpawn == EnemyType.Lizard || enemyToSpawn == EnemyType.Plague)
        {
            for (int i = 0; i < maxMembers; i++)
            {
                Vector3 spawnPosition = GetValidSpawnPosition(transform.position, 4f);
                GameObject member = Instantiate(prefab, spawnPosition, transform.rotation);
                spawnedEnemies.Add(member);
                InitializeAgent(member);
            }
        }
        else
        {
            Vector3 spawnPosition = GetValidSpawnPosition(transform.position, 0f);
            GameObject enemy = Instantiate(prefab, spawnPosition, transform.rotation);
            spawnedEnemies.Add(enemy);
            InitializeAgent(enemy);
        }
    }

    private void InitializeAgent(GameObject enemy)
    {
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false; // Temporarily disable the agent until initialization
            agent.Warp(enemy.transform.position);
            agent.enabled = true;
            agent.SetDestination(enemy.transform.position);
        }
    }

    private GameObject GetPrefabForEnemyType()
    {
        switch (enemyToSpawn)
        {
            case EnemyType.Lizard: return lizardPrefab;
            case EnemyType.Plague: return plaguePrefab;
            case EnemyType.Golem: return golemPrefab;
            case EnemyType.Dragon: return dragonPrefab;
            default: return null;
        }
    }

    private bool AllEnemiesDead()
    {
        spawnedEnemies.RemoveAll(enemy => enemy == null);
        return spawnedEnemies.Count == 0;
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

    private Vector3 GetValidSpawnPosition(Vector3 origin, float range)
    {
        Vector3 randomOffset = new Vector3(Random.Range(-range, range), -3, Random.Range(-range, range));
        Vector3 tentativePosition = origin + randomOffset;

        if (NavMesh.SamplePosition(tentativePosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return origin;
    }
}
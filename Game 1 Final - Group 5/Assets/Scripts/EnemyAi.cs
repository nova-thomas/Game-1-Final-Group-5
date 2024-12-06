using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : Actor
{
    public NavMeshAgent agent;
    public Rigidbody myRig;

    public GameObject player;
    private Player playerScript;

    public LayerMask whatIsGround, whatIsPlayer;

    public float maxHealth;
    public HealthbarControl healthbar;

    public GameObject coins;
    public GameObject powerup;
    public float monsterDropHeight;

    public enum ElementType
    {
        None,
        Fire,
        Ice,
        Poison
    }

    [Header("Enemy Settings")]
    public ElementType enemyElementType; // Dropdown for element type
    

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    public bool alreadyAttacked;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public bool playedAmbient;
    public float timeBetweenAmbient;

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
        agent = GetComponent<NavMeshAgent>();
        myRig = GetComponent<Rigidbody>();
        maxHealth = health;
    }

    public void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    public void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    public void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }

    public void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void AmbientPlayed()
    {
        playedAmbient = false;
    }

    public void Die()
    {
        // Play death animation

        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        float baseDamage = GameManager.Instance.damage; // Get damage from GameManager

        switch (tag)
        {
            case "Regular Bullet":
                TakeDamage(baseDamage);
                break;
            case "Fire Bullet":
                if (enemyElementType == ElementType.Poison)
                { // Weakness
                    TakeDamage(baseDamage * 1.3 * GameManager.Instance.fireUpgrade);
                }
                else if (enemyElementType == ElementType.Fire)
                { // Resistance
                    TakeDamage(baseDamage * 0.8 * GameManager.Instance.fireUpgrade);
                }
                else
                {
                    TakeDamage(baseDamage * GameManager.Instance.fireUpgrade);
                }
                break;
            case "Ice Bullet":
                if (enemyElementType == ElementType.Fire)
                { // Weakness
                    TakeDamage(baseDamage * 1.3 * GameManager.Instance.iceUpgrade);
                }
                else if (enemyElementType == ElementType.Ice)
                { // Resistance
                    TakeDamage(baseDamage * 0.8 * GameManager.Instance.iceUpgrade);
                }
                else
                {
                    TakeDamage(baseDamage * GameManager.Instance.iceUpgrade);
                }
                break;
            case "Poison Bullet":
                if (enemyElementType == ElementType.Ice)
                { // Weakness
                    TakeDamage(baseDamage * 1.3 * GameManager.Instance.poisonUpgrade);
                }
                else if (enemyElementType == ElementType.Poison)
                { // Resistance
                    TakeDamage(baseDamage * 0.8 * GameManager.Instance.poisonUpgrade);
                }
                else
                {
                    TakeDamage(baseDamage * GameManager.Instance.poisonUpgrade);
                }
                break;
        }
    }

    private void TakeDamage(double amount)
    {
        health -= (int)amount;
        healthbar.UpdateHealthBar();

        if (health <= 0)
        {
            float randomValue = Random.Range(0f, 100f);

            if (randomValue <= 50f) // 50% chance for coins
            {
                SpawnItem(coins);
            }

            if (randomValue <= 30f) // 30% chance for powerup
            {
                SpawnItem(powerup);
            }


            Die();
        }
    }


    public float GetCurrentHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    private void SpawnItem(GameObject item)
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);

        Vector3 itemSpawnPos = new Vector3(
            transform.position.x + randomX,
            transform.position.y + monsterDropHeight,
            transform.position.z + randomZ
        );

        GameObject spawnedItem = Instantiate(item, itemSpawnPos, Quaternion.identity);
        Rigidbody rb = spawnedItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }
}
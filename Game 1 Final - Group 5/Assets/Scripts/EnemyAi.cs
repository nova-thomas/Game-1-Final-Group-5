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

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
        agent = GetComponent<NavMeshAgent>();
        myRig = GetComponent<Rigidbody>();
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

    public void Die()
    {
        // Play death animation

        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;

        switch (tag)
        {
            case "Regular Bullet":
                TakeDamage(playerScript.damage);
                break;
            case "Fire Bullet":
                if (enemyElementType == ElementType.Poison)
                { // Weakness
                    TakeDamage(playerScript.damage * 1.3);
                } else if (enemyElementType == ElementType.Fire) 
                { // Resistance
                    TakeDamage(playerScript.damage * 0.8);
                } else 
                {
                    TakeDamage(playerScript.damage);
                }
                break;
            case "Ice Bullet":
                if (enemyElementType == ElementType.Fire)
                { // Weakness
                    TakeDamage(playerScript.damage * 1.3);
                }
                else if (enemyElementType == ElementType.Ice)
                { // Resistance
                    TakeDamage(playerScript.damage * 0.8);
                }
                else
                {
                    TakeDamage(playerScript.damage);
                }
                break;
            case "Poison Bullet":
                if (enemyElementType == ElementType.Ice)
                { // Weakness
                    TakeDamage(playerScript.damage * 1.3);
                }
                else if (enemyElementType == ElementType.Poison)
                { // Resistance
                    TakeDamage(playerScript.damage * 0.8);
                }
                else
                {
                    TakeDamage(playerScript.damage);
                }
                break;
        }
    }

    private void TakeDamage(double amount)
    {
        health -= (int)amount;

        if (health <= 0)
        {
            Die();
        }
    }
}
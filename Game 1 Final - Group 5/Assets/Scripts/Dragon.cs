using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Solo
{
    private AudioSource audioSource;
    public AudioClip a_ambient;
    public AudioClip a_attack;

    public Transform fireAttackPosition;
    public Transform iceAttackPosition;
    public Transform toxinAttackPosition;

    public GameObject fireBallPrefab;
    public GameObject iceProjectilePrefab;
    public GameObject toxinPrefab;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

        if (!playedAmbient)
        {
            timeBetweenAmbient = Random.Range(10, 15);
            audioSource.PlayOneShot(a_ambient);
            playedAmbient = true;
            Invoke(nameof(AmbientPlayed), timeBetweenAmbient);
        }

        // Win Condition
        if (health <= 0)
        {
            Win();
            Destroy(gameObject);
        }
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        Vector3 lookAtVar = new Vector3(player.transform.position.x, gameObject.transform.position.y, player.transform.position.z);

        transform.LookAt(lookAtVar);

        if (!alreadyAttacked)
        {
            // Attack code

            //
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void breathFire()
    {
        // Audio
        audioSource.PlayOneShot(a_attack);

    }

    public void shootIce()
    {
        // Audio
        audioSource.PlayOneShot(a_attack);

    }

    public void breathToxin()
    {
        // Audio
        audioSource.PlayOneShot(a_attack);

    }

    public void Win()
    {
        // Win Screen

    }
}

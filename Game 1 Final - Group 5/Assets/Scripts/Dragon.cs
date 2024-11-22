using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Solo
{
    private AudioSource audioSource;
    public AudioClip a_ambient1;
    public AudioClip a_ambient2;
    public AudioClip a_BreathFire;
    public AudioClip a_ShootIce;
    public AudioClip a_BreathToxin;

    public Transform fireAttackPosition;
    public Transform iceAttackPosition;
    public Transform toxinAttackPosition;

    public GameObject fireBallPrefab;
    public GameObject iceProjectilePrefab;
    public GameObject toxinPrefab;

    // Update is called once per frame
    void Update()
    {
        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
        if (health <= 0)
        {
            Win();
        }
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player.transform);

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
        audioSource.PlayOneShot(a_BreathFire);

    }

    public void shootIce()
    {
        // Audio
        audioSource.PlayOneShot(a_ShootIce);

    }

    public void breathToxin()
    {
        // Audio
        audioSource.PlayOneShot(a_BreathToxin);

    }

    public void Win()
    {
        // Win Screen

    }
}

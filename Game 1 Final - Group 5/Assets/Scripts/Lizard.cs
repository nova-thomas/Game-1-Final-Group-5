using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lizard : Pack
{
    private AudioSource audioSource;
    public AudioClip a_ambient1;
    public AudioClip a_ambient2;
    public AudioClip a_FireBallAttack;

    public Transform firePosition;
    public GameObject fireBallPrefab;
    public int fireBallSpeed;

    // Update is called once per frame
    void Update()
    {
        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player.transform);

        if (!alreadyAttacked)
        {
            FireBallAttack();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void FireBallAttack()
    {
        // Audio
        audioSource.PlayOneShot(a_FireBallAttack);

        if (fireBallPrefab != null)
        {
            GameObject fireBall = Instantiate(fireBallPrefab, firePosition.position, firePosition.rotation);
            Rigidbody fireBallRB = fireBall.GetComponent<Rigidbody>();

            if (fireBallRB != null)
            {
                fireBallRB.velocity = firePosition.forward * fireBallSpeed;
            }
        }
    }
}

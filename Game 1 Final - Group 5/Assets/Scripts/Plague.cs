using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plague : Pack
{
    private AudioSource audioSource;
    public AudioClip a_ambient1;
    public AudioClip a_ambient2;
    public AudioClip a_EmitToxin;

    public Transform toxinPosition;

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
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player.transform);

        if (!alreadyAttacked)
        {
            EmitToxin();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void EmitToxin()
    {
        // Audio
        audioSource.PlayOneShot(a_EmitToxin);

    }
}

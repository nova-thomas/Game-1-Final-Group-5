using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Solo
{
    private AudioSource audioSource;
    public AudioClip a_ambient;
    public AudioClip a_SwingAttack;
    public AudioClip a_SlamAttack;

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

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patrolling();
            myAnimator.SetInteger("DIR", 0);
        }

        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
            myAnimator.SetInteger("DIR", 1);
        }
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

        if (!playedAmbient)
        {
            timeBetweenAmbient = Random.Range(5, 10);
            audioSource.PlayOneShot(a_ambient);
            playedAmbient = true;
            Invoke(nameof(AmbientPlayed), timeBetweenAmbient);
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

    public void SwingAttack()
    {
        // Audio
        audioSource.PlayOneShot(a_SwingAttack);

    }

    public void SlamAttack()
    {
        // Audio
        audioSource.PlayOneShot(a_SlamAttack);

    }
}

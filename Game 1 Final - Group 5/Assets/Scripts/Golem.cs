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
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            myAnimator.SetInteger("DIR", 0);
            Patrolling();
        }

        if (playerInSightRange && !playerInAttackRange)
        {
            if (myAnimator.GetInteger("DIR") == 0)
            {
                myAnimator.CrossFade("Walk", .2f);
            }
            myAnimator.SetInteger("DIR", 1);
            ChasePlayer();
        }
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

        if (!playedAmbient)
        {
            timeBetweenAmbient = Random.Range(10, 15);
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
            myAnimator.SetBool("SWING", false);
            myAnimator.SetBool("SLAM", false);
        }
    }

    public void SwingAttack()
    {
        // Audio
        audioSource.PlayOneShot(a_SwingAttack);
        myAnimator.SetBool("SWING", true);
    }

    public void SlamAttack()
    {
        // Audio
        audioSource.PlayOneShot(a_SlamAttack);
        myAnimator.SetBool("SLAM", true);
    }
}

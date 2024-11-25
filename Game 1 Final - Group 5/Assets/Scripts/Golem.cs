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
        if (agent.velocity.x > .1 || agent.velocity.z > .1) 
        {
            if (myAnimator.GetInteger("DIR") == 0)
            {
                myAnimator.CrossFade("Walk", .2f);
            }
            myAnimator.SetInteger("DIR", 1);
        }
        else
        {
            myAnimator.SetInteger("DIR", 0);
        }

        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patrolling();
        }

        if (playerInSightRange && !playerInAttackRange)
        {
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

        Vector3 lookAtVar = new Vector3(player.transform.position.x, gameObject.transform.position.y, player.transform.position.z);

        transform.LookAt(lookAtVar);

        if (!alreadyAttacked)
        {
            // Attack code

            //
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
            myAnimator.SetBool("SWIPE", false);
            myAnimator.SetBool("SLAM", false);
        }
    }

    public void SwingAttack()
    {
        // Audio
        audioSource.PlayOneShot(a_SwingAttack);
        myAnimator.SetBool("SWIPE", true);
    }

    public void SlamAttack()
    {
        // Audio
        audioSource.PlayOneShot(a_SlamAttack);
        myAnimator.SetBool("SLAM", true);
    }
}

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
            timeBetweenAmbient = Random.Range(15, 50);
            AudioClip choice;
            int val;
            val = Random.Range(0, 2);
            if (val == 0)
            {
                choice = a_ambient1;
            } else
            {
                choice = a_ambient2;
            }
            audioSource.PlayOneShot(choice);
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
                fireBallRB.velocity = -firePosition.right * fireBallSpeed;
            }

            // Set the damage value on the FireProjectile component
            FireProjectile fireProjectile = fireBall.GetComponent<FireProjectile>();
            if (fireProjectile != null)
            {
                fireProjectile.damage = damage; 
            }
        }
    }
}

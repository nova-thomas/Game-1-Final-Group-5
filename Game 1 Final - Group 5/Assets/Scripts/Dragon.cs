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

    public int fireBallSpeed;
    public int iceSpikeSpeed;
    public int toxicCloudSpeed;

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

        fireAttackPosition.LookAt(player.transform);
        iceAttackPosition.LookAt(player.transform);
        toxinAttackPosition.LookAt(player.transform);

        // Rotate the attack positions
        fireAttackPosition.rotation *= Quaternion.Euler(0, 90, 0);
        iceAttackPosition.rotation *= Quaternion.Euler(0, 90, 0);
        toxinAttackPosition.rotation *= Quaternion.Euler(0, 90, 0);


        // Draw debug rays for attack positions
        Debug.DrawRay(fireAttackPosition.position, fireAttackPosition.right * 10, Color.red);
        Debug.DrawRay(iceAttackPosition.position, iceAttackPosition.right * 10, Color.cyan);
        Debug.DrawRay(toxinAttackPosition.position, toxinAttackPosition.right * 10, Color.green);

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
            int randomAttack = Random.Range(0, 3); // Generates 0, 1, or 2

            switch (randomAttack)
            {
                case 0:
                    breathFire();
                    break;
                case 1:
                    shootIce();
                    break;
                case 2:
                    breathToxin();
                    break;
            }
            //
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void breathFire()
    {
        // Audio
        audioSource.PlayOneShot(a_attack);

        if (fireBallPrefab != null)
        {
            GameObject fireBall = Instantiate(fireBallPrefab, fireAttackPosition.position, fireAttackPosition.rotation);
            Rigidbody fireBallRB = fireBall.GetComponent<Rigidbody>();

            if (fireBallRB != null)
            {
                fireBallRB.velocity = -fireAttackPosition.right * fireBallSpeed;
            }

            // Set the damage value on the FireProjectile component
            FireProjectile fireProjectile = fireBall.GetComponent<FireProjectile>();
            if (fireProjectile != null)
            {
                fireProjectile.damage = damage;
            }
        }
    }

    public void shootIce()
    {
        // Audio
        audioSource.PlayOneShot(a_attack);

        if (iceProjectilePrefab != null)
        {
            GameObject iceSpike = Instantiate(iceProjectilePrefab, iceAttackPosition.position, iceAttackPosition.rotation);
            Rigidbody iceSpikeRB = iceSpike.GetComponent<Rigidbody>();

            if (iceSpikeRB != null)
            {
                iceSpikeRB.velocity = -iceAttackPosition.right * iceSpikeSpeed;
            }

            // Set the damage value on the FireProjectile component
            IceProjectile iceProjectile = iceSpike.GetComponent<IceProjectile>();
            if (iceProjectile != null)
            {
                iceProjectile.damage = damage;
            }
        }
    }

    public void breathToxin()
    {
        // Audio
        audioSource.PlayOneShot(a_attack);

        if (toxinPrefab != null)
        {
            GameObject toxicCloud = Instantiate(toxinPrefab, toxinAttackPosition.position, toxinAttackPosition.rotation);
            Rigidbody toxicCloudRB = toxicCloud.GetComponent<Rigidbody>();

            if (toxicCloudRB != null)
            {
                toxicCloudRB.velocity = -toxinAttackPosition.right * toxicCloudSpeed;
            }

            // Set the damage value on the FireProjectile component
            ToxicProjectile toxicProjectile = toxicCloud.GetComponent<ToxicProjectile>();
            if (toxicCloud != null)
            {
                toxicProjectile.damage = damage;
            }
        }
    }

    public void Win()
    {
        // Win Screen

    }
}

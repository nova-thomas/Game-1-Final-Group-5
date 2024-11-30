using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plague : Pack
{
    private AudioSource audioSource;
    public AudioClip a_ambient1;
    public AudioClip a_ambient2;
    public AudioClip a_EmitToxin;

    public GameObject toxinPrefab;
    public Transform toxinPosition;
    public int toxicCloudSpeed;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timeBetweenAmbient = Random.Range(5, 20);
        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

        if (!playedAmbient)
        {
            AudioClip choice;
            int val;
            val = Random.Range(0, 2);
            if (val == 0)
            {
                choice = a_ambient1;
            }
            else
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
            EmitToxin();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void EmitToxin()
    {
        // Audio
        audioSource.PlayOneShot(a_EmitToxin);

        if (toxinPrefab != null)
        {
            GameObject toxicCloud = Instantiate(toxinPrefab, toxinPosition.position, toxinPosition.rotation);
            Rigidbody toxicCloudRB = toxicCloud.GetComponent<Rigidbody>();


            if (toxicCloudRB != null)
            {
                toxicCloudRB.velocity = -toxinPosition.right * toxicCloudSpeed;
            }

            // Set the damage value on the FireProjectile component
            ToxicProjectile toxicCloudProjectile = toxicCloud.GetComponent<ToxicProjectile>();
            if (toxicCloudProjectile != null)
            {
                toxicCloudProjectile.damage = damage;
            }
        }
    }
}

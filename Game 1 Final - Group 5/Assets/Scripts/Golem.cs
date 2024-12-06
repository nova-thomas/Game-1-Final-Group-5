using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Solo
{
    private AudioSource audioSource;
    public AudioClip a_ambient;
    public AudioClip a_SwingAttack;
    public AudioClip a_SlamAttack;

    public GameObject hitboxPrefab;
    public Transform hitboxTransform;
    public float attackSpeed;

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

        if (!myAnimator.GetBool("SLAM") && !myAnimator.GetBool("SWIPE"))
        {
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
                timeBetweenAmbient = Random.Range(5, 20);
                audioSource.PlayOneShot(a_ambient);
                playedAmbient = true;
                Invoke(nameof(AmbientPlayed), timeBetweenAmbient);
            }
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
            int attack = Random.Range(0, 2);

            switch (attack)
            {
                case 0:
                    SwingAttack(); 
                    break;
                case 1:
                    SlamAttack();
                    break;
                default:
                    break;
            }

            //
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private IEnumerator AttackTime(float attackSpeed)
    {
        yield return new WaitForSeconds(attackSpeed);
        GameObject hitbox = Instantiate(hitboxPrefab, hitboxTransform);
        Destroy(hitbox, 1);
        myAnimator.SetBool("SWIPE", false);
        myAnimator.SetBool("SLAM", false);
    }

    public void SwingAttack()
    {
        // Audio
        audioSource.PlayOneShot(a_SwingAttack);
        myAnimator.CrossFade("Swipe", .2f);
        myAnimator.SetBool("SWIPE", true);

        attackSpeed = 1;
        StartCoroutine(AttackTime(attackSpeed));
    }

    public void SlamAttack()
    {
        // Audio
        audioSource.PlayOneShot(a_SlamAttack);
        myAnimator.CrossFade("Slam", .2f);
        myAnimator.SetBool("SLAM", true);

        attackSpeed = 1.5f;
        StartCoroutine(AttackTime(attackSpeed));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dragon : Solo
{
    private AudioSource audioSource;
    public AudioClip a_ambient;
    public AudioClip a_attack;

    public Transform fireAttackPosition;
    public Transform iceAttackPosition;
    public Transform toxinAttackPosition;

    public GameObject dragonFBX;
    public GameObject fireBallPrefab;
    public GameObject iceProjectilePrefab;
    public GameObject toxinPrefab;

    public int fireBallSpeed;
    public int iceSpikeSpeed;
    public int toxicCloudSpeed;

    public new int coins { get; set; } = 0;
    public new int powerup { get; set; } = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        myAnimator = dragonFBX.GetComponent<Animator>();
        coins = 0; 
        powerup = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsGrounded())
            return; // Skip AI behavior until grounded
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
            timeBetweenAmbient = Random.Range(5, 20);
            audioSource.PlayOneShot(a_ambient);
            playedAmbient = true;
            Invoke(nameof(AmbientPlayed), timeBetweenAmbient);
        }

        // Win Condition
        if (health <= 0)
        {
            Win();
           
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
                    StartCoroutine(shootIce());
                    break;
                case 2:
                    StartCoroutine(breathToxin());
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
        myAnimator.CrossFade("FireAttack",.2f);

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

    public IEnumerator shootIce()
    {
        // Audio
        audioSource.PlayOneShot(a_attack);
        myAnimator.CrossFade("IceAttack", .2f);

        yield return new WaitForSeconds(.5f);

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

    public IEnumerator breathToxin()
    {
        // Audio
        audioSource.PlayOneShot(a_attack);
        myAnimator.CrossFade("AcidAttack", .2f);

        yield return new WaitForSeconds(.8f);

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
        Time.timeScale = 0f;
        SceneManager.LoadScene("CutsceneEnd");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
}

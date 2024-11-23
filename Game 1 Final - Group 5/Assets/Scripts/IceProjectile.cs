using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectile : MonoBehaviour
{
    public float alive = 5f;
    public double damage;

    void Start()
    {
        //destroy after x amount of seconds
        Destroy(gameObject, alive);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    //collision with objects
    private void OnCollisionEnter(Collision collision)
    {
        //kill kill kill 
        Destroy(gameObject);
    }
}

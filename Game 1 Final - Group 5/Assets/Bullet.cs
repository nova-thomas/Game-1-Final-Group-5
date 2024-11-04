using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float alive = 10f;

    void Start()
    {
        //destroy after x amount of seconds
        Destroy(gameObject, alive);
    }

    //collision with objects
    private void OnCollisionEnter(Collision collision)
    {
        //kill kill kill 
        Destroy(gameObject);
    }
}

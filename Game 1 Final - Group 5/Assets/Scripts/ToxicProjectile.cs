using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicProjectile : MonoBehaviour
{
    public float alive = 3f;
    public double damage;

    void Start()
    {
        //destroy after x amount of seconds
        Destroy(gameObject, alive);
    }
}

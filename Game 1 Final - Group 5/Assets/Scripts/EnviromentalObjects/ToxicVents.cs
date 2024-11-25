using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ToxicVents: EnviromentalObjects
{
    public int damage;
    public GameObject[] Toxin;
    public BoxCollider BoxCollider;

    // Start is called before the first frame update
    void Start()
    {
        elementType = 3;
        active = true;
        BoxCollider = GetComponent<BoxCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Damage
        }
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.tag == "Ice Bullet")
        {
            //Shut off
            for (int i = 0; i < Toxin.Length; i++)
            {
                Destroy(Toxin[i]);
            }
            active = false;
            BoxCollider.center = new Vector3(0, -.5f, 0);
            BoxCollider.size = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

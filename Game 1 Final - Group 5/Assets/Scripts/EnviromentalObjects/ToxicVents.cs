using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ToxicVents: EnviromentalObjects
{
    public int damage;
    public GameObject[] Toxin;

    // Start is called before the first frame update
    void Start()
    {
        elementType = 2;
        active = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ice Bullet")
        {
            //Shut off
            for (int i = 0; Toxin[i] != null; i++)
            {
                Destroy(Toxin[i]);
            }
        }
        else if (collision.gameObject.tag == "Player")
        {
            //Damage
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

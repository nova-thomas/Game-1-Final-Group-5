using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class FireJets: EnviromentalObjects
{
    public int damage;
    public GameObject[] flame;

    // Start is called before the first frame update
    void Start()
    {
        elementType = 1;
        active = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Poison Bullet")
        {
            //Shut off
            for (int i = 0; flame[i] != null; i++)
            {
                Destroy(flame[i]);
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

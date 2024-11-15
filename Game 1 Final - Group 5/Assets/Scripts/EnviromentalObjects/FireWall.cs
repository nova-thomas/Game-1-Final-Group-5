using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWall : EnviromentalObjects
{
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        elementType = 1;
        active = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Fire Bullet")
        {
            //Shut off
            active = false;
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

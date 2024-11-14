using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWall : EnviromentalObjects
{
    // Start is called before the first frame update
    void Start()
    {
        elementType = 0;
        active = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Fire Bullet")
        {
            //melt
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

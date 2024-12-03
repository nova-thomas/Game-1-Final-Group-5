using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IceWall : EnviromentalObjects
{
    // Start is called before the first frame update
    void Start()
    {
        elementType = 2;
        active = true;
    }

    private void OnTriggerEnter(Collider Trigger)
    {
        if (Trigger.gameObject.tag == "Fire Bullet")
        {
            //Melt
            active = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            this.transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime);

        }
    }
}

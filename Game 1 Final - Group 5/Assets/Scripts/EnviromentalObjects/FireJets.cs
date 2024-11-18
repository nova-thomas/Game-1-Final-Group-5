using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class FireJets: EnviromentalObjects
{
    public int damage;
    public GameObject[] flame;
    public BoxCollider BoxCollider;

    // Start is called before the first frame update
    void Start()
    {
        elementType = 1;
        active = true;
        BoxCollider = GetComponent<BoxCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Poison Bullet")
        {
            //Shut off
            for (int i = 0; i < flame.Length; i++)
            {
                Destroy(flame[i]);
            }
            active = false;
            BoxCollider.center = new Vector3(0, -.5f, 0);
            BoxCollider.size = Vector3.zero;
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    public GameObject field;
    private bool valid;
    private GameObject hero;
    private void Start()
    {
        valid = true;
        hero = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        bool isIn = field.GetComponent<PlantformSensor>().isIn;
        
        /*if(hero.GetComponent<PlayerCtrl>().status == PlayerCtrl.Status.Climb)
        {
            valid = false;
        }
        else
        {
            valid = true;
        }*/

        if(valid && isIn)
        {
            gameObject.layer = LayerMask.NameToLayer("GroundLayer");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Platform");
        }
    }

    void resetValid()
    {
        valid = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderCtrl : MonoBehaviour
{
    private GameObject hero;
    private bool isIn;
    // Start is called before the first frame update
    void Start()
    {
        hero = GameObject.FindGameObjectWithTag("Player");
        isIn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isIn) {
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("footSensor"))
        {
            isIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("footSensor"))
        {
            isIn = false;
        }
    }
}

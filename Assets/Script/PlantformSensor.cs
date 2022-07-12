using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantformSensor : MonoBehaviour
{
    public bool isIn;
    // Start is called before the first frame update
    void Start()
    {
        isIn = false;
    }

    // Update is called once per frame
    void Update()
    {
        
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

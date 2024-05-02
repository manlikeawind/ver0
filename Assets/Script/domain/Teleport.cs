using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class Teleport : BaseObj
{
    public float posX;
    public float posY;
    public string to;

    public override void construct(string info)
    {
        JObject o = JObject.Parse(info);
        to = (string)o["to"];
        posX = (float)o["posx"];
        posY = (float)o["posy"];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject root = GameObject.FindGameObjectWithTag("Root");
            if (root != null)
            {
                root.GetComponent<AreaManager>().transformToNextScene(to, posX, posY);
            }
        }
    }
}

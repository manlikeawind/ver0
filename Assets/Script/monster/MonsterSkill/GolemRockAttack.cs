using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemRockAttack : MonoBehaviour
{
    public float dir = 1f;
    public float x = 0f;
    public float y = 0f;
    public SpriteRenderer sprd;
    public float time = -1f;
    public void initStatus(float px, float py, float pdir)
    {
        x = px;
        y = py;
        dir = pdir;
        if (dir < 0)
        {
            transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        transform.position = new Vector3(px,py, 0);
        time = 0.0f;
    }

    private void Start()
    {
        sprd = GetComponent<SpriteRenderer>();
        sprd.material.SetFloat("_Show", 7f);
    }

    private void FixedUpdate()
    {
        if(time >= 0f)
        {
            time += Time.fixedDeltaTime;
            double count = Math.Floor(time / 0.03);
            if (count < 1) { }
            else if (count < 5)
            {
                sprd.material.SetFloat("_Show", (float)count);
            }
            else if (count < 15) { }
            else if (count < 17)
            {
                sprd.material.SetFloat("_Show", (float)count - 10);
            }
            else if (count > 19)
            {
                transform.Translate(new Vector3(dir * 3f, 0, 0),Space.World);
                time = 0f;
            }
        }
    }
}

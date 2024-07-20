using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class Slime : BaseObj
{
    public float hp;
    //position
    public float x;
    public float y;
    public float centerX;
    //max range
    public float lx;
    public float rx;
    public string chest;
    public GroupCtrl groupCtrl;

    public float randomValue;
    public float faceTo;
    public float speed;
    public float walkSpeed;
    public float rageValue;

    public enum Status
    {
        None = 0,
        Idle, Rest, Move, Attack1, Attack2, Attack3, Deffence, Hitted, Drown, Sleep, Throw, Alert,
    }

    public GameObject player;
    public Rigidbody2D rb2d;
    public Animator anim;

    public Status status;
    public Status nextStatus;
    public float statusTime;
    public float statusParam;
    //0:nomal 1:battle
    public int pattern;

    public void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        status = Status.Rest;
        nextStatus = Status.None;
        statusTime = 0f;
        statusParam = 0f;
        speed = 5f;
        walkSpeed = 1.5f;
        pattern = 0;
        rageValue = 0f;
        randomValue = 0f;
    }

    public override void construct(string info)
    {
        JObject o = JObject.Parse(info);

        hp = (float)o["hp"];
        x = (float)o["x"];
        y = (float)o["y"];
        centerX = (float)o["centerX"];
        lx = (float)o["lx"];
        rx = (float)o["rx"];
        chest = (string)o["chest"];
        groupCtrl = GameObject.FindGameObjectWithTag("Root").transform.Find(chest).GetComponent<GroupCtrl>();
        if (centerX > x)
        {
            faceTo = 1f;
        }
        else
        {
            faceTo = -1f;
        }
        if (hp > 0)
        {
            transform.position = new Vector2(x, y);
            status = Status.Rest;
        }
        else
        {
            //transform.gameObject.SetActive(false);
        }
    }

    //judge if hero is in monster's view, width/height is half,for example,if the width of sight is 8f, the incoming param is 4f.
    public bool isHeroInSight()
    {
        float width = 12f;
        float height = 4f;
        float px = player.transform.position.x;
        float py = player.transform.position.y;
        float centerX = transform.position.x + faceTo * width;
        float centerY = transform.position.y;
        if (px < (centerX + width) && px > (centerX - width) && py < (centerY + height) && py > (centerY - height) && px > lx && px < rx)
        {
            LayerMask Ground = 1 << LayerMask.NameToLayer("GroundLayer");
            //check if player is cover by ground, because the position is at foot, so y plus 0.5f;
            RaycastHit2D[] list1 = Physics2D.LinecastAll(new Vector2(transform.position.x, transform.position.y + 1f), new Vector2(px, py + 1f), Ground);
            if (list1.Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void alarm()
    {
        LayerMask monsterLayer = 1 << LayerMask.NameToLayer("Monster");
        Collider2D[] list1 = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 8f, monsterLayer);
        foreach (Collider2D collider in list1)
        {
            if (collider.gameObject.tag.Equals("Slime"))
            {
                collider.gameObject.GetComponent<Slime>().goBattle();
            }
        }
    }

    public void goBattle()
    {
        pattern = 1;
    }
}

using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class SlimeSwordN : BaseObj
{
    public enum Status
    {
        None = 0,
        Idle, Rest, Move, Attack1, Attack2, Hitted, Drown, Sleep, Throw, Alert,
    }

    private float hp;
    //position
    private float x;
    private float y;
    private float centerX;
    //max range
    private float lx;
    private float rx;

    //0:nomal 1:patrol
    private int role;

    private float randomValue;
    private float faceTo;
    private float speed;
    private float walkSpeed;
    private float patrolRadius;
    private float rageValue;

    private GameObject player;
    private Rigidbody2D rb2d;
    private Animator anim;

    private Status status;
    private Status nextStatus;
    private float statusTime;
    private float statusParam;
    //0:nomal 1:battle
    private int pattern;

    public void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        status = Status.Rest;
        nextStatus = Status.None;
        statusTime = 0f;
        statusParam = 0f;
        speed = 6f;
        walkSpeed = 1f;
        pattern = 0;
        patrolRadius = 8f;
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
        role = (int)o["role"];
        if (centerX > x) {
            faceTo = 1f;
        }
        else
        {
            faceTo = -1f;
        }
        if (hp > 0)
        {
            transform.position = new Vector2(x, y);
            if (role == 1)
            {
                status = Status.Move;
            }
            else
            {
                status = Status.Rest;
            }
        }
        else
        {
            //transform.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (pattern == 0)
        {
            if (isHeroInSight())
            {
                rageValue += Time.fixedDeltaTime * 10;
            }
            else
            {
                rageValue = 0f;
            }
            if(rageValue > 10f)
            {
                pattern = 1;
                alarm();
            }
        }
        else if (pattern == 1)
        {
            if(player.transform.position.x < transform.position.x)
            {
                faceTo = -1;
            }
            else
            {
                faceTo = 1;
            }
            if (player.transform.position.x < lx || player.transform.position.x > rx) {
                rageValue = 0f;
                pattern = 0;
            }
        }
        statusTime += Time.fixedDeltaTime;
        if (nextStatus != Status.None)
        {
            initStatus();
        }

        AI();

        if (faceTo > 0f)
        {
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        else if (faceTo < 0f)
        {
            transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
    }

    private void initStatus()
    {
        switch (nextStatus)
        {
            case Status.Idle:
                rb2d.velocity = new Vector2(0f, 0f);
                anim.Play("monsterSwordN_idle");
                break;
            case Status.Rest:
                if (centerX > x)
                {
                    faceTo = 1f;
                }
                else
                {
                    faceTo = -1f;
                }
                rb2d.velocity = new Vector2(0f, 0f);
                anim.Play("monsterSwordN_rest");
                break;
            case Status.Move:
                anim.Play("monsterSwordN_move");
                break;
            case Status.Attack1:
                rb2d.velocity = new Vector2(0f, 0f);
                anim.Play("monsterSwordN_attack");
                break;
            case Status.Attack2:
                rb2d.velocity = new Vector2(0f, 0f);
                anim.Play("monsterSwordN_attack");
                break;
            default:
                break;
        }
        status = nextStatus;
        nextStatus = Status.None;
        statusTime = 0.0f;
        randomValue = Random.value;
    }

    public void AI()
    {
        switch (status)
        {
            case Status.Rest:
                if (role == 1) {
                    nextStatus = Status.Move;
                }
                break;
            case Status.Move:
                if (pattern == 0)
                {
                    if (role == 0)
                    {
                        if (transform.position.x > x - 0.1f && transform.position.x < x + 0.1f)
                        {
                            nextStatus = Status.Rest;
                        }
                        else
                        {
                            if (transform.position.x < x)
                            {
                                faceTo = 1f;
                            }
                            else
                            {
                                faceTo = -1f;
                            }
                            rb2d.velocity = new Vector2(walkSpeed * faceTo, 0.0f);
                        }
                    }
                    else if (role == 1) {
                        if (transform.position.x < x - patrolRadius || transform.position.x < lx) { 
                            faceTo = 1f;
                        }else if (transform.position.x > x + patrolRadius || transform.position.x > rx)
                        {
                            faceTo = -1f;
                        }
                        rb2d.velocity = new Vector2(walkSpeed * faceTo, 0.0f);
                    }
                    if (statusTime > (randomValue * 8 + 10f)) { 
                        nextStatus = Status.Idle;
                    }
                }
                else if (pattern == 1)
                {
                    if (player.transform.position.x > transform.position.x) { 
                        faceTo = 1f;
                    }
                    else
                    {
                        faceTo = -1f;
                    }
                    rb2d.velocity = new Vector2(speed * faceTo, 0.0f);
                    if (faceTo* player.transform.position.x > faceTo * transform.position.x 
                        && faceTo * player.transform.position.x < faceTo * (transform.position.x + faceTo * 1.5f)) 
                    {
                        nextStatus = Status.Attack1;
                    }
                }
                break;
            case Status.Idle:
                if (pattern == 0)
                {
                    if (statusTime > (randomValue * 2f + 2f))
                    {
                        nextStatus = Status.Move;
                    }
                }
                else if (pattern == 1)
                {
                    if (statusTime > randomValue + 1.0f) {
                        if (player.transform.position.x > transform.position.x)
                        {
                            faceTo = 1f;
                        }
                        else
                        {
                            faceTo = -1f;
                        }
                        if (faceTo * player.transform.position.x > faceTo * transform.position.x
                            && faceTo * player.transform.position.x < faceTo * (transform.position.x + faceTo * (0.5f + randomValue / 2)))
                        {
                            nextStatus = Status.Attack1;
                        }
                        else {
                            nextStatus = Status.Move;
                        }
                    }
                }
                break;
            case Status.Attack1:
                if (statusTime > 1f)
                {
                    nextStatus = Status.Idle;
                }
                break;
            case Status.Attack2:
                if (statusTime > 1f)
                {
                    nextStatus = Status.Idle;
                }
                break;
            default:
                break;
        }
    }

    //judge if hero is in monster's view, width/height is half,for example,if the width of sight is 8f, the incoming param is 4f.
    private bool isHeroInSight()
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

    private bool rectOverlap() {
        return true;
    }
    private void alarm()
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SlimeBowN : Slime
{
    GameObject arrowPrefab;

    private void Awake()
    {
        arrowPrefab = (GameObject)Resources.Load("Prefabs/material/arrow");
    }

    private void FixedUpdate() {
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
            if (rageValue > 10f)
            {
                pattern = 1;
                alarm();
            }
        }
        else if (pattern == 1)
        {
            pattern = 2;
        }
        else if (pattern == 2)
        {
            if (player.transform.position.x < transform.position.x)
            {
                faceTo = -1;
            }
            else
            {
                faceTo = 1;
            }
            if (player.transform.position.x < lx || player.transform.position.x > rx)
            {
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
                GameObject arrow = Instantiate(arrowPrefab);
                arrow.transform.position = transform.position + new Vector3(0,0.5f,0);
                Vector2 dir = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
                Vector2 v = dir.normalized * 20f;
                arrow.GetComponent<Arrow>().initStatus(v.x, v.y, true);
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

    private void AI()
    {
        switch (status)
        {
            case Status.Rest:
                nextStatus = Status.Move;
                break;
            case Status.Move:
                if (pattern == 0)
                {
                    if (transform.position.x < x - 0.5f)
                    {
                        /*if(statusTime > 0.5f)
                        {
                            nextStatus = Status.Idle;
                        }*/
                        faceTo = 1f;
                    }
                    else if (transform.position.x > x + 0.5f)
                    {
                        /*if (statusTime > 0.5f)
                        {
                            nextStatus = Status.Idle;
                        }*/
                        faceTo = -1f;
                    }
                    rb2d.velocity = new Vector2(walkSpeed / 2 * faceTo, rb2d.velocity.y);
                }
                else if (pattern == 1)
                {
                }
                else if (pattern == 2)
                {
                    nextStatus = Status.Attack1;
                }
                break;
            case Status.Idle:
                if (pattern == 0)
                {
                    if(statusTime > 3f)
                    {
                        nextStatus = Status.Move;
                    }
                }
                else if (pattern == 1)
                {
                }
                else if (pattern == 2)
                {
                    if (statusTime > 1.0f)
                    {
                        nextStatus = Status.Attack1;
                    }
                }
                break;
            case Status.Attack1:
                if (statusTime > 3f)
                {
                    nextStatus = Status.Attack1;
                }
                break;
            case Status.Attack2:
                //attack2 must behind attack1
                if (statusTime > 1f)
                {
                    nextStatus = Status.Idle;
                }
                break;
            default:
                break;
        }
    }
}

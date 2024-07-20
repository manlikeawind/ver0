using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeShieldN : Slime
{
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
            if (rageValue > 10f)
            {
                pattern = 1;
                alarm();
            }
        }
        else if (pattern == 1)
        {
            if (groupCtrl.getToken())
            {
                pattern = 2;
            }
            if (player.transform.position.x < lx || player.transform.position.x > rx)
            {
                rageValue = 0f;
                pattern = 0;
            }
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
            case Status.Deffence:
                anim.Play("monsterSwordN_alert");
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
                if (pattern > 0)
                {
                    nextStatus = Status.Move;
                }
                break;
            case Status.Move:
                if (pattern == 0)
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
                        rb2d.velocity = new Vector2(speed * faceTo, 0.0f);
                    }
                }
                else if (pattern == 1)
                {
                    if (player.transform.position.x > transform.position.x)
                    {
                        faceTo = 1f;
                    }
                    else
                    {
                        faceTo = -1f;
                    }
                    rb2d.velocity = new Vector2(walkSpeed * faceTo, 0.0f);
                    if (transform.position.x > player.transform.position.x - 4f && transform.position.x < player.transform.position.x + 4f)
                    {
                        nextStatus = Status.Idle;
                    }
                }
                else if (pattern == 2)
                {
                    if (player.transform.position.x > transform.position.x)
                    {
                        faceTo = 1f;
                    }
                    else
                    {
                        faceTo = -1f;
                    }
                    rb2d.velocity = new Vector2(speed * faceTo, 0.0f);
                    if (faceTo * player.transform.position.x > faceTo * transform.position.x
                        && faceTo * player.transform.position.x < faceTo * (transform.position.x + faceTo * (0.5f + 0.5 * randomValue)))
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
                    if (player.transform.position.x > transform.position.x)
                    {
                        faceTo = 1f;
                    }
                    else
                    {
                        faceTo = -1f;
                    }
                    if (!(faceTo * player.transform.position.x > faceTo * transform.position.x
                        && faceTo * player.transform.position.x < faceTo * (transform.position.x + faceTo * (0.5f + randomValue / 2))))
                    {
                        nextStatus = Status.Move;
                    }
                }
                else if (pattern == 2)
                {
                    if (statusTime > 1.0f)
                    {
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
                        else
                        {
                            nextStatus = Status.Move;
                        }
                    }
                }
                break;
            case Status.Attack1:
                if (statusTime > 1f)
                {
                    nextStatus = Status.Idle;
                    groupCtrl.releaseToken();
                    pattern = 1;
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

    private bool rectOverlap()
    {
        return true;
    }
}

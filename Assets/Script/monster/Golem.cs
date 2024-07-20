using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Golem : BaseObj
{
    //UA2A-unActive to active, A2UA-active to unActive
    //Attack1-common attack, left punch, right punch, both punch
    //Attack2-jump attack,
    //Attack3-dach attack,
    //Attack4-special attack,
    //Attack5-throw Attack
    public enum Status
    {
        None = 0,
        Idle, Move, Jump, Attack1, Attack2, Attack3, Attack4, Attack5, Crash, UA2A, A2UA //unActive to active, active to unActive
    }

    public Status status;
    public Status nextStatus;
    public float statusTime; 
    public bool inBattle;
    public float faceTo;
    public float randomValue;
    public int phase;
    public float armor;

    public float x;
    public float rangeR;

    public GameObject player;
    public SpriteRenderer sprd;
    public Rigidbody2D rb2d;

    //GameObject rockBallPrefab;
    GameObject rockAttackPrefab;

    // Start is called before the first frame update
    void Start()
    {
        x = transform.position.x;
        inBattle = false;
        status = Status.None;
        phase = 0;
        armor = 100f;
        player = GameObject.FindGameObjectWithTag("Player");
        //rockBallPrefab = (GameObject)Resources.Load("Prefabs/material/rockBall");
        rockAttackPrefab = (GameObject)Resources.Load("Prefabs/monsterSkill/Golem/golemRockAttack");
        faceTo = 1f;
        sprd = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        sprd.material.SetFloat("_Show", 8f);
    }

    public override void construct(string info)
    {
        JObject o = JObject.Parse(info);
        rangeR = (float)o["r"];
    }

    private void FixedUpdate()
    {
        if(inBattle)
        {
            statusTime += Time.fixedDeltaTime;
            if (nextStatus != Status.None)
            {
                initStatus();
            }

            AI();
        }
        else
        {
            LayerMask playerLayer = 1 << LayerMask.NameToLayer("Player");
            bool playerIn = Physics2D.OverlapCircle((Vector2)transform.position, 5f, playerLayer);
            if(playerIn)
            {
                inBattle = true;
                nextStatus = Status.UA2A;
            }
        }
    }

    public void initStatus()
    {
        switch (nextStatus) {
            case Status.UA2A:
                sprd.material.SetFloat("_Show", 8f);
                break;
            case Status.A2UA:
                break;
            case Status.Idle:
                rb2d.velocity = new Vector2(0, 0);
                sprd.material.SetFloat("_Show", 0f);
                break;
            case Status.Move:
                break;
            case Status.Attack1:
                sprd.material.SetFloat("_Show", 3f);
                break;
            case Status.Attack2:
                sprd.material.SetFloat("_Show", 5f);
                float x = (player.transform.position.x - transform.position.x - faceTo * 2f)*2;
                rb2d.velocity = new Vector2(x, 10f);
                break;
            case Status.Attack3:
                rb2d.velocity = new Vector2(faceTo * 10f, 0f);
                break;
            case Status.Attack4:
                sprd.material.SetFloat("_Show", 3f);
                break;
            case Status.Attack5:
                break;
            case Status.Crash:
                sprd.material.SetFloat("_Show", 8f);
                break;
        }
        status = nextStatus;
        nextStatus = Status.None;
        phase = 0;
        statusTime = 0.0f;
        randomValue = Random.value;
    }

    public void nextSkill() {
        if (player.transform.position.x < (x - rangeR) || player.transform.position.x > (x + rangeR))
        {
            nextStatus = Status.A2UA;
            return;
        }
        else if (armor < 0)
        {
            nextStatus = Status.Crash;
        }
        else
        {
            if (player.transform.position.x - transform.position.x > -5f && player.transform.position.x - transform.position.x < 5f)
            {
                if ((transform.position.x + faceTo * 5) > (x + rangeR) || (transform.position.x + faceTo * 5) < (x - rangeR))
                {
                    nextStatus = Status.Jump;
                }
                else
                {
                    if (randomValue < 0.5f)
                    {
                        nextStatus = Status.Attack1;
                    }
                    else if (randomValue < 0.8f)
                    {
                        nextStatus = Status.Attack4;
                    }
                    else
                    {
                        if ((transform.position.x + faceTo * 15) > (x + rangeR) || (transform.position.x + faceTo * 15) < (x - rangeR))
                        {
                            nextStatus = Status.Jump;
                        }
                        else
                        {
                            nextStatus = Status.Attack3;
                        }
                    }
                }
            }
            else if (player.transform.position.x - transform.position.x > -10f && player.transform.position.x - transform.position.x < 10f)
            {
                if ((transform.position.x + faceTo * 5) > (x + rangeR) || (transform.position.x + faceTo * 5) < (x - rangeR))
                {
                    nextStatus = Status.Jump;
                }
                else
                {
                    if ((transform.position.x + faceTo * 15) > (x + rangeR) || (transform.position.x + faceTo * 15) < (x - rangeR))
                    {
                        nextStatus = Status.Attack4;
                    }
                    else
                    {
                        if (randomValue < 0.6f)
                        {
                            nextStatus = Status.Attack3;
                        }
                        else
                        {
                            nextStatus = Status.Attack4;
                        }
                    }
                }
            }
            else if (player.transform.position.x - transform.position.x > -15f && player.transform.position.x - transform.position.x < 15f)
            {
                if (randomValue < 0.5f)
                {
                    nextStatus = Status.Attack2;
                }
                else if (randomValue < 0.8f)
                {
                    nextStatus = Status.Attack3;
                }
                else
                {
                    nextStatus = Status.Attack4;
                }
            }
            else {
                
            }
        }
    }

    public void AI()
    {
        switch(status) { 
            case Status.UA2A:
                if(statusTime > 2f)
                {
                    nextStatus = Status.Idle;
                }
                break;
            case Status.A2UA:
                if(statusTime > 2f)
                {
                    status = Status.None;
                    nextStatus = Status.None;
                    inBattle = false;
                }
                break;
            case Status.Idle:
                if (player.transform.position.x < transform.position.x)
                {
                    faceTo = -1f;
                }
                else
                {
                    faceTo = 1f;
                }
                if (faceTo > 0f)
                {
                    transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                }
                else if (faceTo < 0f)
                {
                    transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                }
                if (statusTime > 1f + randomValue) {
                    nextSkill();
                }
                break;
            case Status.Move:
                if (player.transform.position.x < transform.position.x)
                {
                    faceTo = -1f;
                }
                else
                {
                    faceTo = 1f;
                }
                if (faceTo > 0f)
                {
                    transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                }
                else if (faceTo < 0f)
                {
                    transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                }
                {
                    float frame = Mathf.Floor(statusTime / 0.25f);
                    float t = 2 * (frame / 2 - Mathf.Floor(frame / 2));
                    sprd.material.SetFloat("_Show", t + 1f);
                }
                break;
            case Status.Attack1:
                if (phase == 0)
                {
                    if (statusTime > 0.5f)
                    {
                        phase++;
                        sprd.material.SetFloat("_Show", 4f);
                    }
                }
                else if (phase == 1)
                {
                    if (statusTime > 1f)
                    {
                        phase++;
                        sprd.material.SetFloat("_Show", 3f);
                    }

                }
                else if (phase == 2)
                {
                    if (statusTime > 1.5f)
                    {
                        phase++;
                        sprd.material.SetFloat("_Show", 4f);
                    }
                }
                else if (phase == 3)
                {
                    if (statusTime > 2f)
                    {
                        if (player.transform.position.x - transform.position.x > -5f && player.transform.position.x - transform.position.x < 5f)
                        {
                            phase++;
                            if (player.transform.position.x < transform.position.x)
                            {
                                faceTo = -1f;
                            }
                            else
                            {
                                faceTo = 1f;
                            }
                            if (faceTo > 0f)
                            {
                                transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                            }
                            else if (faceTo < 0f)
                            {
                                transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                            }
                            sprd.material.SetFloat("_Show", 3f);
                        }
                        else
                        {
                            phase = phase + 2;
                        }
                    }
                }
                else if (phase == 4)
                {
                    if (statusTime > 3f)
                    {
                        phase++;
                        sprd.material.SetFloat("_Show", 4f);
                    }
                }
                else if (phase == 5) {
                    if (statusTime > 4f) {
                        phase++;
                    }
                }
                else
                {
                    nextStatus = Status.Idle;
                }
                if (statusTime < 2f) {
                    transform.Translate(new Vector3(2 * faceTo * Time.fixedDeltaTime, 0f, 0f), Space.World);
                }
                break;
            case Status.Attack2:
                if (phase == 0)
                {
                    float delta = 40 * Time.fixedDeltaTime;
                    float vy = rb2d.velocity.y - delta;
                    if (vy < 0)
                    {
                        sprd.material.SetFloat("_Show", 0f);
                    }
                    rb2d.velocity = new Vector2(rb2d.velocity.x, vy);
                    if (statusTime > 0.5f)
                    {
                        LayerMask groundLayer = 1 << LayerMask.NameToLayer("GroundLayer");
                        bool onGround = Physics2D.OverlapCircle((Vector2)transform.position, 0.1f, groundLayer);
                        if (onGround)
                        {
                            //nextStatus = Status.Idle;
                            phase++;
                        }
                    }
                }
                else if (phase == 1)
                {
                    phase++;
                    sprd.material.SetFloat("_Show", 4f);
                }
                else { 
                    if(statusTime > 1.5f)
                    {
                        nextStatus = Status.Idle;
                    }
                }
                break;
            case Status.Attack3:
                {
                    float frame = Mathf.Floor(statusTime / 0.25f);
                    float t = 2 * (frame / 2 - Mathf.Floor(frame / 2));
                    sprd.material.SetFloat("_Show", t + 6f);
                }
                if (statusTime > 2f)
                {
                    nextStatus = Status.Idle;
                }
                break;
            case Status.Attack4:
                if (phase == 0)
                {
                    if (statusTime > 1f)
                    {
                        phase++;
                    }
                }
                else if (phase == 1)
                {
                    phase++;
                    sprd.material.SetFloat("_Show", 4f);
                }
                else if (phase == 2) {
                    if (statusTime > 1.2f) { 
                        phase++;
                        GameObject rockAttack = Instantiate(rockAttackPrefab);
                        rockAttack.GetComponent<GolemRockAttack>().initStatus(transform.position.x+3*faceTo, transform.position.y, faceTo);
                    }
                }
                else if(phase == 3)
                {
                    if (statusTime > 1.5f)
                    {
                        phase++;
                        sprd.material.SetFloat("_Show", 1f);
                    }
                }else
                {
                    if(statusTime > 5f)
                    {
                        nextStatus = Status.Idle;
                    }
                }
                break;
        }
    }
}

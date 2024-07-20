using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class MonsterSwordN : Monster
{
    [System.Serializable]
    public struct Attr
    {

        public int role;
        public float xl;
        public float xr;
        public float yh;
        public float yl;
        public float xm;
        public float ym;
        public int status;
    }

    public float moveSpeed;
    public float runSpeed;
    public int valid;
    public float faceTo;
    public enum Status
    {
        None = 0,
        Idle, Rest, Move, Search, Attack1, Attack2, Hitted, Alert, patrol, Drown, Sleep, Throw,
    }
    public struct StatusParam {
        public int intParam1;
        public float floatParam1;
        public float floatParam2;
        public bool boolParam1;
        public void clear() {
            intParam1 = 0;
            floatParam1 = 0;
            floatParam2 = 0;
            boolParam1 = false;
        }
    }
    public enum Pattern
    {
        None = 0,
        Normal, Confront, Battle,
    }

    public Attr attr;
    public Pattern pattern;
    private Pattern nextPattern;
    private float patternTime;
    private float aiTime;
    public Status status;
    private Status nextStatus;
    public float statusTime;
    private StatusParam statusParam;
    private Rigidbody2D rb2d;
    private Animator anim;
    private GameObject player;
    void OnDrawGizmos()
    {
        float width = 8f;
        float height = 8f;
        Vector3 center = new Vector3(transform.position.x+faceTo*width/2, transform.position.y);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, new Vector3(width, height, 0f));
    }

    public override void construct(string info) 
    {
        attr = JsonUtility.FromJson<Attr>(info);
        transform.position = new Vector2(attr.xm, attr.ym);
        if (attr.status == 1)
        {
            validate();
        }
        else
        {
            invalid();
        }
    }

    public void validate() {
        valid = 1;
        gameObject.SetActive(true);
    }

    public void invalid() { 
        valid = 0;
        gameObject.SetActive(false);
    }

    public override void attacked(Weapon attackMode, float damage)
    {
        if(pattern == Pattern.Normal && status != Status.Search)
        {
            nextStatus = Status.Search;
        }
    }

    public void AI()
    {
        float sightWidth = 4f;
        float sightHeight = 4f;
        switch (pattern) {
            case Pattern.Normal:
                switch (status) {
                    case Status.Idle:
                        if(statusTime > 2f)
                        {
                            nextStatus = Status.Move;
                        }
                        if (isHeroInSight(faceTo, sightWidth, sightHeight))
                        {
                            nextPattern = Pattern.Battle;
                        }
                        break;
                    case Status.Move:
                        if (statusTime > 2f)
                        {
                            statusTime = 0f;
                            float r = Random.value;
                            Debug.Log(r);
                            if (r > 0.5f)
                            {
                                nextStatus = Status.Idle;
                            }
                        }
                        if (isHeroInSight(faceTo, sightWidth, sightHeight))
                        {
                            nextPattern = Pattern.Battle;
                            nextStatus = Status.Idle;
                        }
                        break;
                    case Status.Search:
                        if (isHeroInSight(0f, sightWidth * 2, sightHeight))
                        {
                            nextPattern = Pattern.Battle;
                            nextStatus = Status.Idle;
                        }
                        break;
                    default:
                        break;
                }
                break;
            case Pattern.Battle:
                float px = player.transform.position.x;
                float py = player.transform.position.y;
                switch (status) {
                    case Status.Idle:
                        if(statusTime > 2f)
                        {
                            if (py > (transform.position.y + 1.5f))
                            {
                                nextStatus = Status.Throw;
                            }
                            else
                            {
                                if (isHeroInAttackRange())
                                {
                                    nextStatus = Status.Attack1;
                                }
                                else
                                {
                                    nextStatus = Status.Move;
                                }
                            }
                        }
                        if (attr.xl > px || attr.xr < px || attr.yl > py || attr.yh < py)
                        {
                            nextPattern = Pattern.Confront;
                        }
                        break;
                    case Status.Move:
                        if (py > (transform.position.y + 1.5f))
                        {
                            nextStatus = Status.Throw;
                        }
                        else
                        {
                            if (isHeroInAttackRange())
                            {
                                nextStatus = Status.Attack1;
                            }
                        }
                        if (attr.xl > px || attr.xr < px || attr.yl > py || attr.yh < py)
                        {
                            nextPattern = Pattern.Confront;
                            nextStatus = Status.Idle;
                        }
                        break;
                    case Status.Attack1:
                        if(statusTime > 1f)
                        {
                            nextStatus = Status.Idle;
                        }
                        break;
                    case Status.Throw:
                        if (statusTime > 1f)
                        {
                            nextStatus = Status.Idle;
                        }
                        break;
                    default:
                        nextStatus = Status.Idle;
                        break;
                }
                break;
            case Pattern.Confront:
                if (patternTime > 5f)
                {
                    nextPattern = Pattern.Normal;
                    nextStatus = Status.Idle;
                }
                break;
            default:
                nextPattern = Pattern.Normal;
                break;
        }
    }

    private void Start()
    {
        pattern = Pattern.Normal;
        nextPattern = Pattern.None;
        patternTime = 0f;
        status = Status.Idle;
        nextStatus = Status.None;
        statusTime = 0f;
        statusParam.clear();
        valid = 0;
        moveSpeed = 1f;
        runSpeed = 2f;
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        float r = Random.value;
        if (r > 0.5f)
        {
            faceTo = 1f;
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        else
        {
            faceTo = -1f;
            transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
    }

    private void FixedUpdate()
    {
        if (valid == 0) return;
        statusTime += Time.fixedDeltaTime;
        patternTime += Time.fixedDeltaTime;
        aiTime += Time.fixedDeltaTime;
        if(nextPattern != Pattern.None)
        {
            pattern = nextPattern;
            nextPattern = Pattern.None;
            patternTime = 0f;
        }
        if (nextStatus != Status.None)
        {
            initStatus();
        }
        switch (pattern)
        {
            case Pattern.Normal:
                processStatus_Normal();
                break;
            case Pattern.Battle:
                processStatus_Battle();
                break;
            case Pattern.Confront:
                processStatus_Confront();
                break;
            default: 
                break;
        }
        if(aiTime > 0.2f)
        {
            AI();
            aiTime = 0f;
        }
        
    }

    private void initStatus() 
    {
        statusParam.clear();
        switch (nextStatus)
        {
            case Status.Idle:
                rb2d.velocity = new Vector2(0f, 0f);
                anim.Play("monsterSwordN_idle");
                break;
            case Status.Rest:
                rb2d.velocity = new Vector2(0f, 0f);
                anim.Play("monsterSwordN_rest");
                break;
            case Status.Move:
                anim.Play("monsterSwordN_move");
                break;
            case Status.Search:
                rb2d.velocity = new Vector2(0f, 0f);
                anim.Play("monsterSwordN_alert");
                break;
            case Status.Attack1:
                rb2d.velocity = new Vector2(0f, 0f);
                anim.Play("monsterSwordN_attack");
                break;
            case Status.Attack2:
                break;
            case Status.Throw:
                rb2d.velocity = new Vector2(0f, 0f);
                anim.Play("monsterSwordN_throw");
                break;
            default:
                break;
        }
        status = nextStatus;
        nextStatus = Status.None;
        statusTime = 0.0f;
    }

    //judge if hero is in monster's view, width/height is half,for example,if the width of sight is 8f, the incoming param is 4f.
    private bool isHeroInSight(float dir, float width, float height)
    {
        float px = player.transform.position.x;
        float py = player.transform.position.y;
        float centerX = transform.position.x + dir * width;
        float centerY = transform.position.y;
        if(px < (centerX + width) && px > (centerX - width) && py < (centerY + height) && py > (centerY - height))
        {
            LayerMask Ground = 1 << LayerMask.NameToLayer("GroundLayer");
            //check if player is cover by ground, because the position is at foot, so y plus 0.5f;
            RaycastHit2D[] list1 = Physics2D.LinecastAll(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(px, py+0.5f), Ground);
            if(list1.Length == 0)
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

    private bool isHeroInAttackRange() {
        float px = player.transform.position.x;
        //float py = player.transform.position.y;
        float x = transform.position.x;
        //float y = transform.position.y;
        if(px > x - 1f && px < x + 1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void processStatus_Normal() 
    {
        switch (status)
        {
            case Status.Move:
                if (transform.position.x < Mathf.Max(attr.xm - 3f, attr.xl))
                {
                    faceTo = 1f;
                    transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                } else if (transform.position.x > Mathf.Min(attr.xm + 3f, attr.xr)) {
                    faceTo = -1f;
                    transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                }
                rb2d.velocity = new Vector2(moveSpeed * faceTo, 0f);
                break;
            case Status.Search:
                statusParam.floatParam1 = statusParam.floatParam1 + Time.fixedDeltaTime;
                if (statusParam.floatParam1 > 2f)
                {
                    statusParam.floatParam1 = 0f;
                    faceTo = faceTo * -1.0f;
                    if(faceTo > 0f)
                    {
                        transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    }
                    else
                    {
                        transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                    }
                }
                break;
            default:
                break;
        }
    }
    private void processStatus_Battle()
    {
        switch (status)
        {
            case Status.Idle:
                if(player.transform.position.x > transform.position.x)
                {
                    faceTo = 1f;
                    transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

                }
                else if (player.transform.position.x < transform.position.x)
                {
                    faceTo = -1f;
                    transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                }
                break;
            case Status.Move:
                if (player.transform.position.x > transform.position.x + 0.5f)
                {
                    faceTo = 1f;
                    transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

                }
                else if (player.transform.position.x < transform.position.x -0.5f)
                {
                    faceTo = -1f;
                    transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                }
                rb2d.velocity = new Vector2(runSpeed * faceTo, 0f);
                break;
            case Status.Attack1:
                break;
            case Status.Throw:
                break;
            default:
                break;
        }
    }
    private void processStatus_Confront()
    {
    }
}

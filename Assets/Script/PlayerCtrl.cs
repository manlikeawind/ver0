using System;
using System.Collections;
using UnityEngine;
using Common;

public class PlayerCtrl : MonoBehaviour
{
    /*
     * Idle: 0/Idle; 1/climb to Idle; 2/squat;
     * Swim: 0/swim up; 1/swim down;
     */
    public enum Status
    {
        None = 0,
        Idle, Squat, Run, Jump, Fall, Airhike, Fly, Suspend, Swim, SwimAttack, Hang, Climb, ClimbJump, Attack, AttackInteval
    }

    public enum Skill 
    { 
        None = 0,
        katateken_attack1, katateken_attack2, katateken_attack3, katateken_fall_attack, katateken_defence, katateken_defence_attack, 

    }


    #region member variables
    private Weapon equipWeapon;

    //collider parameter
    private float sensorRadius = 0.1f;
    private Vector2 bottomOffset;
    private Vector2 leftOffset;
    private Vector2 rightOffset;
    private Vector2 topOffset;
    private Vector2 midOffset;

    //collider
    private LayerMask groundLayer;
    private LayerMask platformLayer;
    private LayerMask ladderLayer;
    private LayerMask waterLayer;

    //interactor near player
    private Hashtable interactor = new Hashtable();
    private GameObject activePlatform;

    //player status
    public float faceTo = 1.0f;
    public Status status = Status.Idle;
    public Status nextStatus = Status.None;
    public float statusTime = 0.0f;
    private uint statusCode = 0x0000;
    private float[] statusFloatInfo = { 0.0f, 0.0f, 0.0f, 0.0f };
    private Skill skillCode = Skill.None;
    private Skill nextSkillCode = Skill.None;
    private int skillPhase = -1;

    //player parameter
    private float maxSpeed = 5.0f;
    private float jumpSpeed = 15f;
    private float jumpMaxSpeed = 20f;
    private float quickMoveSpeed = 20.0f;
    private float FlySpeed = 12.0f;
    private float flyFallSpeed = 2.0f;
    private float maxFallSpeed = 40.0f;
    private float swimSpeed = 2.0f;
    private float climbSpeed = 1.5f;
    private float climbJumpSpeed = 10f;
    private float accScale = 20.0f;
    private float accInAirScale = 5.0f;
    private float gravity = 40.0f;

    //input
    private InputCtrl playerInput;

    //player gameObject
    private Rigidbody2D rb2d;
    private SpriteRenderer bodySprd;
    private SpriteRenderer weapSprd;
    private SpriteRenderer maskSprd;
    private Animator anim;
    public Texture2D suit;
    #endregion

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerInput = InputCtrl.Instance;
        bodySprd = transform.Find("body").GetComponent<SpriteRenderer>();
        weapSprd = transform.Find("weapon").GetComponent<SpriteRenderer>();
        maskSprd = transform.Find("mask").GetComponent<SpriteRenderer>();
        groundLayer = 1 << LayerMask.NameToLayer("GroundLayer");
        platformLayer = 1 << LayerMask.NameToLayer("Platform");
        ladderLayer = 1 << LayerMask.NameToLayer("Ladder");
        waterLayer = 1 << LayerMask.NameToLayer("WaterLayer");

        bottomOffset = new Vector2(0, 0);
        leftOffset = new Vector2(-0.35f, 0.5f);
        rightOffset = new Vector2(0.35f, 0.5f);
        topOffset = new Vector2(0, 1.0f);
        midOffset = new Vector2(0, 0.5f);

        equipWeapon = Weapon.Katateken;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, sensorRadius);
    }

    public void FixedUpdate()
    {
        //whether move to next status
        if (nextStatus == Status.None)
        {
            checkStatus();
        }

        //initiate next status
        if (nextStatus != Status.None)
        {
            finishStatus();
            initNextStatus();
        }

        processStatus();

        statusTime += Time.fixedDeltaTime;

        if (playerInput.getDirX() > 0.1f)
        {
            faceTo = 1f;
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        else if(playerInput.getDirX() < -0.1f)
        {
            faceTo = -1f;
            transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
    }

    private void finishStatus() {
        switch (status)
        {
            case Status.None:
                break;
            case Status.Idle:
                break;
            case Status.Run:
                break;
            case Status.Jump:
                break;
            case Status.Fall:
                rb2d.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
                break;
            case Status.Airhike:
                break;
            case Status.Fly:
                break;
            case Status.Suspend:
                break;
            case Status.Swim:
                break;
            case Status.SwimAttack:
                break;
            case Status.Hang:
                anim.speed = 1;
                break;
            case Status.Climb:
                break;
            case Status.ClimbJump:
                break;
            case Status.Attack:
                break;
            case Status.AttackInteval:
                break;
            default:
                break;
        }
    }
    private void initNextStatus()
    {
        switch (nextStatus)
        {
            case Status.Idle:
                bodySprd.material.SetFloat("_Show", 0f);
                maskSprd.material.SetFloat("_Show", 0f);
                weapSprd.material.SetFloat("_Show", 0f);
                //anim.Play("player_idle");
                break;
            case Status.Squat:
                bodySprd.material.SetFloat("_Show", 2f);
                maskSprd.material.SetFloat("_Show", 2f);
                weapSprd.material.SetFloat("_Show", 1f);
                //anim.Play("player_squat");
                break;
            case Status.Run:
                //anim.Play("player_run");
                break;
            case Status.Jump:
                if (statusTime > 1f && status == Status.Squat)
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, jumpMaxSpeed);
                }
                else {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
                }
                //anim.Play("player_jump");
                break;
            case Status.Fall:
                rb2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                //anim.Play("player_fall");
                break;
            case Status.Fly:
                //anim.Play("player_fly");
                break;
            case Status.Suspend:
                //rb2d.velocity = new Vector2(0.0f, 0.0f);
                //anim.Play("player_suspend");
                break;
            case Status.Swim:
                //anim.Play("player_swim");
                break;
            case Status.SwimAttack:
                break;
            case Status.Hang:
                rb2d.velocity = new Vector2(0.0f, 0.0f);
                AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
                if(info.IsName("player_climb") || info.IsName("player_climb2")){
                    anim.speed = 0;
                }
                else
                {
                    //anim.Play("player_hang");
                }
                break;
            case Status.ClimbJump:
                if (playerInput.getDirX() > 0.1f || playerInput.getDirX() < -0.1f || playerInput.getDirY() > 0.1f || playerInput.getDirY() < -0.1f)
                {
                    rb2d.velocity = climbJumpSpeed * playerInput.getDirX() * Vector2.right + climbJumpSpeed * playerInput.getDirY() * Vector2.up;
                }
                else
                {
                    rb2d.velocity = climbJumpSpeed * Vector2.up;
                }
                //anim.Play("player_hang");
                break;
            case Status.Climb:
                //anim.Play("player_climb");
                break;
            case Status.Attack:
                switch (nextSkillCode) {
                    case Skill.katateken_attack1:
                        bodySprd.material.SetFloat("_Show", 32f);
                        maskSprd.material.SetFloat("_Show", 32f);
                        weapSprd.material.SetFloat("_Show", 2f);
                        break;
                    case Skill.katateken_attack2:
                        bodySprd.material.SetFloat("_Show", 34f);
                        maskSprd.material.SetFloat("_Show", 34f);
                        weapSprd.material.SetFloat("_Show", 5f);
                        break;
                    case Skill.katateken_attack3:
                        bodySprd.material.SetFloat("_Show", 36f);
                        maskSprd.material.SetFloat("_Show", 36f);
                        weapSprd.material.SetFloat("_Show", 8f);
                        break;
                    case Skill.katateken_fall_attack:
                        bodySprd.material.SetFloat("_Show", 39f);
                        maskSprd.material.SetFloat("_Show", 39f);
                        weapSprd.material.SetFloat("_Show", 11f);
                        break;
                    case Skill.katateken_defence:
                        bodySprd.material.SetFloat("_Show", 40f);
                        maskSprd.material.SetFloat("_Show", 40f);
                        weapSprd.material.SetFloat("_Show", 14f);
                        break;
                    case Skill.katateken_defence_attack:
                        bodySprd.material.SetFloat("_Show", 40f);
                        maskSprd.material.SetFloat("_Show", 40f);
                        weapSprd.material.SetFloat("_Show", 14f);
                        break;
                }
                skillCode = nextSkillCode;
                nextSkillCode = Skill.None;
                skillPhase = 1;
                break;
        }

        status = nextStatus;
        nextStatus = Status.None;
        statusTime = 0.0f;
        /*statusCode = 0x0000;
        for (int i = 0; i < statusFloatInfo.Length; i++)
        {
            statusFloatInfo[i] = 0.0f;
        }*/
    }

    #region check player status
    private void checkStatus()
    {
        switch (status)
        {
            case Status.Idle:
                checkIdle();
                break;
            case Status.Squat:
                checkSquat();
                break;
            case Status.Run:
                checkRun();
                break;
            case Status.Jump:
                checkJump();
                break;
            case Status.Fall:
                checkFall();
                break;
            case Status.Fly:
                checkFly();
                break;
            case Status.Suspend:
                checkSuspend();
                break;
            case Status.Swim:
                checkSwim();
                break;
            case Status.SwimAttack:
                checkSwimAttack();
                break;
            case Status.Hang:
                checkHang();
                break;
            case Status.ClimbJump:
                checkClimbJump();
                break;
            case Status.Climb:
                checkClimb();
                break;
            case Status.Attack:
                checkAttack();
                break;
        }
    }
    private void checkIdle()
    {
        bool onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, groundLayer);
        bool onPlatform = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, platformLayer);
        bool onWater = Physics2D.OverlapCircle((Vector2)transform.position + topOffset, sensorRadius, waterLayer);
        if (!(onGround || onPlatform))
        {
            nextStatus = Status.Fall;
        }
        else if (playerInput.getShdR() > 0.5f)
        {
            nextStatus = Status.Squat;
        }
        else if (playerInput.getBtnA() > 0.5f)
        {
            nextStatus = Status.Jump;
        }
        else if (playerInput.getDirX() > 0.1f)
        {
            nextStatus = Status.Run;
        }
        else if (playerInput.getDirX() < -0.1f)
        {
            nextStatus = Status.Run;
        }
        else if (onWater)
        {
            nextStatus = Status.Suspend;
        }
        else if (playerInput.consumeBtnXDown(0.2f)) {
            nextStatus = Status.Attack;
            switch (equipWeapon) {
                case Weapon.Katateken:
                    nextSkillCode = Skill.katateken_attack1;
                    break;
            }
        }
        
    }

    private void checkSquat() {
        if (playerInput.getShdR() < 0.5f)
        {
            nextStatus = Status.Idle;
        }
        else if (playerInput.getBtnA() > 0.5f) {
            nextStatus = Status.Jump;
        }
    }

    private void checkRun()
    {
        Collider2D onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, groundLayer);
        bool onPlatform = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, platformLayer);
        if (onGround != null && onGround.gameObject.tag.Equals("slope"))
        {
            rb2d.AddForce(new Vector2(0, -100f));
        }
        if (!(onGround || onPlatform))
        {
            nextStatus = Status.Fall;
        }
        else if (playerInput.getBtnA() > 0.5f)
        {
            nextStatus = Status.Jump;
        }
        else if (playerInput.getDirX() > -0.1f && playerInput.getDirX() < 0.1f)
        {
            nextStatus = Status.Idle;
            statusCode = 0x0000;
        }
        else if (playerInput.consumeBtnXDown(0.2f))
        {
            nextStatus = Status.Attack;
            switch (equipWeapon)
            {
                case Weapon.Katateken:
                    nextSkillCode = Skill.katateken_attack1;
                    break;
            }
        }
        else
        {
            bool onWater = Physics2D.OverlapCircle((Vector2)transform.position + topOffset, sensorRadius, waterLayer);
            if (onWater)
            {
                nextStatus = Status.Suspend;
            }
        }
    }

    private void checkJump()
    {
        bool onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, groundLayer);
        bool onPlatform = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, platformLayer);
        if ((onGround || onPlatform) && rb2d.velocity.y < 0.1f)
        {
            if (playerInput.getDirX() > 0.1 || playerInput.getDirX() < -0.1)
            {
                nextStatus = Status.Run;
            }
            else
            {
                nextStatus = Status.Idle;
            }
        }
        else if (rb2d.velocity.y < 0.0f)
        {
            bool onLadder = Physics2D.OverlapCircle((Vector2)transform.position + midOffset, sensorRadius, ladderLayer);
            if (onLadder)
            {
                nextStatus = Status.Hang;
            }
            else
            {
                nextStatus = Status.Fall;
            }
        }
        /*if (onLadder && playerInput.getShdR() > 0.5f && statusTime > 0.5f) {
            nextStatus = Status.Hang;
        }*/
    }

    private void checkFall()
    {
        bool onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, groundLayer);
        bool onPlatform = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, platformLayer);
        if ((onGround || onPlatform))
        {
            if (playerInput.getDirX() > 0.1f || playerInput.getDirX() < -0.1f)
            {
                nextStatus = Status.Run;
            }
            else
            {
                nextStatus = Status.Idle;
            }
        }
        else if (playerInput.getBtnA() > 0.5f)
        {
            nextStatus = Status.Fly;
        }
        else if (playerInput.consumeBtnXDown(0.2f))
        {
            nextStatus = Status.Attack;
            switch (equipWeapon)
            {
                case Weapon.Katateken:
                    nextSkillCode = Skill.katateken_fall_attack;
                    break;
            }
        }
        else
        {
            if (playerInput.getShdR() > 0.5f && statusTime > 0.2f)
            {
                bool onLadder = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, ladderLayer);
                if (onLadder)
                {
                    nextStatus = Status.Hang;
                }
            }
            else
            {
                bool onWater = Physics2D.OverlapCircle((Vector2)transform.position + midOffset, sensorRadius, waterLayer);
                if (onWater)
                {
                    nextStatus = Status.Suspend;
                }
            }
        }
    }

    private void checkFly()
    {
        bool onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, groundLayer);
        bool onPlatform = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, platformLayer);
        bool onLadder = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, ladderLayer);
        bool onWater = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, waterLayer);
        if ((onGround || onPlatform))
        {
            if (playerInput.getDirX() > 0.2 || playerInput.getDirX() < -0.2)
            {
                nextStatus = Status.Run;
            }
            else
            {
                nextStatus = Status.Idle;
            }
        }
        else if (playerInput.getBtnA() < 0.5f)
        {
            nextStatus = Status.Fall;
        }
        else if (onLadder && playerInput.getShdR() > 0.5f)
        {
            nextStatus = Status.Hang;
        }
        else if (onWater)
        {
            nextStatus = Status.Suspend;
        }

    }

    private void checkSuspend()
    {
        if (rb2d.velocity.x < swimSpeed && rb2d.velocity.x > -swimSpeed && rb2d.velocity.y < swimSpeed && rb2d.velocity.y > -swimSpeed)
        {
            //need to solve the situation: the water sink
            bool onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, groundLayer);
            bool onPlatform = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, platformLayer);
            bool onWater = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, waterLayer);
            if (playerInput.getBtnB() > 0.5f)
            {
                nextStatus = Status.SwimAttack;
            }
            else if (playerInput.getDirX() > 0.1f || playerInput.getDirX() < -0.1f || playerInput.getDirY() > 0.1f || playerInput.getDirY() < -0.1f)
            {
                nextStatus = Status.Swim;
            }
            else if (!onWater && (onGround || onPlatform))
            {
                nextStatus = Status.Idle;
            }
        }

    }

    private void checkSwim()
    {
        //need to solve the situation: the water sink
        bool onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, groundLayer);
        bool onPlatform = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, platformLayer);
        bool onWater = Physics2D.OverlapCircle((Vector2)transform.position + topOffset, sensorRadius, waterLayer);
        if (!onWater && (onGround || onPlatform))
        {
            nextStatus = Status.Idle;
        }
        else if (playerInput.getBtnB() > 0.5f)
        {
            nextStatus = Status.SwimAttack;
        }
        else if (playerInput.getDirX() < 0.1f && playerInput.getDirX() > -0.1f && playerInput.getDirY() < 0.1f && playerInput.getDirY() > -0.1f)
        {
            nextStatus = Status.Suspend;
        }
    }

    private void checkSwimAttack()
    {
        if (statusTime > 0.5f)
        {
            if (playerInput.getDirX() > 0.1f || playerInput.getDirX() < -0.1f || playerInput.getDirY() > 0.1f || playerInput.getDirY() < -0.1f)
            {
                nextStatus = Status.Swim;
            }
            else
            {
                nextStatus = Status.Suspend;
            }
        }
    }

    private void checkHang()
    {
        if (playerInput.getShdR() > 0.5f && statusTime > 0.2f)
        {
            nextStatus = Status.Fall;
        }
        else if (playerInput.getDirX() > 0.1f || playerInput.getDirX() < -0.1f || playerInput.getDirY() > 0.1f || playerInput.getDirY() < -0.1f)
        {
            nextStatus = Status.Climb;
        }
        else if (playerInput.getBtnA() > 0.5f)
        {
            nextStatus = Status.ClimbJump;
        }
        else
        {
            bool onWater = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, waterLayer);
            if (onWater)
            {
                nextStatus = Status.Suspend;
            }
        }
    }

    private void checkClimbJump()
    {
        bool onLadder = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, ladderLayer);
        if (statusTime > 0.15f)
        {
            if (!onLadder)
            {
                if (playerInput.getBtnA() > 0.5f)
                {
                    nextStatus = Status.Fly;
                }
                else
                {
                    nextStatus = Status.Fall;
                }
            }
            else
            {
                if (playerInput.getDirX() > 0.1f || playerInput.getDirX() < -0.1f || playerInput.getDirY() > 0.1f || playerInput.getDirY() < -0.1f)
                {
                    nextStatus = Status.Climb;
                }
                else
                {
                    nextStatus = Status.Hang;
                }
            }
        }
    }

    private void checkClimb()
    {
        bool onLadder = Physics2D.OverlapCircle((Vector2)transform.position + midOffset, sensorRadius, ladderLayer);
        bool onPlatform = Physics2D.OverlapCircle((Vector2)transform.position + midOffset, 0.4f, platformLayer);
        if (playerInput.getShdR() > 0.5f || !onLadder)
        {
            nextStatus = Status.Fall;
        }
        else if (onPlatform && playerInput.getDirY() > 0.1f) 
        {
            nextStatus = Status.Idle;
        }
        else if (playerInput.getDirX() < 0.1f && playerInput.getDirX() > -0.1f && playerInput.getDirY() < 0.1f && playerInput.getDirY() > -0.1f)
        {
            nextStatus = Status.Hang;
        }
        else if (playerInput.getBtnA() > 0.5f)
        {
            nextStatus = Status.ClimbJump;
        }
    }

    private void checkAttack() {
        switch (skillCode) {
            case Skill.katateken_attack1:
                if (skillPhase == 0 && playerInput.consumeBtnXDown(0.2f)) { 
                    nextStatus = Status.Attack;
                    nextSkillCode = Skill.katateken_attack2;
                }
                else if (statusTime > 1.0f)
                {
                    nextStatus = Status.Idle;
                }
                break;
            case Skill.katateken_attack2:
                if (skillPhase == 0 && playerInput.consumeBtnXDown(0.2f))
                {
                    nextStatus = Status.Attack;
                    nextSkillCode = Skill.katateken_attack3;
                }
                else if (statusTime > 1.0f)
                {
                    nextStatus = Status.Idle;
                }
                break;
            case Skill.katateken_attack3:
                if (skillPhase == 0 && playerInput.consumeBtnXDown(0.2f))
                {
                    nextStatus = Status.Attack;
                    nextSkillCode = Skill.katateken_attack1;
                }
                else if (statusTime > 1.0f)
                {
                    nextStatus = Status.Idle;
                }
                break;
            case Skill.katateken_fall_attack:
                break;
            case Skill.katateken_defence:
                break;
            case Skill.katateken_defence_attack:
                break;
        }
    }
    #endregion

    private void processStatus()
    {
        switch (status)
        {
            case Status.Idle:
                pIdle();
                break;
            case Status.Squat:
                pSquat();
                break;
            case Status.Run:
                pRun();
                break;
            case Status.Jump:
                pJump();
                break;
            case Status.Fall:
                pFall();
                break;
            case Status.Fly:
                pFly();
                break;
            case Status.Suspend:
                pSuspend();
                break;
            case Status.Swim:
                pSwim();
                break;
            case Status.Hang:
                pHang();
                break;
            case Status.SwimAttack:
                pSwimAttack();
                break;
            case Status.ClimbJump:
                pClimbJump();
                break;
            case Status.Climb:
                pClimb();
                break;
            case Status.Attack:
                pAttack();
                break;
        }
    }

    #region process player status
    private void pIdle()
    {
        switch (statusCode)
        {
            case 0x0001://idle to squat
                break;
            case 0x0002://climb to idle
                break;
            case 0x0003://run to squat
                break;
            default:
                break;
        }
        rb2d.velocity = new Vector2(0.0f, 0.0f);
    }
    private void pSquat() {
        if (statusTime < 0.5f) {
            if (playerInput.getDirX() > 0.5f)
            {
                rb2d.velocity = new Vector2(maxSpeed, 0.0f);
            }else if (playerInput.getDirX() < -0.5f)
            {
                rb2d.velocity = new Vector2(-maxSpeed, 0.0f);
            }
        }
        else
        {
            rb2d.velocity = new Vector2(0.0f, 0.0f);
        }
    }
    private void pRun()
    {
        float frameTime = 0.1f;
        double count = Math.Floor(statusTime / frameTime);
        double c = count - Math.Floor(count / 8.0) * 8.0 + 8.0;
        bodySprd.material.SetFloat("_Show", (float)c);
        weapSprd.material.SetFloat("_Show", 0f);
        maskSprd.material.SetFloat("_Show", (float)c);
        if (playerInput.getDirX() > 0f)
        {
            rb2d.velocity = new Vector2(maxSpeed, 0.0f);
        }
        else
        {
            rb2d.velocity = new Vector2(-maxSpeed, 0.0f);
        }
        //rb2d.AddForce(Vector2.right*(playerInput.getDirX() * maxSpeed-rb2d.velocity.x) * accScale);
    }
    private void pJump()
    {
        //statusCode首位表示A键是否松开过，保证一直按着A键不触发二段跳
        /*if (playerInput.getBtnA() < 0.5f) {
            statusCode = statusCode | 0x8000;
        }
        rb2d.AddForce(Vector2.right*(playerInput.getDirX()*maxSpeed-rb2d.velocity.x)*accInAirScale + Vector2.down * gravity);*/
        rb2d.AddForce(Vector2.down * gravity);
    }

    private void pFall()
    {
        //rb2d.AddForce(Vector2.right * (playerInput.getDirX() * maxSpeed - rb2d.velocity.x) * accInAirScale + Vector2.down * gravity * (1.0f + rb2d.velocity.y / maxFallSpeed));
        rb2d.AddForce(Vector2.down * gravity);
        rb2d.velocity = new Vector2(playerInput.getDirX() * maxSpeed, rb2d.velocity.y);
    }

    private void pFly()
    {
        rb2d.AddForce(Vector2.right * (playerInput.getDirX() * FlySpeed - rb2d.velocity.x) * accInAirScale + Vector2.down * gravity * (1.0f + rb2d.velocity.y / flyFallSpeed));
    }

    private void pSuspend()
    {
        float vx = 0f;
        float vy = 0f;
        if (rb2d.velocity.x > swimSpeed)
        {
            float v = rb2d.velocity.x;
            vx = v - v * v * Time.fixedDeltaTime * 1.2f;
            if (vx < swimSpeed)
            {
                vx = 0f;
            }
        }
        else if (rb2d.velocity.x < -swimSpeed)
        {
            float v = rb2d.velocity.x;
            vx = v + v * v * Time.fixedDeltaTime * 1.2f;
            if (vx > -swimSpeed)
            {
                vx = 0f;
            }
        }

        if (rb2d.velocity.y > swimSpeed)
        {
            float v = rb2d.velocity.y;
            vy = v - v * v * Time.fixedDeltaTime * 1.2f;
            if (vy < swimSpeed)
            {
                vy = 0f;
            }
        }
        else if (rb2d.velocity.y < -swimSpeed)
        {
            float v = rb2d.velocity.y;
            vy = v + v * v * Time.fixedDeltaTime * 1.2f;
            if (vy > -swimSpeed)
            {
                vy = 0f;
            }
        }
        rb2d.velocity = new Vector2(vx, vy);
    }

    private void pSwim()
    {
        bool headInWater = Physics2D.OverlapCircle((Vector2)transform.position + topOffset, sensorRadius, waterLayer);
        float dir_y = playerInput.getDirY();
        if (!headInWater && playerInput.getDirY() > 0f)
        {
            dir_y = 0f;
        }
        rb2d.AddForce(Vector2.right * (playerInput.getDirX() * swimSpeed - rb2d.velocity.x) * accInAirScale + Vector2.up * (dir_y * swimSpeed - rb2d.velocity.y) * accInAirScale);
    }

    private void pHang()
    {
        rb2d.velocity = new Vector2(0, 0);
    }

    private void pSwimAttack()
    {
    }

    private void pClimbJump()
    {
    }

    private void pClimb()
    {
        float v_x = playerInput.getDirX() * climbSpeed;
        float v_y = playerInput.getDirY() * climbSpeed;
        if (playerInput.getDirX() > 0)
        {
            bool rightLadder = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, sensorRadius, ladderLayer);
            if (rightLadder == false)
            {
                v_x = 0;
            }
        }
        else
        {
            bool leftLadder = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, sensorRadius, ladderLayer);
            if (leftLadder == false)
            {
                v_x = 0;
            }
        }
        if (playerInput.getDirY() > 0)
        {
            bool topLadder = Physics2D.OverlapCircle((Vector2)transform.position + topOffset, sensorRadius, ladderLayer);
            if (topLadder == false)
            {
                v_y = 0;
            }
        }
        else
        {
            bool bottomLadder = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, ladderLayer);
            if (bottomLadder == false)
            {
                v_y = 0;
            }
        }
        rb2d.velocity = new Vector2(v_x, v_y);
    }

    private void pAttack() {
        switch (skillCode)
        {
            case Skill.katateken_attack1:
                if (statusTime > 0.15f && skillPhase == 1)
                {
                    bodySprd.material.SetFloat("_Show", 33f);
                    maskSprd.material.SetFloat("_Show", 33f);
                    weapSprd.material.SetFloat("_Show", 3f);
                    skillPhase++;
                }
                else if (statusTime > 0.45f && skillPhase == 2)
                {
                    bodySprd.material.SetFloat("_Show", 33f);
                    maskSprd.material.SetFloat("_Show", 33f);
                    weapSprd.material.SetFloat("_Show", 4f);
                    skillPhase++;
                }
                else if (statusTime > 0.6f && skillPhase == 3){
                    skillPhase = 0;
                }
                break;
            case Skill.katateken_attack2:
                if (statusTime > 0.15f && skillPhase == 1)
                {
                    bodySprd.material.SetFloat("_Show", 34f);
                    maskSprd.material.SetFloat("_Show", 34f);
                    weapSprd.material.SetFloat("_Show", 6f);
                    skillPhase++;
                }
                else if (statusTime > 0.45f && skillPhase == 2)
                {
                    bodySprd.material.SetFloat("_Show", 35f);
                    maskSprd.material.SetFloat("_Show", 35f);
                    weapSprd.material.SetFloat("_Show", 7f);
                    skillPhase++;
                }
                else if (statusTime > 0.6f && skillPhase == 3)
                {
                    skillPhase = 0;
                }
                break;
            case Skill.katateken_attack3:
                if (statusTime > 0.15f && skillPhase == 1)
                {
                    bodySprd.material.SetFloat("_Show", 36f);
                    maskSprd.material.SetFloat("_Show", 36f);
                    weapSprd.material.SetFloat("_Show", 9f);
                    skillPhase++;
                }
                else if (statusTime > 0.45f && skillPhase == 2)
                {
                    bodySprd.material.SetFloat("_Show", 37f);
                    maskSprd.material.SetFloat("_Show", 37f);
                    weapSprd.material.SetFloat("_Show", 10f);
                    skillPhase++;
                }
                else if (statusTime > 0.6f && skillPhase == 3)
                {
                    skillPhase = 0;
                }
                break;
            case Skill.katateken_fall_attack:
                break;
            case Skill.katateken_defence:
                break;
            case Skill.katateken_defence_attack:
                break;
        }
    }
    #endregion

    #region Interact

    #endregion
}

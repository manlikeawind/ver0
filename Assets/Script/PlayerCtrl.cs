using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Xml;

public class PlayerCtrl : MonoBehaviour
{
    public enum Status
    {
        None = 0,
        Idle, Run, Jump, Fall, Airhike, Fly, Climb
    }


    #region member variables
    private int abilityIndex;
    private int abilityCount = 7;
    public GameObject[] weapons = new GameObject[7];
    public float switchTime;

    //collider parameter
    private float sensorRadius = 0.1f;
    public Vector2 bottomOffset;

    //collider check
    private bool onGround = false;
    private bool onPlatform = false;
    public bool onLadder = false;

    //interactor near player
    private Hashtable interactor = new Hashtable();
    private GameObject activePlatform;

    //player status
    public float faceTo = 1.0f;
    public Status status = Status.Idle;
    public Status nextStatus = Status.None;
    private float statusTime = 0.0f;
    private int airhike = 0;
    private uint statusCode = 0x0000;
    private float[] statusFloatInfo = { 0.0f, 0.0f, 0.0f, 0.0f }; 

    //player parameter
    private float maxSpeed = 6.0f;
    private float jumpSpeed = 15.0f;
    private float airhikeSpeed = 20.0f;
    private float FlySpeed = 12.0f;
    private float flyFallSpeed = 2.0f;
    private float maxFallSpeed = 40.0f;
    private float accScale = 20.0f;
    private float accInAirScale = 5.0f;
    private float gravity = 40.0f;

    private float idle2run = 0.5f;

    //input
    private float dir_x;
    private float dir_y;
    private float btn_a;
    private float btn_b;
    private float btn_x;
    private float btn_y;
    private float shd_l;
    private float shd_r;
    private float stk_x;
    private float stk_y;
    private float crs_x;
    private float crs_y;

    //player gameObject
    private Rigidbody2D rb2d;
    private SpriteRenderer sprd;
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        rb2d = GetComponent<Rigidbody2D>();
        string[] interactorList = { "ladder", "ant" };
        foreach (string i in interactorList) {
            var tmp = new ArrayList();
            interactor.Add(i, tmp);
        }

        switchTime = 0f;
        abilityIndex = 0;
        weapons[0].SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, sensorRadius);
    }

    private void Update()
    {

        processInput();

        detecteCollision();

        statusTime += Time.deltaTime;

        //whether move to next status
        if (nextStatus == Status.None)
        {
            checkStatus();
        }

        //initiate next status
        if (nextStatus != Status.None)
        {
            initNextStatus();
        }
        if (dir_x > 0.0f)
        {
            faceTo = 1.0f;
        }
        else if (dir_x < 0.0f) {
            faceTo = -1.0f;
        }
        if (faceTo < 0.0f)
        {
            transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }

        switchTime += Time.deltaTime;
        if (crs_y > 0.5f) {
            if (switchTime > 0.5f && stk_x > 0.5f)
            {
                weapons[abilityIndex].SetActive(false);
                abilityIndex = (abilityIndex + 1) % abilityCount;
                weapons[abilityIndex].SetActive(true);
                switchTime = 0f;
            }
            else if (switchTime > 0.5f && stk_x < -0.5f) {
                weapons[abilityIndex].SetActive(false);
                abilityIndex = (abilityIndex - 1 + abilityCount) % abilityCount;
                weapons[abilityIndex].SetActive(true);
                switchTime = 0f;
            }
        }
    }
    public void FixedUpdate()
    {
        processStatus();
    }

    private void detecteCollision()
    {
        LayerMask groundLayer = 1 << LayerMask.NameToLayer("GroundLayer");
        LayerMask platformLayer = 1 << LayerMask.NameToLayer("Platform");
        LayerMask ladderLayer = 1 << LayerMask.NameToLayer("Ladder");
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, groundLayer);
        onPlatform = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, platformLayer);
        onLadder = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, ladderLayer);
    }

    private void processInput()
    {
        var gp = Gamepad.current;
        if (gp == null) return;
        dir_x = gp.leftStick.ReadValue().x;
        dir_y = gp.leftStick.ReadValue().y;
        btn_a = gp.buttonSouth.ReadValue();
        btn_b = gp.buttonEast.ReadValue();
        shd_r = gp.rightShoulder.ReadValue();
        stk_x = gp.rightStick.ReadValue().x;
        stk_y = gp.rightStick.ReadValue().y;
        crs_x = gp.dpad.ReadValue().x;
        crs_y = gp.dpad.ReadValue().y;
    }

    private void checkStatus()
    {
        switch (status)
        {
            case Status.Idle:
                checkIdle();
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
            case Status.Airhike:
                checkAirhike();
                break;
            case Status.Fly:
                checkFly();
                break;
            case Status.Climb:
                checkClimb();
                break;
        }
    }

    private void initNextStatus()
    {
        switch (nextStatus)
        {
            case Status.Idle:
                rb2d.velocity = new Vector2(0.0f, 0.0f);
                break;
            case Status.Run:
                break;
            case Status.Jump:
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
                break;
            /*case Status.Airhike:
                airhike = 1;
                if (dir_x > 0.3f)
                {
                    rb2d.velocity = new Vector2(maxSpeed, airhikeSpeed);
                }
                else if (dir_x < -0.3f)
                {
                    rb2d.velocity = new Vector2(-maxSpeed, airhikeSpeed);
                }
                else {
                    rb2d.velocity = new Vector2(0.0f, airhikeSpeed);
                }
                break;*/
            case Status.Fall:
                break;
            case Status.Fly:
                break;
            case Status.Climb:
                ArrayList ladders = (ArrayList)interactor["ladder"];
                float closest = 999999999.0f;
                GameObject validLadder = null;
                foreach (GameObject obj in ladders) {
                    float distance = Math.Abs(obj.transform.position.x - this.transform.position.x);
                    if (distance < closest) {
                        validLadder = obj;
                        closest = distance;
                    }
                }
                if (validLadder != null) {
                    this.transform.position = new Vector2(validLadder.transform.position.x, this.transform.position.y);
                    rb2d.velocity = new Vector2(0.0f, 0.0f);
                    statusFloatInfo[0] = validLadder.transform.position.x;
                    statusFloatInfo[1] = validLadder.transform.position.y;
                }
                break;
        }

        status = nextStatus;
        nextStatus = Status.None;
        statusTime = 0.0f;
        statusCode = 0x0000;
    }

    #region check player status
    private void checkIdle()
    {
        if (!(onGround || onPlatform))
        {
            nextStatus = Status.Fall;
        }
        else if (btn_a > 0.5f /*&& (dir_y < 0.8f || dir_x > 0.8f || dir_x < -0.8f)*/)
        {
            nextStatus = Status.Jump;
        }
        else if (dir_x > idle2run)
        {
            nextStatus = Status.Run;
        }
        else if (dir_x < -idle2run)
        {
            nextStatus = Status.Run;
        }
        else if (onLadder && dir_y < -0.5f) {
            nextStatus = Status.Climb;
            activePlatform.GetComponent<PlatformEffector2D>().rotationalOffset = 180f;
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.4f, transform.position.z);
        }
    }

    private void checkRun()
    {
        if (!(onGround || onPlatform))
        {
            nextStatus = Status.Fall;
        }
        else if (btn_a > 0.5f /*&& (dir_y < 0.8f || dir_x > 0.8f || dir_x < -0.8f)*/)
        {
            nextStatus = Status.Jump;
        }
        else if (rb2d.velocity.x > -0.05f && rb2d.velocity.x < 0.05f && dir_x > -idle2run && dir_x < idle2run)
        {
            nextStatus = Status.Idle;
        }
    }

    private void checkJump()
    {
        if ((onGround || onPlatform) && rb2d.velocity.y < 0.1f)
        {
            airhike = 0;
            if (dir_x > 0.1 || dir_x < -0.1)
            {
                nextStatus = Status.Run;
            }
            else
            {
                nextStatus = Status.Idle;
            }
        }
        /*else if (btn_a > 0.5f && (statusCode & 0x8000) != 0 && airhike == 0)
        {
            nextStatus = Status.Airhike;
        }*/
        else if (rb2d.velocity.y < 0.0f)
        {
            nextStatus = Status.Fall;
        }
        if (onLadder && shd_r > 0.5f && statusTime > 0.2f) {
            nextStatus = Status.Climb;
        }
    }

    private void checkFall()
    {
        if ((onGround || onPlatform))
        {
            airhike = 0;
            if (dir_x > 0.2 || dir_x < -0.2)
            {
                nextStatus = Status.Run;
            }
            else
            {
                nextStatus = Status.Idle;
            }
        }
        /*else if (btn_a > 0.5f && airhike == 0)
        {
            nextStatus = Status.Airhike;
        }*/
        else if (btn_a > 0.5f) {
            nextStatus = Status.Fly;
        }
        if (onLadder && shd_r > 0.5f)
        {
            nextStatus = Status.Climb;
        }
    }

    private void checkAirhike()
    {
        if ((onGround || onPlatform) && rb2d.velocity.y < 0.1f)
        {
            airhike = 0;
            if (dir_x > 0.2 || dir_x < -0.2)
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
            nextStatus = Status.Fall;
        }
    }

    private void checkFly() {
        if ((onGround || onPlatform))
        {
            airhike = 0;
            if (dir_x > 0.2 || dir_x < -0.2)
            {
                nextStatus = Status.Run;
            }
            else
            {
                nextStatus = Status.Idle;
            }
        }
        else if (btn_a < 0.5f)
        {
            nextStatus = Status.Fall;
        }
    }

    private void checkClimb() {
        /*if ((onGround || onPlatform) && dir_y > 0.2f) {
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y+0.7f);
            nextStatus = Status.Idle;
        }*/
        if (!onLadder)
        {
            nextStatus = Status.Fall;
        }
        else if (this.transform.position.y > statusFloatInfo[1]-0.2f) {
            nextStatus = Status.Idle;
            this.transform.position = new Vector2(statusFloatInfo[0], statusFloatInfo[1]);
        }
        else if (btn_a > 0.5f) {
            if (dir_x > 0.3f)
            {
                rb2d.velocity = new Vector2(maxSpeed, 0.0f);
                nextStatus = Status.Fall;
            }
            else if (dir_x < -0.3f)
            {
                rb2d.velocity = new Vector2(-maxSpeed, 0.0f);
                nextStatus = Status.Fall;
            }
            else {
                nextStatus = Status.Fall;
            }
        }
    }
    #endregion

    private void processStatus()
    {
        switch (status)
        {
            case Status.Idle:
                p_idle();
                break;
            case Status.Run:
                p_run();
                break;
            case Status.Jump:
                p_jump();
                break;
            case Status.Fall:
                p_fall();
                break;
            /*case Status.Airhike:
                p_airhike();
                break;*/
            case Status.Fly:
                p_fly();
                break;
            case Status.Climb:
                p_climb();
                break;
        }
    }

    #region process player status
    private void p_idle()
    {
        rb2d.velocity = new Vector2(0.0f, 0.0f);
    }

    private void p_run()
    {
        rb2d.AddForce(Vector2.right*(dir_x * maxSpeed-rb2d.velocity.x) * accScale);
    }
    private void p_jump()
    {
        //statusCode首位表示A键是否松开过，保证一直按着A键不触发二段跳
        if (btn_a < 0.5f) {
            statusCode = statusCode | 0x8000;
        }
        rb2d.AddForce(Vector2.right*(dir_x*maxSpeed-rb2d.velocity.x)*accInAirScale + Vector2.down * gravity);
    }

    private void p_fall()
    {
        rb2d.AddForce(Vector2.right * (dir_x * maxSpeed - rb2d.velocity.x) * accInAirScale + Vector2.down * gravity * (1.0f + rb2d.velocity.y / maxFallSpeed));
    }
    private void p_airhike() {
        rb2d.AddForce(Vector2.right * (dir_x * maxSpeed - rb2d.velocity.x) * accInAirScale + Vector2.down * gravity * (1.0f + rb2d.velocity.y / maxFallSpeed));
    }
    private void p_fly() {
        rb2d.AddForce(Vector2.right * (dir_x * FlySpeed - rb2d.velocity.x) * accInAirScale + Vector2.down * gravity * (1.0f + rb2d.velocity.y / flyFallSpeed));
    }

    private void p_climb() {
        if (dir_y > 0.5)
        {
            rb2d.velocity = new Vector2(0.0f, 1.6f);
        }
        else if (dir_y < -0.5f)
        {
            rb2d.velocity = new Vector2(0.0f, -3.2f);
        }
        else {
            rb2d.velocity = new Vector2(0.0f, 0.0f);
        }
    }
    #endregion

    #region Interact
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag) {
            case "Ladder":
                ArrayList ladders = (ArrayList)interactor["ladder"];
                ladders.Add(collision.gameObject);
                break;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ladder":
                ArrayList ladders = (ArrayList)interactor["ladder"];
                ladders.Remove(collision.gameObject);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag) {
            case "Platform":
                activePlatform = collision.gameObject;
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tama : MonoBehaviour
{
    private const float walkSpeed = 3f;
    private const float runSpeed = 5f;
    private const float airSpeed = 3f;
    private const float jumpSpeed = 12f;
    private const float climbSpeed = 3f;
    private const float gravity = 30f;

    public enum Status
    {
        None = 0,
        Idle, Run, Lie, Jump, Fall, Climb
    }

    public Status status;
    public Status nextStatus;
    private int statusIntParam1;
    public float statusTime;
    private Transform interact;

    private float dir_x;
    private float dir_y;

    private float btn_a = 0f;
    private float pre_a = 0f;
    private float a_time = 0f;
    private float btn_b;
    private float btn_x;
    private float btn_y;

    public bool onGround = false;
    public bool onPlatform = false;
    public bool onLadder = false;

    private float fireTime;
    private float windTime;

    private float opInterval;

    //collider parameter
    public float sensorRadius;
    public Vector2 bottomOffset;

    private LayerMask groundLayer;
    private LayerMask platformLayer;
    private LayerMask ladderLayer;

    private Rigidbody2D rb2d;
    private SpriteRenderer sprd;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, sensorRadius);
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        rb2d = GetComponent<Rigidbody2D>();
        sprd = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        status = Status.Idle;
        nextStatus = Status.None;
        interact = null;
        groundLayer = 1 << LayerMask.NameToLayer("GroundLayer");
        platformLayer = 1 << LayerMask.NameToLayer("Platform");
        ladderLayer = 1 << LayerMask.NameToLayer("Ladder");
    }

    // Update is called once per frame
    void Update()
    {
        var gp = Gamepad.current;
        if (gp == null) return;
        dir_x = gp.leftStick.ReadValue().x;
        dir_y = gp.leftStick.ReadValue().y;

        btn_a = gp.buttonSouth.ReadValue();
        btn_b = gp.buttonEast.ReadValue();
        btn_x = gp.buttonWest.ReadValue();
        btn_y = gp.buttonNorth.ReadValue();

        if (btn_a != pre_a) {
            a_time = 0;
            pre_a = btn_a;
        }
        a_time += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, groundLayer);
        onPlatform = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, platformLayer);
        onLadder = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, sensorRadius, ladderLayer);

        if(status == Status.Idle || status == Status.Run)
        {
            if(interact != null && btn_a > 0.5f)
            {
                interact.GetComponent<Interact>().startInteract();
            }
        }
        //check status
        if (nextStatus == Status.None)
        {
            checkStatus();
        }

        //initiate next status
        if (nextStatus != Status.None)
        {
            initStatus();
        }

        //process status
        processStatus();

        statusTime += Time.fixedDeltaTime;
    }

    private void checkStatus() {
        if (nextStatus == Status.None)
        {
            switch (status)
            {
                case Status.Idle:
                    if (!(onGround || onPlatform))
                    {
                        nextStatus = Status.Fall;
                    }
                    else if (dir_y < -0.9f) 
                    {
                        nextStatus = Status.Lie;
                    }
                    else if (btn_a > 0.5f)
                    {
                        nextStatus = Status.Jump;
                    }
                    else if (dir_x > 0.5f)
                    {
                        nextStatus = Status.Run;
                    }
                    else if (dir_x < -0.5f)
                    {
                        nextStatus = Status.Run;
                    }
                    else if (onLadder && dir_y < -0.5f)
                    {
                    }
                    break;
                case Status.Run:
                    if (!(onGround || onPlatform))
                    {
                        nextStatus = Status.Fall;
                    }
                    else if (dir_y < -0.9f)
                    {
                        nextStatus = Status.Lie;
                    }
                    else if (btn_a > 0.5f)
                    {
                        nextStatus = Status.Jump;
                    }
                    else if (rb2d.velocity.x > -0.05f && rb2d.velocity.x < 0.05f && dir_x > -0.5f && dir_x < 0.5f)
                    {
                        nextStatus = Status.Idle;
                    }
                    break;
                case Status.Lie:
                    if (dir_y > 0f)
                    {
                        nextStatus = Status.Idle;
                    } else if (btn_a > 0.5f) 
                    {
                        nextStatus = Status.Fall;
                    }
                    break;
                case Status.Jump:
                    if ((onGround || onPlatform) && rb2d.velocity.y < 0.1f)
                    {
                        if (dir_x > 0.1 || dir_x < -0.1)
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
                    else if (onLadder && btn_a > 0.5f && a_time < 0.1f)
                    {
                        nextStatus = Status.Climb;
                    }
                    break;
                case Status.Fall:
                    if ((onGround || onPlatform))
                    {
                        if (dir_x > 0.2 || dir_x < -0.2)
                        {
                            nextStatus = Status.Run;
                        }
                        else
                        {
                            nextStatus = Status.Idle;
                        }
                    }
                    else if (onLadder && btn_a > 0.5f && a_time < 0.1f)
                    {
                        nextStatus = Status.Climb;
                    }
                    break;
                case Status.Climb:
                    if (!onLadder) 
                    {
                        nextStatus = Status.Fall;
                    }
                    break;
            }
        }
    }

    private void initStatus()
    {
        switch (nextStatus)
        {
            case Status.Idle:
                rb2d.velocity = new Vector2(0.0f, 0.0f);
                break;
            case Status.Run:
                break;
            case Status.Lie:
                rb2d.velocity = new Vector2(0.0f, 0.0f);
                break;
            case Status.Jump:
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
                statusIntParam1 = 0;
                break;
            case Status.Fall:
                if (onPlatform == true) 
                {
                    transform.Translate(0f, -0.5f, 0f);
                }
                break;
            case Status.Climb:
                RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0.5f, 0f), Vector2.right, 1f, ladderLayer);
                if (hit.collider != null)
                {
                    float xOffset = hit.point.x - transform.position.x;
                    transform.Translate(xOffset, 0f, 0f);
                    rb2d.velocity = new Vector2(0.0f, 0.0f);
                }
                break;
        }

        status = nextStatus;
        nextStatus = Status.None;
        statusTime = 0.0f;
    }

    private void processStatus() {
        float deltaTime = Time.fixedDeltaTime;
        switch (status)
        {
            case Status.Idle:
                break;
            case Status.Lie:
                break;
            case Status.Run:
                rb2d.velocity = new Vector2(dir_x * runSpeed, 0);
                rb2d.AddForce(Vector2.down * gravity);
                break;
            case Status.Jump:
                /*float startTime = 0.2f;
                float endTime = 0.7f;
                if (statusIntParam1 == 0)
                {
                    if (btn_a < 0.5f || statusTime > endTime)
                    {
                        statusIntParam1 = 1;
                    }
                    if (statusTime > startTime) 
                    {
                        float rate = (statusTime - startTime) / (endTime - startTime);
                        rb2d.AddForce(Vector2.down * gravity * rate);
                    }

                }
                else
                {
                    rb2d.AddForce(Vector2.down * gravity);
                }*/
                rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y - gravity * deltaTime);
                break;
            case Status.Fall:
                rb2d.velocity = new Vector2(dir_x * airSpeed, rb2d.velocity.y - gravity * deltaTime);
                break;
            case Status.Climb:
                rb2d.velocity = new Vector2(0, dir_y * climbSpeed);
                break;
        }
        
    }

    public void setInteract(Transform transform) {
        interact = transform;
    }
}

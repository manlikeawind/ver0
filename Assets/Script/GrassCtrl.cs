using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class GrassCtrl : BaseObj
{
    private enum Status
    {
        None = 0,
        Idle, Winded, Fired, Freeze, Reaped, Dead
    };

    private int type;
    private float hp;
    private float ignition;
    private float minTemp;
    private bool isFired;
    private float temprature;
    private float heatEnergy;
    private Vector2 windEnergy;
    private float envTemp;
    private Status status;
    private Status nextStatus;
    private float statusTime;
    private GameObject myFire;
    private Animator anim;

    private AreaManager manager;

    public override void construct(SqliteDataReader reader)
    {
    }

    public override void fired()
    {
        if (!isFired) {
            heatEnergy += Time.fixedDeltaTime * 50;
        }
    }

    public override void heat()
    {
        if (temprature < envTemp + 10f) {
            heatEnergy += Time.fixedDeltaTime * 20;
        }
    }

    public override void freeze()
    {
        if(temprature > -50f)
        {
            heatEnergy -= Time.fixedDeltaTime * 20;
        }
    }

    public override void cooling()
    {
        if (temprature > 5f)
        {
            heatEnergy -= Time.fixedDeltaTime * 10;
        }
    }

    public override void wind(float dirx, float diry)
    {
        windEnergy = new Vector2(dirx, diry);
    }

    public override void attacked(int weaponType, float damage)
    {
        if (weaponType == 1 && hp > 0f) {
            hp = 0f;
            nextStatus = Status.Reaped;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        status = Status.Idle;
        nextStatus= Status.None;
        statusTime = 0f;
        type = 0;
        hp = 100f;
        isFired = false;
        ignition = 100f;
        minTemp = -100f;
        temprature = 20f;
        heatEnergy = 0f;
        windEnergy = new Vector2(0f, 0f);
        envTemp = 20f;
        anim = GetComponent<Animator>();
        manager = GameObject.FindGameObjectWithTag("AreaManager").GetComponent<AreaManager>();
    }

    private void FixedUpdate()
    {
        if (isFired) {
            hp -= 20f * Time.fixedDeltaTime;
            if (hp < 0f) {
                nextStatus = Status.Dead;
            }
        }

        if (heatEnergy > 50 * Time.fixedDeltaTime)
        {
            heatEnergy = 50 * Time.fixedDeltaTime;
        }
        else if (heatEnergy < -20 * Time.fixedDeltaTime) {
            heatEnergy = -20 * Time.fixedDeltaTime;
        }
        temprature += heatEnergy;
        heatEnergy = 0f;

        if (temprature > ignition)
        {
            if (myFire == null) {
                GameObject fire = manager.instGameObject("fire");
                fire.GetComponent<Fire>().setMainObj(transform.gameObject);
                fire.transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);

                isFired = true;
                myFire = fire;
            }
        }
        else if (temprature > envTemp + 1f)
        {
            temprature -= 1f * Time.fixedDeltaTime;
        }else if(temprature < envTemp - 1f)
        {
            temprature += 1f * Time.fixedDeltaTime;
        }
        if (nextStatus != Status.None) 
        {
            initStatus();
        }

        processStatus();
    }

    private void initStatus() {
        switch (nextStatus)
        {
            case Status.Idle:
                anim.Play("idle");
                break;
            case Status.Winded:
                anim.Play("wind");
                break;
            case Status.Freeze:
                anim.Play("freeze");
                break;
            case Status.Reaped:
                anim.Play("reaped");
                break;
            case Status.Dead:
                transform.gameObject.SetActive(false);
                break;
        }
        status = nextStatus;
        nextStatus = Status.None;
        statusTime = 0f;
    }

    private void processStatus() {
        switch (status)
        {
            case Status.Idle:
                if (temprature < 0f)
                {
                    nextStatus = Status.Freeze;
                }
                else if (windEnergy.x*windEnergy.x > 0.01f)
                {
                    nextStatus = Status.Winded;
                }
                break;
            case Status.Winded:
                windEnergy = windEnergy / 2;
                if(windEnergy.x > 0f)
                {
                    transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                }
                if (temprature < 0f)
                {
                    nextStatus = Status.Freeze;
                }
                else if (windEnergy.x * windEnergy.x < 0.01f)
                {
                    nextStatus = Status.Idle;
                }
                break;
            case Status.Freeze:
                if (temprature < minTemp)
                {
                    temprature = minTemp;
                }
                if (temprature > 0)
                {
                    nextStatus = Status.Idle;
                }
                break;
            case Status.Reaped:
                statusTime += Time.fixedDeltaTime;
                if (statusTime > 0.5f) {
                    nextStatus = Status.Dead;
                }
                break;
            case Status.Dead:
                break;
        }
    }

}

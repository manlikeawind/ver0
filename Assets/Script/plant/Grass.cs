using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : BaseObj
{
    [System.Serializable]
    public class Attr
    {
        public int status;
        public float ignition;
        public float hp;
        public float fireRadius;
    }

    public Vector2 windEnergy;
    private float heatEnergy;
    private bool isFired;
    private bool isWind;

    private Attr attr;
    private Animator anim;
    private AreaManager manager;
    private SpriteRenderer sprd;

    public override void construct(string info)
    {
        attr = JsonUtility.FromJson<Attr>(info);
        if (attr.status == 1)
        {
            validate();
        }
        else
        {
            invalid();
        }
    }

    public override void validate()
    {
        valid = true;
        sprd.enabled = true;
        sprd.color = new Color(1f, 1f, 1f, 1f);
    }

    public override void invalid()
    {
        valid = false;
        attr.status = 0;
        sprd.enabled = false;
    }

    public override void wind(float x, float y)
    {
        if (valid == false) return;
        windEnergy.x = x;
    }

    public override void fired()
    {
        if (valid == false) return;
        if (isFired == false)
        {
            heatEnergy = heatEnergy + Time.fixedDeltaTime;
        }
    }

    public override void attacked(Weapon weaponType, float damage)
    {
        if (valid == false) return;
        switch (weaponType)
        {
            case Weapon.Katateken:
            case Weapon.Boom:
                invalid();
                break;
            default:
                break;
        }
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        manager = GameObject.FindGameObjectWithTag("AreaManager").GetComponent<AreaManager>();
        sprd = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        windEnergy = new Vector2(0f,0f);
        heatEnergy = 0f;
        isFired = false;
        isWind = false;
    }

    private void FixedUpdate()
    {
        if (valid == false) return;

        if (heatEnergy > attr.ignition && isFired == false) {
            isFired = true;
            sprd.color = new Color(1f, 0f, 0f, 1f);
        }
        if (heatEnergy < attr.ignition && isFired == true) {
            isFired = false;
            sprd.color = new Color(1f, 1f, 1f, 1f);
        }

        if(windEnergy.x > 0.3 || windEnergy.x < -0.3 && isWind == false)
        {
            float anim_speed = Random.Range(0.6f,1.0f);
            float anim_offset = Random.Range(0.0f, 1.0f);
            anim.SetFloat("speed", anim_speed);
            //anim.SetFloat("offset", anim_offset);
            anim.SetBool("iswind",true);
            isWind = true;
        }
        if(windEnergy.x < 0.3 && windEnergy.x > -0.3 && isWind == true)
        {
            anim.SetBool("iswind", false);
            isWind = false;
        }

        if (isFired)
        {
            attr.hp -= Time.fixedDeltaTime;
            if (attr.hp < 0f)
            {
                invalid();
            }
            LayerMask objs = 1 << LayerMask.NameToLayer("Interact");
            Collider2D[] list1 = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), attr.fireRadius, objs);
            foreach (Collider2D collider in list1)
            {
                collider.gameObject.GetComponent<BaseObj>().fired();
            }
        }
        else if(heatEnergy > 0f) {
            heatEnergy -= (Time.fixedDeltaTime / 10);
        }

        if (windEnergy.x > 0.3f || windEnergy.x < -0.3f) {
            windEnergy.x = windEnergy.x * 0.9f;
        }

    }

   

}

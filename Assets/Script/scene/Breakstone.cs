using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakstone : BaseObj
{
    [System.Serializable]
    public class Attr
    {
        public int mode;
    }

    public Attr attr;
    public Sprite sprite0;
    public Sprite sprite1;
    public GameObject wall;

    private SpriteRenderer sprd;
    private AreaManager manager;
    public override void construct(string info)
    {
        attr = JsonUtility.FromJson<Attr>(info);
        if(attr.mode == 1)
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
        sprd.sprite = sprite0;
        wall.SetActive(true);
    }
    public override void invalid()
    {
        valid = false;
        sprd.sprite = sprite1;
        wall.SetActive(false);
    }
    private void Awake()
    {
        sprd = GetComponent<SpriteRenderer>();
        manager = GameObject.FindGameObjectWithTag("AreaManager").GetComponent<AreaManager>();
    }

    public override void attacked(Weapon weapon, float damage)
    {
        if (valid == false) return;
        if(weapon == Weapon.Boom)
        {
            this.invalid();
        }
    }
}

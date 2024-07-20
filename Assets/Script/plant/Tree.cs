using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : BaseObj
{
    [System.Serializable]
    public class Attr
    {
        public float hp;
        public int chopCount;
    }

    private Attr attr;
    private Hashtable fruits;
    public Sprite sprite0;
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    private SpriteRenderer sprd;
    private AreaManager manager;
    

    public override void construct(string info)
    {
        attr = JsonUtility.FromJson<Attr>(info);
        if (attr.hp <= 0 || attr.chopCount == 3)
        {
            invalid();
        }
        else
        {
            validate();
        }
    }

    public override void validate()
    {
        valid = true;
        if (attr.chopCount == 1)
        {
            sprd.sprite = sprite1;
        }
        else if (attr.chopCount == 2)
        {
            sprd.sprite = sprite2;
        }
        else
        {
            sprd.sprite = sprite0;
        }
    }

    public override void invalid()
    {
        valid = false;
        attr.hp = 0;
        sprd.sprite = sprite3;
    }
    private void Awake()
    {
        sprd = GetComponent<SpriteRenderer>();
        manager = GameObject.FindGameObjectWithTag("AreaManager").GetComponent<AreaManager>();
        fruits = new Hashtable();
    }

    public override void attacked(Weapon weaponType, float damage) {
        if (valid == false) return;
        switch (weaponType) {
            case Weapon.Katateken:
                attr.chopCount = attr.chopCount + 1;
                if(attr.chopCount == 1)
                {
                    sprd.sprite = sprite1;
                }
                else if(attr.chopCount == 2)
                {
                    sprd.sprite = sprite2;
                }
                else if (attr.chopCount == 3)
                {
                    invalid();
                }
                break;
            case Weapon.Boom:
                invalid();
                break;
            default:
                attr.hp = attr.hp - damage;
                if(attr.hp <= 0)
                {
                    invalid();
                }
                break;
        }

        if (valid == false)
        {
            foreach (string key in fruits.Keys)
            {
                ((GameObject)fruits[key]).GetComponent<Fruit>().drop();
            }
        }
    }

    public override void addChild(string id, GameObject child) {
        fruits.Add(id, child);
    }

    public override void removeChild(string id)
    {
        fruits.Remove(id);
    }
}

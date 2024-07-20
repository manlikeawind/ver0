using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Box : BaseObj
{
    [System.Serializable]
    public class Attr
    {
        public int hp;
    }

    private Attr attr;
    private SpriteRenderer sprd;
    private Hashtable store;

    public override void construct(string info)
    {
        attr = JsonUtility.FromJson<Attr>(info);
        if(attr.hp < 1)
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
        sprd.enabled = true;
    }
    public override void invalid()
    {
        valid = false;
        attr.hp = 0;
        sprd.enabled = false;
        foreach(string key in store.Keys) {
            GameObject obj = (GameObject)(store[key]);
            obj.SetActive(true);
            obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public override void attacked(Weapon weaponType, float damage)
    {
        if (valid == false) return;
        switch (weaponType)
        {
            case Weapon.Katateken:
                attr.hp = attr.hp - 1;
                break;
            case Weapon.Boom:
                attr.hp = 0;
                break;
            default:
                break;
        }
        if(attr.hp < 1)
        {
            this.invalid();
        }
    }
    private void Awake()
    {
        sprd = GetComponent<SpriteRenderer>();
        store = new Hashtable();
    }

    public override void addChild(string id, GameObject child)
    {
        store.Add(id, child);
        child.SetActive(false);
    }

    public override void removeChild(string id)
    {
        store.Remove(id);
    }
}

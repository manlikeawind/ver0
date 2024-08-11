using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : BaseObj
{
    [System.Serializable]
    public class Attr
    {
        public string parent;
        public int status;
        public float x;
        public float y;
    }

    private Attr attr;
    private AreaManager manager;
    private GameObject parent;
    private Rigidbody2D rgbd;
    public override void construct(string info)
    {
        attr = JsonUtility.FromJson<Attr>(info);
        if (attr.status > 0) {
            validate();
        }
        else
        {
            invalid();
        }
    }
    public override void validate()
    {
        /*valid = true;
        if (attr.status == 1)
        {
            rgbd.bodyType = RigidbodyType2D.Static;
            parent = manager.getAreaGameObject(attr.parent);
            this.transform.parent = parent.transform;
            this.transform.localPosition = new Vector2(attr.x, attr.y);
            parent.GetComponent<BaseObj>().addChild(oid, gameObject);
            if (parent.GetComponent<BaseObj>().valid == false)
            {
                invalid();
            }
        }
        else { 
            this.transform.position = new Vector2(attr.x, attr.y);
        }*/
    }
    public override void invalid()
    {
        valid = false;
        attr.status = 0;
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("AreaManager").GetComponent<AreaManager>();
        rgbd = GetComponent<Rigidbody2D>();
    }

    public void drop() {
        if (valid == false) return;
        attr.status = 2;
        rgbd.bodyType = RigidbodyType2D.Dynamic;
    }

    public override void pickUp()
    {
        Debug.Log(valid);
        if (valid == false) return;
        invalid();
    }
}

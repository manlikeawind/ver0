using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class Fire : MonoBehaviour
{
    private bool flag;
    private float deadTime;
    private float radius;

    // Start is called before the first frame update
    void Start()
    {
        deadTime = 0.0f;
        radius = 0.5f;
        flag = true;
    }

    private void FixedUpdate()
    {
        LayerMask objs = 1 << LayerMask.NameToLayer("Interact");
        Collider2D[] list1 = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), radius, objs);
        foreach (Collider2D collider in list1)
        {
            collider.gameObject.GetComponent<BaseObj>().fired();
        }

        if (flag == false)
        {
            deadTime += Time.fixedDeltaTime;
            if (deadTime > 0.5f) {
                Destroy(transform.gameObject);
            }
        }
    }

    public void outfire() {
        flag = false;
    }
}

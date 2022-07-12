using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class Fire : MonoBehaviour
{
    private GameObject mainObj;
    private float deadTime;
    private float radius1;
    private float radius2;

    // Start is called before the first frame update
    void Start()
    {
        deadTime = 0.0f;
        radius1 = 0.5f;
        radius2 = 2f;
    }

    private void FixedUpdate()
    {
        LayerMask objs = 1 << LayerMask.NameToLayer("Interact");
        Collider2D[] list1 = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), radius1, objs);
        foreach (Collider2D collider in list1)
        {
            collider.gameObject.GetComponent<BaseObj>().fired();
        }
        Collider2D[] list2 = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), radius2, objs);
        foreach (Collider2D collider in list2)
        {
            collider.gameObject.GetComponent<BaseObj>().heat();
        }

        if (mainObj == null)
        {
            deadTime += Time.fixedDeltaTime;
            if (deadTime > 0.5f) {
                Destroy(transform.gameObject);
            }
        }
    }

    public void setMainObj(GameObject obj) {
        mainObj = obj;
    }
}

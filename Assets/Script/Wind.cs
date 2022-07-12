using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    private GameObject main;
    private float width;
    private float height;
    private int dirX;
    private int dirY;
    private float deadTime;

    // Start is called before the first frame update
    void Start()
    {
        deadTime = 0f;
    }

    public void setMainObj(GameObject obj) {
        main = obj;
    }

    public void setParam(float posx, float posy, float w, float h, int dir_x, int dir_y) {
        transform.position = new Vector2(posx, posy);
        width = w;
        height = h;
        dirX = dir_x;
        dirY = dir_y;
        transform.localScale = new Vector3(width, height, 1f);
    }

    private void FixedUpdate()
    {
        LayerMask objs = 1 << LayerMask.NameToLayer("Interact");
        Collider2D[] list = Physics2D.OverlapBoxAll(transform.position, new Vector2(width, height), 0, objs);
        foreach (Collider2D collider in list)
        {
            collider.gameObject.GetComponent<BaseObj>().wind(dirX, dirY);
        }

        if (main == null)
        {
            deadTime += Time.fixedDeltaTime;
            if (deadTime > 2.0f)
            {
                Destroy(transform.gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float v_x;
    private float v_y;
    private float flyTime = -99999f;
    private Rigidbody2D rb2d;
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    public void initStatus(float vx, float vy, bool isDown) {
        v_x = vx;
        v_y = vy;
        flyTime = 0;
    }
    private void FixedUpdate()
    {
        flyTime += Time.fixedDeltaTime;
        if (flyTime > 0) {
            float y = v_y - flyTime * 3f;
            Vector3 v1 = new Vector3(1, 0, 0);
            Vector3 v2 = new Vector3(v_x, y, 0);
            float dotResult = Vector3.Dot(v1, v2.normalized);
            float radius = Mathf.Acos(dotResult) / Mathf.Deg2Rad;
            if(y < 0)
            {
                radius = -radius;
            }
            transform.localRotation = Quaternion.Euler(0, 0, radius);
            rb2d.velocity = new Vector2(v_x, y);
        }
    }
}

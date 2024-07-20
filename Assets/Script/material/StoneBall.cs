using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBall : MonoBehaviour
{
    private float v_x;
    private float v_y;
    public float flyTime= -99999f;
    private Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void initStatus(float vx, float vy)
    {
        v_x = vx;
        if(v_x < 0)
        {
            transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        v_y = vy;
        flyTime = 0;
    }

    private void FixedUpdate()
    {
        flyTime += Time.fixedDeltaTime;
        if (flyTime > 0)
        {
            float radius = -57.3248408f * v_x * flyTime;
            transform.localRotation = Quaternion.Euler(0, 0, radius);
            rb2d.velocity = new Vector2(v_x, rb2d.velocity.y);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalBall : BaseObj
{
    private Rigidbody2D rb2d;
    private float radius;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        radius = 0.5f;
    }

    private void FixedUpdate()
    {
        if (rb2d.velocity.sqrMagnitude > 4f) {
            LayerMask player = 1 << LayerMask.NameToLayer("Player");
            Collider2D[] list1 = Physics2D.OverlapCircleAll(transform.position, radius, player);
        }
    }
}

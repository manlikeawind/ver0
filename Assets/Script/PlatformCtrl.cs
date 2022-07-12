using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCtrl : MonoBehaviour
{
    private PlatformEffector2D effector;
    private BoxCollider2D boxCollider2D;
    private Vector2 point;
    private Vector2 size;
    private float time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        point = new Vector2(transform.position.x+boxCollider2D.offset.x, transform.position.y+boxCollider2D.offset.y);
        size = new Vector3(boxCollider2D.size.x, boxCollider2D.size.y-0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        if (effector.rotationalOffset > 90f) {
            time += Time.deltaTime;
            LayerMask playerLayer = 1 << LayerMask.NameToLayer("Player");
            bool isoverlap = Physics2D.OverlapBox(point, size, 0, playerLayer);
            if (!isoverlap && time > 1f) {
                effector.rotationalOffset = 0;
            }
        }
        else
        {
            time = 0.0f;
        }
    }
}

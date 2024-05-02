using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Fire_test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    private void FixedUpdate()
    {
        var gp = Gamepad.current;
        if (gp == null) return;
        float btn_y = gp.buttonNorth.ReadValue();
        if (btn_y > 0.5) {
            LayerMask objs = 1 << LayerMask.NameToLayer("Interact");
            Collider2D[] list1 = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y-1.5f), 0.5f, objs);
            foreach (Collider2D collider in list1)
            {
                collider.gameObject.GetComponent<BaseObj>().fired();
            }
        }
    }
}

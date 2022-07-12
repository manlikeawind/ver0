using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Wind_test : MonoBehaviour
{
    private GameObject hero;
    // Start is called before the first frame update
    void Start()
    {
        hero = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        var gp = Gamepad.current;
        if (gp == null) return;
        float btn_y = gp.buttonNorth.ReadValue();
        if (btn_y > 0.5)
        {
            LayerMask objs = 1 << LayerMask.NameToLayer("Interact");
            if (hero.GetComponent<PlayerCtrl>().faceTo > 0f)
            {
                Collider2D[] list = Physics2D.OverlapBoxAll(new Vector2(transform.position.x+2.5f, transform.position.y-0.5f), new Vector2(5, 2), 0, objs);
                foreach (Collider2D collider in list)
                {
                    collider.gameObject.GetComponent<BaseObj>().wind(1f, 0);
                }
            }
            else {
                Collider2D[] list = Physics2D.OverlapBoxAll(new Vector2(transform.position.x-2.5f, transform.position.y-0.5f), new Vector2(5, 2), 0, objs);
                foreach (Collider2D collider in list)
                {
                    collider.gameObject.GetComponent<BaseObj>().wind(-1f, 0);
                }
            }
        }
    }
}

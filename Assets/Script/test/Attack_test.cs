using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Attack_test : MonoBehaviour
{
    private float attackCD;
    private GameObject hero;
    // Start is called before the first frame update
    void Start()
    {
        attackCD = 0f;
        hero = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        attackCD += Time.fixedDeltaTime;
        var gp = Gamepad.current;
        if (gp == null) return;
        float btn_y = gp.buttonNorth.ReadValue();
        if (btn_y > 0.5) {
            LayerMask objs = 1 << LayerMask.NameToLayer("Interact");
            if (hero.GetComponent<PlayerCtrl>().faceTo > 0f)
            {
                Collider2D[] list = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + 0.5f, transform.position.y - 1f), new Vector2(1, 1), 0, objs);
                foreach (Collider2D collider in list)
                {
                    collider.gameObject.GetComponent<BaseObj>().attacked(Common.Weapon.Katateken, 20f);
                }
            }
            else
            {
                Collider2D[] list = Physics2D.OverlapBoxAll(new Vector2(transform.position.x - 0.5f, transform.position.y - 1f), new Vector2(1, 1), 0, objs);
                foreach (Collider2D collider in list)
                {
                    collider.gameObject.GetComponent<BaseObj>().attacked(Common.Weapon.Katateken, 20f);
                }
            }
        }
    }
}

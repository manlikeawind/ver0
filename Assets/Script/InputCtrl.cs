using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputCtrl : MonoBehaviour
{
    private static InputCtrl _instance;

    public enum Button { 
        None, a_up, a_down, b_up, b_down, x_up, x_down, y_up, y_down, start_up, start_down
    }

    struct Event{ 
        public Button button;
        public float time;
    }

    private Queue<Event> events;

    //type: 0-gamepad; 1-keyboard
    private int type;

    private float dir_x;
    private float dir_y;
    private float btn_a;
    private float btn_b;
    private float btn_x;
    private float btn_y;
    private float shd_l;
    private float shd_r;
    private float stk_x;
    private float stk_y;
    private float crs_x;
    private float crs_y;
    private float tri_l;
    private float tri_r;
    private float btn_ls;
    private float btn_rs;

    private float btn_start;
    private float btn_select;

    public static InputCtrl Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        type = 0;
        events = new Queue<Event>();
    }

    // Update is called once per frame
    private void Update()
    {
        float now = Time.realtimeSinceStartup;

        int insure = 100;
        while (events.Count > 0) {
            float t = events.Peek().time;
            if (t < now - 0.2f)
            {
                events.Dequeue();
            }
            else
            {
                break;
            }
            insure--;
            if(insure < 0) { break; }
        }

        if (type == 0)
        {
            var gp = Gamepad.current;
            if (gp == null) return;

            if (gp.buttonSouth.ReadValue() - btn_a > 0.5)
            {
                Event e;
                e.button = Button.a_down;
                e.time = now;
                events.Enqueue(e);
            }
            else if (gp.buttonSouth.ReadValue() - btn_a < -0.5)
            {
                Event e;
                e.button = Button.a_up;
                e.time = now;
                events.Enqueue(e);
            }

            if (gp.buttonWest.ReadValue() - btn_x > 0.5)
            {
                Event e;
                e.button = Button.x_down;
                e.time = now;
                events.Enqueue(e);
            }
            else if (gp.buttonWest.ReadValue() - btn_x < -0.5)
            {
                Event e;
                e.button = Button.x_up;
                e.time = now;
                events.Enqueue(e);
            }

            if (gp.startButton.ReadValue() - btn_start > 0.5)
            {
                Event e;
                e.button = Button.start_down;
                e.time = now;
                events.Enqueue(e);
            }
            else if (gp.startButton.ReadValue() - btn_start < -0.5)
            {
                Event e;
                e.button = Button.start_up;
                e.time = now;
                events.Enqueue(e);
            }

            dir_x = gp.leftStick.ReadValue().x;
            dir_y = gp.leftStick.ReadValue().y;
            btn_a = gp.buttonSouth.ReadValue();
            btn_b = gp.buttonEast.ReadValue();
            btn_x = gp.buttonWest.ReadValue();
            btn_y = gp.buttonNorth.ReadValue();
            shd_l = gp.leftShoulder.ReadValue();
            shd_r = gp.rightShoulder.ReadValue();
            stk_x = gp.rightStick.ReadValue().x;
            stk_y = gp.rightStick.ReadValue().y;
            crs_x = gp.dpad.ReadValue().x;
            crs_y = gp.dpad.ReadValue().y;
            tri_l = gp.leftTrigger.ReadValue();
            tri_r = gp.rightTrigger.ReadValue();
            btn_ls = gp.leftStickButton.ReadValue();
            btn_rs = gp.rightStickButton.ReadValue();
            btn_select = gp.selectButton.ReadValue();
            btn_start = gp.startButton.ReadValue();
        }
        else {
            if (Input.GetKey(KeyCode.A))
            {
                dir_x = -1.0f;
            }else if (Input.GetKey(KeyCode.D))
            {
                dir_x = 1.0f;
            }
            else
            {
                dir_x = 0.0f;
            }

            if(Input.GetKey(KeyCode.Space)) {
                btn_a = 1.0f;
            }
            else
            {
                btn_a = 0f;
            }
        }
        
    }

    public float getDirX() { return dir_x; }
    public float getDirY() { return dir_y; }
    public float getBtnA() { return btn_a; }
    public float getBtnB() { return btn_b; }
    public float getBtnX() { return btn_x; }
    public float getBtnY() { return btn_y; }
    public float getShdL() { return shd_l; }
    public float getShdR() { return shd_r; }
    public float getStkX() { return stk_x; }
    public float getStkY() { return stk_y; }
    public float getCrsX() { return crs_x; }
    public float getCrsY() { return crs_y; }
    public float getBtnStart() { return btn_start; }

    public bool consumeBtnAUp(float limit) {
        float now = Time.realtimeSinceStartup;
        bool ret = false;
        foreach(Event e in events)
        {
            if (e.button == Button.a_up && now - e.time < limit) {
                ret = true;
            }
        }
        if (ret)
        {
            events.Clear();
        }
        return ret;
    }

    public bool consumeBtnXDown(float limit)
    {
        float now = Time.realtimeSinceStartup;
        bool ret = false;
        foreach (Event e in events)
        {
            if (e.button == Button.x_down && now - e.time < limit)
            {
                ret = true;
            }
        }
        if (ret)
        {
            events.Clear();
        }
        return ret;
    }

    public bool consumeBtnStartDown(float limit) {
        float now = Time.realtimeSinceStartup;
        bool ret = false;
        foreach (Event e in events)
        {
            if (e.button == Button.start_down && now - e.time < limit)
            {
                ret = true;
            }
        }
        if (ret)
        {
            events.Clear();
        }
        return ret;
    }
}

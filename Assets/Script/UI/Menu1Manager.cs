using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu1Manager : MonoBehaviour
{
    public enum Content
    {
        None,
        Weapon, Clothing, Use, Etc, Spec
    }

    private UIManager uiManager;
    private InputCtrl input;
    private Content cont;
    private bool onCheckBox;

    public GameObject weaponBag;

    // Start is called before the first frame update
    void Start()
    {
        cont = Content.Weapon;
        onCheckBox = false;
        input = InputCtrl.Instance;
        uiManager = UIManager.Instance;
        uiManager.registerMenu1Ctrl(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(input.getBtnB() > 0.5f)
        {
            if(onCheckBox == false)
            {
                uiManager.exitUI();
            }
            else
            {

            }
        }
    }

    public void changeWeaponBag(int start, int end, JObject data) {
        int volume = (int)data["volume"];
        JArray items = (JArray)data["items"];

    }
}

using Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    private GameManager gameManager;
    
    public GameObject mask;
    public GameObject menu0;
    public GameObject menu1;
    public GameObject shotCut;
    public GameObject baseUI;

    private BasePage basePageCtrl;
    private Menu1Manager menu1Ctrl;

    public UIStatus status;
    public static UIManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        status = UIStatus.Common;
        gameManager = GameManager.Instance;
    }

    public void enterUI(UIStatus s) {
        switch (s)
        {
            case UIStatus.Menu0:
                menu0.SetActive(true);
                break;
            case UIStatus.Menu1:
                menu1.SetActive(true);
                break;
            case UIStatus.Shotcut:
                shotCut.SetActive(true);
                break;
        }
        baseUI.SetActive(false);
        status = s;
    }

    public void exitUI()
    {
        switch (status)
        {
            case UIStatus.Menu0:
                menu0.SetActive(false);
                break;
            case UIStatus.Menu1:
                menu1.SetActive(false);
                break;
            case UIStatus.Shotcut:
                shotCut.SetActive(false);
                break;
        }
        baseUI.SetActive(true);
        status = UIStatus.Common;
    }

    public Sprite getIcon(UIIconType iconType, string code) {
        int index = 0;
        int.TryParse(code, out index);
        Sprite r = null;
        switch (iconType)
        {
            case UIIconType.Blank:
                r = gameManager.resource.getBlankPng();
                break;
            case UIIconType.Ability:
                r = gameManager.resource.getAbilityIcon(index);
                break;
            case UIIconType.Item:
                r = gameManager.resource.getItemIcon(index);
                break;
        }
        return r;
    }

    public void registerBasePageCtrl(BasePage basePage)
    {
        basePageCtrl = basePage;
    }

    public void registerMenu1Ctrl(Menu1Manager menu1)
    {
        menu1Ctrl = menu1;
    }

    public void initUI() {
        JObject data = (JObject)gameManager.gameInfo["data"];
        float health = (float)data["player"]["status"]["health"];
        float shield = (float)data["player"]["status"]["shield"];
        basePageCtrl.changeHealth(health, shield);

        float endurence = (float)data["player"]["status"]["endurence"];
        basePageCtrl.changeEndurence(endurence);

        menu1Ctrl.setBagInfo((JObject)data["bag"]);
        int weaponVolume = (int)data["bag"]["weapon"]["volume"];
        menu1Ctrl.changeWeaponBag(0, weaponVolume);
        int clothingVolume = (int)data["bag"]["clothing"]["volume"];
        menu1Ctrl.changeClothingBag(0, clothingVolume);
        int foodAndDrugVolume = (int)data["bag"]["food&drug"]["volume"];
        menu1Ctrl.changeFoodAndDrugBag(0, foodAndDrugVolume);
        int metarialVolume = (int)data["bag"]["metarial"]["volume"];
        menu1Ctrl.changeMetarialBag(0, metarialVolume);
        int specialVolume = (int)data["bag"]["special"]["volume"];
        menu1Ctrl.changeSpecialBag(0, specialVolume);
    }

    public void changeUI(UIDataType dataType, JToken data) {
        switch(dataType)
        {
            case UIDataType.Health:
                float health = (float)data["health"];
                float shield = (float)data["shield"];
                basePageCtrl.changeHealth(health, shield);
                break;
            case UIDataType.Endurence:
                float endurence = (float)data["v"];
                basePageCtrl.changeEndurence(endurence);
                break;
            case UIDataType.Shotcut0:
                string index0 = (string)data["v"];
                basePageCtrl.changeShotcut0(index0);
                break;
            case UIDataType.Shotcut1:
                string index1 = (string)data["v"];
                basePageCtrl.changeShotcut1(index1);
                break;
            case UIDataType.Shotcut2:
                string index2 = (string)data["v"];
                basePageCtrl.changeShotcut2(index2);
                break;
            case UIDataType.Shotcut3:
                string index3 = (string)data["v"];
                basePageCtrl.changeShotcut3(index3);
                break;
            case UIDataType.Time:
                string time = (string)data["v"];
                basePageCtrl.changeTime(time);
                break;
            case UIDataType.Temperature:
                int temperature = (int)data["v"];
                basePageCtrl.changeTemprature(temperature);
                break;
            case UIDataType.Detector:
                int detector = (int)data["v"];
                basePageCtrl.changeDetector(detector);
                break;
        }
    }
}

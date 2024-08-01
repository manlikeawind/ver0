using Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu1Manager : MonoBehaviour
{
    public const int col = 5;
    public enum Content
    {
        None,
        Weapon, Clothing, Use, Etc, Spec
    }

    private UIManager uiManager;
    private InputCtrl input;
    private Content cont;
    private bool onCheckBox;
    private JObject bagInfo;

    public GameObject itemPrafab;

    public GameObject weaponTab;
    public GameObject clothingTab;
    public GameObject foodAndDrugTab;
    public GameObject metarialTab;
    public GameObject specialTab;
    public GameObject settingTab;
    public GameObject weaponRect;
    public GameObject clothingRect;
    public GameObject foodAndDrugRect;
    public GameObject metarialRect;
    public GameObject specialRect;
    public GameObject weaponBag;
    public GameObject clothingBag;
    public GameObject foodAndDrugBag;
    public GameObject metarialBag;
    public GameObject specialBag;
    public GameObject checkBox;

    private int weaponIndex;
    private int clothingIndex;
    private int foodAndDrugIndex;
    private int metarialIndex;
    private int specialIndex;

    private float sleepTime;

    private void Start()
    {
        input = InputCtrl.Instance;
        uiManager = UIManager.Instance;
        uiManager.registerMenu1Ctrl(this);
        cont = Content.Weapon;
        onCheckBox = false;
        weaponIndex = 0;
        clothingIndex = 0;
        foodAndDrugIndex = 0;
        metarialIndex = 0;
        specialIndex = 0;
        sleepTime = 0f;
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        sleepTime = sleepTime + Time.unscaledDeltaTime;
        if(sleepTime > 0.3f){
            if(input.getBtnB() > 0.5f)
            {
                if(onCheckBox == false)
                {
                    uiManager.exitUI();
                }
                else
                {
                    checkBox.SetActive(false);
                    sleepTime = 0f;
                }
            }else if(input.getShdL() > 0.5f){
                switch (cont)
                {
                    case Content.Weapon:
                        break;
                    case Content.Clothing:
                        clothingRect.SetActive(false);
                        weaponRect.SetActive(true);
                        cont = Content.Weapon;
                        break;
                    case Content.Use:
                        foodAndDrugRect.SetActive(false);
                        clothingRect.SetActive(true);
                        cont = Content.Clothing;
                        break;
                    case Content.Etc:
                        metarialRect.SetActive(false);
                        foodAndDrugRect.SetActive(true);
                        cont = Content.Use;
                        break;
                    case Content.Spec:
                        specialRect.SetActive(false);
                        metarialRect.SetActive(true);
                        cont = Content.Etc;
                        break;
                }
                sleepTime = 0f;
            }
            else if (input.getShdR() > 0.5f)
            {
                switch (cont)
                {
                    case Content.Weapon:
                        clothingRect.SetActive(true);
                        weaponRect.SetActive(false);
                        cont = Content.Clothing;
                        break;
                    case Content.Clothing:
                        foodAndDrugRect.SetActive(true);
                        clothingRect.SetActive(false);
                        cont = Content.Use;
                        break;
                    case Content.Use:
                        metarialRect.SetActive(true);
                        foodAndDrugRect.SetActive(false);
                        cont = Content.Etc;
                        break;
                    case Content.Etc:
                        specialRect.SetActive(true);
                        metarialRect.SetActive(false);
                        cont = Content.Spec;
                        break;
                    case Content.Spec:
                        break;
                }
                sleepTime = 0f;
            }
            else if(input.getCrsY() > 0.5f){
                switch (cont){
                    case Content.Weapon:
                        if(weaponIndex >= col){
                            changeActiveItem(weaponIndex, weaponIndex - col);
                            weaponIndex = weaponIndex - col;
                        }
                        break;
                    case Content.Clothing:
                        if(clothingIndex >= col){
                            changeActiveItem(clothingIndex, clothingIndex - col);
                            clothingIndex = clothingIndex - col;
                        }
                        break;
                    case Content.Use:
                        if(foodAndDrugIndex >= col){
                            changeActiveItem(foodAndDrugIndex, foodAndDrugIndex - col);
                            foodAndDrugIndex = foodAndDrugIndex - col;
                        }
                        break;
                    case Content.Etc:
                        if(metarialIndex >= col){
                            changeActiveItem(metarialIndex, metarialIndex - col);
                            metarialIndex = metarialIndex - col;
                        }
                        break;
                    case Content.Spec:
                        if(specialIndex >= col){
                            changeActiveItem(specialIndex, specialIndex - col);
                            specialIndex = specialIndex - col;
                        }
                        break;
                }
                sleepTime = 0f;
            }
            else if(input.getCrsY() < -0.5f){
                int volume = 0;
                switch (cont){
                    case Content.Weapon:
                        volume = (int)bagInfo["weapon"]["volume"];
                        if(weaponIndex + col < volume){
                            changeActiveItem(weaponIndex, weaponIndex + col);
                            weaponIndex = weaponIndex + col;
                        }
                        break;
                    case Content.Clothing:
                        volume = (int)bagInfo["clothing"]["volume"];
                        if(clothingIndex + col < volume){
                            changeActiveItem(clothingIndex, clothingIndex + col);
                            clothingIndex = clothingIndex + col;
                        }
                        break;
                    case Content.Use:
                        volume = (int)bagInfo["food&drug"]["volume"];
                        if(foodAndDrugIndex + col < volume){
                            changeActiveItem(foodAndDrugIndex, foodAndDrugIndex + col);
                            foodAndDrugIndex = foodAndDrugIndex + col;
                        }
                        break;
                    case Content.Etc:
                        volume = (int)bagInfo["metarial"]["volume"];
                        if(metarialIndex + col < volume){
                            changeActiveItem(metarialIndex, metarialIndex + col);
                            metarialIndex = metarialIndex + col;
                        }
                        break;
                    case Content.Spec:
                        volume = (int)bagInfo["special"]["volume"];
                        if(specialIndex + col < volume){
                            changeActiveItem(specialIndex, specialIndex + col);
                            specialIndex = specialIndex + col;
                        }
                        break;
                }
                sleepTime = 0f;
            }
            else if(input.getCrsX() > 0.5f){
                int r = col;
                switch (cont){
                    case Content.Weapon:
                        r = weaponIndex % col;
                        if(r < col - 1){
                            changeActiveItem(weaponIndex, weaponIndex + 1);
                            weaponIndex++;
                        }
                        break;
                    case Content.Clothing:
                        r = clothingIndex % col;
                        if(r < col - 1){
                            changeActiveItem(clothingIndex, clothingIndex + 1);
                            clothingIndex++;
                        }
                        break;
                    case Content.Use:
                        r = foodAndDrugIndex % col;
                        if(r < col - 1){
                            changeActiveItem(foodAndDrugIndex, foodAndDrugIndex + 1);
                            foodAndDrugIndex++;
                        }
                        break;
                    case Content.Etc:
                        r = metarialIndex % col;
                        if(r < col - 1){
                            changeActiveItem(metarialIndex, metarialIndex + 1);
                            metarialIndex++;
                        }
                        break;
                    case Content.Spec:
                        r = specialIndex % col;
                        if(r < col - 1){
                            changeActiveItem(specialIndex, specialIndex + 1);
                            specialIndex++;
                        }
                        break;
                }
                sleepTime = 0f;
            }
            else if(input.getCrsX() < -0.5f){
                switch (cont){
                    case Content.Weapon:
                        if(weaponIndex > 0){
                            changeActiveItem(weaponIndex, weaponIndex - 1);
                            weaponIndex--;
                        }
                        break;
                    case Content.Clothing:
                        if(clothingIndex > 0){
                            changeActiveItem(clothingIndex, clothingIndex - 1);
                            clothingIndex--;
                        }
                        break;
                    case Content.Use:
                        if(foodAndDrugIndex > 0){
                            changeActiveItem(foodAndDrugIndex, foodAndDrugIndex - 1);
                            foodAndDrugIndex--;
                        }
                        break;
                    case Content.Etc:
                        if(metarialIndex > 0){
                            changeActiveItem(metarialIndex, metarialIndex - 1);
                            metarialIndex--;
                        }
                        break;
                    case Content.Spec:
                        if(specialIndex > 0){
                            changeActiveItem(specialIndex, specialIndex - 1);
                            specialIndex--;
                        }
                        break;
                }
                sleepTime = 0f;
            }
        }
    }

    public void setBagInfo(JObject data){
        bagInfo = data;
    }

    public void changeActiveItem(int from, int to){
        
    }

    public void changeWeaponBag(int start, int end) {
        int volume = (int)bagInfo["weapon"]["volume"];
        JArray items = (JArray)bagInfo["weapon"]["items"];
        if(weaponBag.transform.childCount < end){
            for(int i = start; i < weaponBag.transform.childCount; i++){
                updateItemInfo(weaponBag.transform.GetChild(i), (JObject)items[i]);
            }
            for(int i = weaponBag.transform.childCount; i < end; i++){
                GameObject obj = Instantiate(itemPrafab);
                obj.name = i.ToString();
                obj.transform.SetParent(weaponBag.transform);
                obj.transform.localScale = new Vector3(1, 1, 1);
                updateItemInfo(weaponBag.transform.GetChild(i), (JObject)items[i]);
            }
        }else{
            for(int i = start; i < end; i++){
                updateItemInfo(weaponBag.transform.GetChild(i), (JObject)items[i]);
            }
        }
    }

    public void changeClothingBag(int start, int end) {
        int volume = (int)bagInfo["clothing"]["volume"];
        JArray items = (JArray)bagInfo["clothing"]["items"];
        if (clothingBag.transform.childCount < end)
        {
            for (int i = start; i < clothingBag.transform.childCount; i++)
            {
                updateItemInfo(clothingBag.transform.GetChild(i), (JObject)items[i]);
            }
            for (int i = clothingBag.transform.childCount; i < end; i++)
            {
                GameObject obj = Instantiate(itemPrafab);
                obj.name = i.ToString();
                obj.transform.SetParent(clothingBag.transform);
                obj.transform.localScale = new Vector3(1, 1, 1);
                updateItemInfo(clothingBag.transform.GetChild(i), (JObject)items[i]);
            }
        }
        else
        {
            for (int i = start; i < end; i++)
            {
                updateItemInfo(clothingBag.transform.GetChild(i), (JObject)items[i]);
            }
        }
    }

    public void changeFoodAndDrugBag(int start, int end) {
        int volume = (int)bagInfo["food&drug"]["volume"];
        JArray items = (JArray)bagInfo["food&drug"]["items"];
        if (foodAndDrugBag.transform.childCount < end)
        {
            for (int i = start; i < foodAndDrugBag.transform.childCount; i++)
            {
                updateItemInfo(foodAndDrugBag.transform.GetChild(i), (JObject)items[i]);
            }
            for (int i = foodAndDrugBag.transform.childCount; i < end; i++)
            {
                GameObject obj = Instantiate(itemPrafab);
                obj.name = i.ToString();
                obj.transform.SetParent(foodAndDrugBag.transform);
                obj.transform.localScale = new Vector3(1, 1, 1);
                updateItemInfo(foodAndDrugBag.transform.GetChild(i), (JObject)items[i]);
            }
        }
        else
        {
            for (int i = start; i < end; i++)
            {
                updateItemInfo(foodAndDrugBag.transform.GetChild(i), (JObject)items[i]);
            }
        }
    }

    public void changeMetarialBag(int start, int end) {
        int volume = (int)bagInfo["metarial"]["volume"];
        JArray items = (JArray)bagInfo["metarial"]["items"];
        if (metarialBag.transform.childCount < end)
        {
            for (int i = start; i < metarialBag.transform.childCount; i++)
            {
                updateItemInfo(metarialBag.transform.GetChild(i), (JObject)items[i]);
            }
            for (int i = metarialBag.transform.childCount; i < end; i++)
            {
                GameObject obj = Instantiate(itemPrafab);
                obj.name = i.ToString();
                obj.transform.SetParent(metarialBag.transform);
                obj.transform.localScale = new Vector3(1, 1, 1);
                updateItemInfo(metarialBag.transform.GetChild(i), (JObject)items[i]);
            }
        }
        else
        {
            for (int i = start; i < end; i++)
            {
                updateItemInfo(metarialBag.transform.GetChild(i), (JObject)items[i]);
            }
        }
    }

    public void changeSpecialBag(int start, int end) {
        int volume = (int)bagInfo["special"]["volume"];
        JArray items = (JArray)bagInfo["special"]["items"];
        if (specialBag.transform.childCount < end)
        {
            for (int i = start; i < specialBag.transform.childCount; i++)
            {
                updateItemInfo(specialBag.transform.GetChild(i), (JObject)items[i]);
            }
            for (int i = specialBag.transform.childCount; i < end; i++)
            {
                GameObject obj = Instantiate(itemPrafab);
                obj.name = i.ToString();
                obj.transform.SetParent(specialBag.transform);
                obj.transform.localScale = new Vector3(1, 1, 1);
                updateItemInfo(specialBag.transform.GetChild(i), (JObject)items[i]);
            }
        }
        else
        {
            for (int i = start; i < end; i++)
            {
                updateItemInfo(specialBag.transform.GetChild(i), (JObject)items[i]);
            }
        }
    }

    public void updateItemInfo(Transform item, JObject data)
    {
        string code = (string)data["code"];
        if (!code.Equals("0000"))
        {
            Sprite icon = uiManager.getIcon(UIIconType.Item, code);
            item.Find("Icon").GetComponent<Image>().sprite = icon;
            int count = (int)data["count"];
            if (count > 1)
            {
                item.Find("Count").GetComponent<TextMeshProUGUI>().text = "X" + count;
            }
        }
        else
        {
            Sprite icon = uiManager.getIcon(UIIconType.Blank, code);
            item.Find("Icon").GetComponent<Image>().sprite = icon;
        }
    }

    public void showItemInfo(JObject item){

    }
}

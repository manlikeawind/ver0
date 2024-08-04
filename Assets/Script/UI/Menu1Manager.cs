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
    public const int rowInSight = 5;
    public enum Content
    {
        None,
        Weapon, Clothing, Use, Etc, Spec, Option
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
    public GameObject optionTab;
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
    public GameObject content1;
    public GameObject content2;
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
        if(sleepTime > 0.2f){
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
                        clothingTab.transform.localScale = new Vector3(1f, 1f, 1f);
                        weaponTab.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                        clothingRect.SetActive(false);
                        weaponRect.SetActive(true);
                        cont = Content.Weapon;
                        break;
                    case Content.Use:
                        foodAndDrugTab.transform.localScale = new Vector3(1f, 1f, 1f);
                        clothingTab.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                        foodAndDrugRect.SetActive(false);
                        clothingRect.SetActive(true);
                        cont = Content.Clothing;
                        break;
                    case Content.Etc:
                        metarialTab.transform.localScale = new Vector3(1f, 1f, 1f);
                        foodAndDrugTab.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                        metarialRect.SetActive(false);
                        foodAndDrugRect.SetActive(true);
                        cont = Content.Use;
                        break;
                    case Content.Spec:
                        specialTab.transform.localScale = new Vector3(1f, 1f, 1f);
                        metarialTab.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                        specialRect.SetActive(false);
                        metarialRect.SetActive(true);
                        cont = Content.Etc;
                        break;
                    case Content.Option:
                        optionTab.transform.localScale = new Vector3(1f, 1f, 1f);
                        specialTab.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                        content1.SetActive(true);
                        content2.SetActive(false);
                        cont = Content.Spec;
                        break;
                }
                sleepTime = 0f;
            }
            else if (input.getShdR() > 0.5f)
            {
                switch (cont)
                {
                    case Content.Weapon:
                        weaponTab.transform.localScale = new Vector3(1f, 1f, 1f);
                        clothingTab.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                        clothingRect.SetActive(true);
                        weaponRect.SetActive(false);
                        cont = Content.Clothing;
                        break;
                    case Content.Clothing:
                        clothingTab.transform.localScale = new Vector3(1f, 1f, 1f);
                        foodAndDrugTab.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                        foodAndDrugRect.SetActive(true);
                        clothingRect.SetActive(false);
                        cont = Content.Use;
                        break;
                    case Content.Use:
                        foodAndDrugTab.transform.localScale = new Vector3(1f, 1f, 1f);
                        metarialTab.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                        metarialRect.SetActive(true);
                        foodAndDrugRect.SetActive(false);
                        cont = Content.Etc;
                        break;
                    case Content.Etc:
                        metarialTab.transform.localScale = new Vector3(1f, 1f, 1f);
                        specialTab.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                        specialRect.SetActive(true);
                        metarialRect.SetActive(false);
                        cont = Content.Spec;
                        break;
                    case Content.Spec:
                        specialTab.transform.localScale = new Vector3(1f, 1f, 1f);
                        optionTab.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                        content2.SetActive(true);
                        content1.SetActive(false);
                        cont = Content.Option;
                        break;
                    case Content.Option:
                        break;
                }
                sleepTime = 0f;
            }
            else if (input.getCrsY() > 0.5f)
            {
                int y;
                float z;
                int volume;
                int nowRow;
                float newR;
                switch (cont)
                {
                    case Content.Weapon:
                        if (weaponIndex >= col)
                        {
                            changeActiveItem(weaponBag, weaponIndex, weaponIndex - col);
                            weaponIndex = weaponIndex - col;
                            volume = (int)bagInfo["weapon"]["volume"];
                            y = volume / col;
                            if (volume % col != 0)
                            {
                                y++;
                            }
                            z = (y - rowInSight) * (1f - weaponRect.GetComponent<ScrollRect>().normalizedPosition.y);
                            nowRow = weaponIndex / col;
                            if (weaponIndex % col != 0)
                            {
                                nowRow++;
                            }
                            if (z > (float)nowRow)
                            {
                                newR = (float)nowRow / (float)(y - rowInSight);
                                weaponRect.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1f - newR);
                            }
                        }
                        break;
                    case Content.Clothing:
                        if (clothingIndex >= col)
                        {
                            changeActiveItem(clothingBag, clothingIndex, clothingIndex - col);
                            clothingIndex = clothingIndex - col;
                            volume = (int)bagInfo["clothing"]["volume"];
                            y = volume / col;
                            if (volume % col != 0)
                            {
                                y++;
                            }
                            z = (y - rowInSight) * (1f - clothingRect.GetComponent<ScrollRect>().normalizedPosition.y);
                            nowRow = clothingIndex / col;
                            if (clothingIndex % col != 0)
                            {
                                nowRow++;
                            }
                            if (z > (float)nowRow)
                            {
                                newR = (float)nowRow / (float)(y - rowInSight);
                                clothingRect.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1f - newR);
                            }
                        }
                        break;
                    case Content.Use:
                        if (foodAndDrugIndex >= col)
                        {
                            changeActiveItem(foodAndDrugBag, foodAndDrugIndex, foodAndDrugIndex - col);
                            foodAndDrugIndex = foodAndDrugIndex - col;
                            volume = (int)bagInfo["food&drug"]["volume"];
                            y = volume / col;
                            if (volume % col != 0)
                            {
                                y++;
                            }
                            z = (y - rowInSight) * (1f - foodAndDrugRect.GetComponent<ScrollRect>().normalizedPosition.y);
                            nowRow = foodAndDrugIndex / col;
                            if (foodAndDrugIndex % col != 0)
                            {
                                nowRow++;
                            }
                            if (z > (float)nowRow)
                            {
                                newR = (float)nowRow / (float)(y - rowInSight);
                                foodAndDrugRect.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1f - newR);
                            }
                        }
                        break;
                    case Content.Etc:
                        if (metarialIndex >= col)
                        {
                            changeActiveItem(metarialBag, metarialIndex, metarialIndex - col);
                            metarialIndex = metarialIndex - col;
                            volume = (int)bagInfo["metarial"]["volume"];
                            y = volume / col;
                            if (volume % col != 0)
                            {
                                y++;
                            }
                            z = (y - rowInSight) * (1f - metarialRect.GetComponent<ScrollRect>().normalizedPosition.y);
                            nowRow = metarialIndex / col;
                            if (metarialIndex % col != 0)
                            {
                                nowRow++;
                            }
                            if (z > (float)nowRow)
                            {
                                newR = (float)nowRow / (float)(y - rowInSight);
                                metarialRect.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1f - newR);
                            }
                        }
                        break;
                    case Content.Spec:
                        if (specialIndex >= col)
                        {
                            changeActiveItem(specialBag, specialIndex, specialIndex - col);
                            specialIndex = specialIndex - col;
                            volume = (int)bagInfo["special"]["volume"];
                            y = volume / col;
                            if (volume % col != 0)
                            {
                                y++;
                            }
                            z = (y - rowInSight) * (1f - specialRect.GetComponent<ScrollRect>().normalizedPosition.y);
                            nowRow = specialIndex / col;
                            if (specialIndex % col != 0)
                            {
                                nowRow++;
                            }
                            if (z > (float)nowRow)
                            {
                                newR = (float)nowRow / (float)(y - rowInSight);
                                specialRect.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1f - newR);
                            }
                        }
                        break;
                }
                sleepTime = 0f;
            }
            else if (input.getCrsY() < -0.5f)
            {
                int y;
                float z;
                int volume;
                int nowRow;
                float newR = 0f;
                switch (cont)
                {
                    case Content.Weapon:
                        volume = (int)bagInfo["weapon"]["volume"];
                        if (weaponIndex + col < volume)
                        {
                            changeActiveItem(weaponBag, weaponIndex, weaponIndex + col);
                            weaponIndex = weaponIndex + col;
                            y = volume / col;
                            if (volume % col != 0)
                            {
                                y++;
                            }
                            z = (y - rowInSight) * (1 - weaponRect.GetComponent<ScrollRect>().normalizedPosition.y);
                            nowRow = weaponIndex / col + 1;
                            if (weaponIndex % col != 0)
                            {
                                nowRow++;
                            }
                            if (z + rowInSight < (float)nowRow)
                            {
                                newR = (float)(nowRow - rowInSight) / (float)(y - rowInSight);
                                weaponRect.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1f - newR);
                            }
                        }
                        break;
                    case Content.Clothing:
                        volume = (int)bagInfo["clothing"]["volume"];
                        if (clothingIndex + col < volume)
                        {
                            changeActiveItem(clothingBag, clothingIndex, clothingIndex + col);
                            clothingIndex = clothingIndex + col;
                            y = volume / col;
                            if (volume % col != 0)
                            {
                                y++;
                            }
                            z = (y - rowInSight) * (1 - clothingRect.GetComponent<ScrollRect>().normalizedPosition.y);
                            nowRow = clothingIndex / col + 1;
                            if (clothingIndex % col != 0)
                            {
                                nowRow++;
                            }
                            if (z + rowInSight < (float)nowRow)
                            {
                                newR = (float)(nowRow - rowInSight) / (float)(y - rowInSight);
                                clothingRect.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1f - newR);
                            }
                        }
                        break;
                    case Content.Use:
                        volume = (int)bagInfo["food&drug"]["volume"];
                        if (foodAndDrugIndex + col < volume)
                        {
                            changeActiveItem(foodAndDrugBag, foodAndDrugIndex, foodAndDrugIndex + col);
                            foodAndDrugIndex = foodAndDrugIndex + col;
                            y = volume / col;
                            if (volume % col != 0)
                            {
                                y++;
                            }
                            z = (y - rowInSight) * (1 - foodAndDrugRect.GetComponent<ScrollRect>().normalizedPosition.y);
                            nowRow = foodAndDrugIndex / col + 1;
                            if (foodAndDrugIndex % col != 0)
                            {
                                nowRow++;
                            }
                            if (z + rowInSight < (float)nowRow)
                            {
                                newR = (float)(nowRow - rowInSight) / (float)(y - rowInSight);
                                foodAndDrugRect.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1f - newR);
                            }
                        }
                        break;
                    case Content.Etc:
                        volume = (int)bagInfo["metarial"]["volume"];
                        if (metarialIndex + col < volume)
                        {
                            changeActiveItem(metarialBag, metarialIndex, metarialIndex + col);
                            metarialIndex = metarialIndex + col;
                            y = volume / col;
                            if (volume % col != 0)
                            {
                                y++;
                            }
                            z = (y - rowInSight) * (1f - metarialRect.GetComponent<ScrollRect>().normalizedPosition.y);
                            nowRow = metarialIndex / col + 1;
                            if (metarialIndex % col != 0)
                            {
                                nowRow++;
                            }
                            if (z + rowInSight < (float)nowRow)
                            {
                                newR = (float)(nowRow - rowInSight) / (float)(y - rowInSight);
                                metarialRect.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1f - newR);
                            }
                        }
                        break;
                    case Content.Spec:
                        volume = (int)bagInfo["special"]["volume"];
                        if (specialIndex + col < volume)
                        {
                            changeActiveItem(specialBag, specialIndex, specialIndex + col);
                            specialIndex = specialIndex + col;
                            y = volume / col;
                            if (volume % col != 0)
                            {
                                y++;
                            }
                            z = (y - rowInSight) * (1 - specialRect.GetComponent<ScrollRect>().normalizedPosition.y);
                            nowRow = specialIndex / col + 1;
                            if (specialIndex % col != 0)
                            {
                                nowRow++;
                            }
                            if (z + rowInSight < (float)nowRow)
                            {
                                newR = (float)(nowRow - rowInSight) / (float)(y - rowInSight);
                                specialRect.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1f - newR);
                            }
                        }
                        break;
                }
                sleepTime = 0f;
            }
            else if (input.getCrsX() > 0.5f)
            {
                int r = col;
                int volume = 0;
                switch (cont)
                {
                    case Content.Weapon:
                        volume = (int)bagInfo["weapon"]["volume"];
                        r = weaponIndex % col;
                        if (r < col - 1 && weaponIndex < volume - 1)
                        {
                            changeActiveItem(weaponBag, weaponIndex, weaponIndex + 1);
                            weaponIndex++;
                        }
                        break;
                    case Content.Clothing:
                        volume = (int)bagInfo["clothing"]["volume"];
                        r = clothingIndex % col;
                        if (r < col - 1 && clothingIndex < volume - 1)
                        {
                            changeActiveItem(clothingBag, clothingIndex, clothingIndex + 1);
                            clothingIndex++;
                        }
                        break;
                    case Content.Use:
                        volume = (int)bagInfo["food&drug"]["volume"];
                        r = foodAndDrugIndex % col;
                        if (r < col - 1 && foodAndDrugIndex < volume - 1)
                        {
                            changeActiveItem(foodAndDrugBag, foodAndDrugIndex, foodAndDrugIndex + 1);
                            foodAndDrugIndex++;
                        }
                        break;
                    case Content.Etc:
                        volume = (int)bagInfo["metarial"]["volume"];
                        r = metarialIndex % col;
                        if (r < col - 1 && metarialIndex < volume - 1)
                        {
                            changeActiveItem(metarialBag, metarialIndex, metarialIndex + 1);
                            metarialIndex++;
                        }
                        break;
                    case Content.Spec:
                        volume = (int)bagInfo["special"]["volume"];
                        r = specialIndex % col;
                        if (r < col - 1 && specialIndex < volume - 1)
                        {
                            changeActiveItem(specialBag, specialIndex, specialIndex + 1);
                            specialIndex++;
                        }
                        break;
                }
                sleepTime = 0f;
            }
            else if (input.getCrsX() < -0.5f)
            {
                int r = 0;
                switch (cont)
                {
                    case Content.Weapon:
                        r = weaponIndex % col;
                        if (weaponIndex > 0 && r > 0)
                        {
                            changeActiveItem(weaponBag, weaponIndex, weaponIndex - 1);
                            weaponIndex--;
                        }
                        break;
                    case Content.Clothing:
                        r = clothingIndex % col;
                        if (clothingIndex > 0 && r > 0)
                        {
                            changeActiveItem(clothingBag, clothingIndex, clothingIndex - 1);
                            clothingIndex--;
                        }
                        break;
                    case Content.Use:
                        r = foodAndDrugIndex % col;
                        if (foodAndDrugIndex > 0 && r > 0)
                        {
                            changeActiveItem(foodAndDrugBag, foodAndDrugIndex, foodAndDrugIndex - 1);
                            foodAndDrugIndex--;
                        }
                        break;
                    case Content.Etc:
                        r = metarialIndex % col;
                        if (metarialIndex > 0 && r > 0)
                        {
                            changeActiveItem(metarialBag, metarialIndex, metarialIndex - 1);
                            metarialIndex--;
                        }
                        break;
                    case Content.Spec:
                        r = specialIndex % col;
                        if (specialIndex > 0 && r > 0)
                        {
                            changeActiveItem(specialBag, specialIndex, specialIndex - 1);
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

    public void initMenu1() {
        weaponTab.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        int weaponVolume = (int)bagInfo["weapon"]["volume"];
        for (int i = weaponVolume; i < weaponBag.transform.childCount; i++)
        {
            Destroy(weaponBag.transform.GetChild(i).gameObject);
        }
        changeWeaponBag(0, weaponVolume);
        changeActiveItem(weaponBag, -1, 0);
        int clothingVolume = (int)bagInfo["clothing"]["volume"];
        for (int i = clothingVolume; i < clothingBag.transform.childCount; i++)
        {
            Destroy(clothingBag.transform.GetChild(i).gameObject);
        }
        changeClothingBag(0, clothingVolume);
        changeActiveItem(clothingBag, -1, 0);
        int foodAndDrugVolume = (int)bagInfo["food&drug"]["volume"];
        for (int i = foodAndDrugVolume; i < foodAndDrugBag.transform.childCount; i++)
        {
            Destroy(foodAndDrugBag.transform.GetChild(i).gameObject);
        }
        changeFoodAndDrugBag(0, foodAndDrugVolume);
        changeActiveItem(foodAndDrugBag, -1, 0);
        int metarialVolume = (int)bagInfo["metarial"]["volume"];
        for (int i = metarialVolume; i < metarialBag.transform.childCount; i++)
        {
            Destroy(metarialBag.transform.GetChild(i).gameObject);
        }
        changeMetarialBag(0, metarialVolume);
        changeActiveItem(metarialBag, -1, 0);
        int specialVolume = (int)bagInfo["special"]["volume"];
        for (int i = specialVolume; i < specialBag.transform.childCount; i++)
        {
            Destroy(specialBag.transform.GetChild(i).gameObject);
        }
        changeSpecialBag(0, specialVolume);
        changeActiveItem(specialBag, -1, 0);
    }

    public void changeActiveItem(GameObject obj, int from, int to){
        if(from >= 0)
        {
            obj.transform.GetChild(from).GetComponent<Image>().sprite = uiManager.getIconBg(0);
        }
        obj.transform.GetChild(to).GetComponent<Image>().sprite = uiManager.getIconBg(2);
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

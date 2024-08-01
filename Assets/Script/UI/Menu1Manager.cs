using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        cont = Content.Weapon;
        onCheckBox = false;
        weaponIndex = 0;
        clothingIndex = 0;
        foodAndDrugIndex = 0;
        metarialIndex = 0;
        specialIndex = 0;
        sleepTime = 0f;
        input = InputCtrl.Instance;
        uiManager = UIManager.Instance;
        uiManager.registerMenu1Ctrl(this);
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
                }
            }else if(intput.getShdL() > 0.5f){
                switch (cont){
                    case Content.Weapon:
                        break;
                    case Content.Clothing:
                        clothingBag.SetActive(false);
                        weaponBag.SetActive(true);
                        cont = Content.Weapon;
                        break;
                    case Content.Use:
                        foodAndDrugBag.SetActive(false);
                        clothingBag.SetActive(true);
                        cont = Content.Clothing;
                        break;
                    case Content.Etc:
                        metarialBag.SetActive(false);
                        foodAndDrugBag.SetActive(true);
                        cont = Content.Use;
                        break;
                    case Content.Spec:
                        specialBag.SetActive(false);
                        metarialBag.SetActive(true);
                        cont = Content.Etc;
                        break;
                }
            }else if(input.getShdR() > 0.5f){
                switch (cont){
                    case Content.Weapon:
                        clothingBag.SetActive(true);
                        weaponBag.SetActive(false);
                        cont = Content.Clothing;
                        break;
                    case Content.Clothing:
                        foodAndDrugBag.SetActive(true);
                        clothingBag.SetActive(false);
                        cont = Content.Use;
                        break;
                    case Content.Use:
                        metarialBag.SetActive(true);
                        foodAndDrugBag.SetActive(false);
                        cont = Content.Etc;
                        break;
                    case Content.Etc:
                        specialBag.SetActive(true);
                        metarialBag.SetActive(false);
                        cont = Content.Spec;
                        break;
                    case Content.Spec:
                        break;
                }
            }else if(intput.getCrsY > 0.5f){
                switch (cont){
                    case Content.Weapon:
                        if(weaponIndex >= col){
                            changeActiveItem(weaponIndex, weaponIndex - col)
                            weaponIndex = weaponIndex - col;
                        }
                        break;
                    case Content.Clothing:
                        if(clothingIndex >= col){
                            changeActiveItem(clothingIndex, clothingIndex - col)
                            clothingIndex = clothingIndex - col;
                        }
                        break;
                    case Content.Use:
                        if(foodAndDrugIndex >= col){
                            changeActiveItem(foodAndDrugIndex, foodAndDrugIndex - col)
                            foodAndDrugIndex = foodAndDrugIndex - col;
                        }
                        break;
                    case Content.Etc:
                        if(metarialIndex >= col){
                            changeActiveItem(metarialIndex, metarialIndex - col)
                            metarialIndex = metarialIndex - col;
                        }
                        break;
                    case Content.Spec:
                        if(specialIndex >= col){
                            changeActiveItem(specialIndex, specialIndex - col)
                            specialIndex = specialIndex - col;
                        }
                        break;
                }
            }else if(intpu.getCrsY < -0.5f){
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
            }else if(intput.getCrsX > 0.5f){
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
            }else if(intpu.getCrsX < -0.5f){
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
            }
        }
    }

    public void setBagInfo(JObject data){
        bagInfo = data;
    }

    public void changeActiveItem(int from, int to){
        
    }

    public void changeWeaponBag(int start, int end, JObject data) {
        int volume = (int)data["volume"];
        JArray items = (JArray)data["items"];
        if(weaponBag.transform.childCount < end){
            for(int i = start; i < weaponBag.transform.childCount; i++){

            }
            for(int i = weaponBag.transform.childCount; i < end; i++){
                GameObject obj = Instantiate(itemPrafab);
                obj.name = i.ToString();
                obj.transform.SetParent(weaponBag.transform);
                obj.transform.localScale = new Vector3(1, 1, 1);
            }
        }else{
            for(int i = start; i < end; i++){

            }
        }
    }

    public void changeClothingBag(int start, int end, JObject data) {

    }

    public void changefoodAndDrugBag(int start, int end, JObject data) {

    }

    public void changeMetarialBag(int start, int end, JObject data) {

    }

    public void changeSpecialBag(int start, int end, JObject data) {

    }

    public void showItemInfo(JObject item){

    }
}

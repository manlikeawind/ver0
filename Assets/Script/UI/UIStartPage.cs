using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEditorInternal.VersionControl;
using UnityEngine.UI;

public class UIStartPage : MonoBehaviour
{

    public enum Status
    {
        None = 0,
        MainPage, SaveList, Setting, 
    }

    public GameObject continueGameBtn;
    public GameObject newGameBtn;
    public GameObject endGameBtn;
    public GameObject settingBtn;
    public GameObject maskPage;
    public GameObject saveListPage;
    public GameObject saveListObj;
    public GameObject settingPage;
    public GameObject saveItemPrafab;
    private GameManager gameManager;
    private InputCtrl input;
    private Status status;

    private void Start()
    {
        status = Status.MainPage;
        gameManager = GameManager.Instance;
        input = InputCtrl.Instance;

        string lang = PlayerPrefs.GetString("lang", "en");
        continueGameBtn.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = gameManager.textInfo.getText("ui_startPage_continueGame", lang);
        newGameBtn.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = gameManager.textInfo.getText("ui_startPage_newGame", lang);
        endGameBtn.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = gameManager.textInfo.getText("ui_startPage_endGame", lang);
        settingBtn.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = gameManager.textInfo.getText("ui_startPage_setting", lang);

        EventSystem.current.SetSelectedGameObject(continueGameBtn);
    }

    private void Update()
    {
        switch (status)
        {
            case Status.MainPage:
                break;
            case Status.SaveList:
                if(input.getBtnB() > 0.5f)
                {
                    saveListPage.SetActive(false);
                    maskPage.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(continueGameBtn);
                }
                break;
            case Status.Setting:
                if (input.getBtnB() > 0.5f)
                {
                    settingPage.SetActive(false);
                    maskPage.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(continueGameBtn);
                }
                break;
            default:
                break;
        }
    }

    public void ContinueGameBtnClickEvent()
    {
        status = Status.SaveList;
        saveListPage.SetActive(true);
        maskPage.SetActive(true);
        List<JObject> saveJsonList = gameManager.saveLoad.saveList;
        GameObject firstItem = null;
        if (saveListObj.transform.childCount < saveJsonList.Count)
        {
            for(int i = 0; i < saveListObj.transform.childCount; i++)
            {
                if(i == 0)
                {
                    firstItem = saveListObj.transform.GetChild(0).gameObject;
                }
                updateSaveItem(saveListObj.transform.GetChild(i), saveJsonList[i]);
            }
            for (int i = saveListObj.transform.childCount; i < saveJsonList.Count; i++)
            {
                GameObject obj = Instantiate(saveItemPrafab);
                if (i == 0)
                {
                    firstItem = obj;
                }
                obj.name = i.ToString();
                obj.transform.SetParent(saveListObj.transform);
                obj.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 450 - i * 100, 0);
                obj.transform.localScale = new Vector3(1, 1, 1);
                updateSaveItem(obj.transform, saveJsonList[i]);

                Button btnTmp = obj.GetComponent<Button>();
                btnTmp.onClick.AddListener(delegate () {
                    this.onSaveItemClicked(obj);
                });
            }
        }
        else if (saveListObj.transform.childCount > saveJsonList.Count) {
            for (int i = 0; i < saveJsonList.Count; i++)
            {
                if (i == 0)
                {
                    firstItem = saveListObj.transform.GetChild(0).gameObject;
                }
                updateSaveItem(saveListObj.transform.GetChild(i), saveJsonList[i]);
            }
            for (int i = saveJsonList.Count; i < saveListObj.transform.childCount; i++)
            {
                Destroy(saveListObj.transform.GetChild(i).gameObject);
            }
        }
        else
        {
            for (int i = 0; i < saveJsonList.Count; i++)
            {
                if (i == 0)
                {
                    firstItem = saveListObj.transform.GetChild(0).gameObject;
                }
                updateSaveItem(saveListObj.transform.GetChild(i), saveJsonList[i]);
            }
        }

        EventSystem.current.SetSelectedGameObject(firstItem);
    }

    private void updateSaveItem(Transform saveItem, JObject data)
    {
        saveItem.Find("Title").GetComponent<TextMeshProUGUI>().text = (string)data["scene"];
        DateTime startTime = new DateTime(1970,1,1,0,0,0);
        DateTime dt = startTime.AddSeconds((long)data["time"]).ToLocalTime();
        saveItem.Find("Time").GetComponent<TextMeshProUGUI>().text = dt.ToString();
    }

    public void startNewGameBtnClickEvent() {
        Scene scene = SceneManager.GetSceneByName("S0");
        SceneManager.UnloadSceneAsync(scene);
        gameManager.startNewGame();
    }

    public void settingBtnClickEvent() {
        status = Status.Setting;
        maskPage.SetActive(true);
        settingPage.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void endGameBtnClickEvent() { 
        Application.Quit();
    }

    public void onSaveItemClicked(GameObject sender) {
        int index = Int32.Parse(sender.name);
        List<JObject> saveJsonList = gameManager.saveLoad.saveList;
        JObject save = saveJsonList[index];
        if ((int)save["valid"] == 1)
        {
            //string sceneName = (string)save[""];
        }
    }
}

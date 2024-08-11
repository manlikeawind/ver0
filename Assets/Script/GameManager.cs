using Common;
using Mono.Data.Sqlite;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    //private SqliteHelper sqliteHelper;
    public InputCtrl input;
    public enum Status
    {
        None,
        Pre,
        Run,
        OnUI,
        Onload
    }

    private Status status;
    private Status nextStatus;
    private float statusTime;
    private bool sceneLoadComplete;
    private AreaManager areaManager;
    private UIManager uiManager;

    private GameObject hero;

    public uint frameNum;
    public float gameTime;
    public SaveLoad saveLoad;
    public Resource resource;
    public GameObject videoPlayer;
    public GameObject UIMask;
    public TextInfo textInfo;
    public ItemsInfo itemInfo;

    public JObject gameInfo;

    /*    public Dictionary<string, XmlNode> npcInfo;
        public Dictionary<string, XmlNode> itemInfo;
        public Dictionary<string, XmlNode> playerInfo;*/
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        saveLoad = new SaveLoad();
        textInfo = new TextInfo();
        itemInfo = new ItemsInfo();
        resource = new Resource();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        frameNum = 0;
        gameTime = 0.0f;
        status = Status.None;
        nextStatus = Status.Pre;
        statusTime = 0f;
        sceneLoadComplete = false;
        areaManager = null;
        gameInfo = null;
        uiManager = UIManager.Instance;
        input = InputCtrl.Instance;
        hero = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        gameTime = gameTime + Time.deltaTime;
        statusTime = statusTime + Time.unscaledDeltaTime;
        if(nextStatus != Status.None)
        {
            //init status
            switch(nextStatus)
            {
                case Status.Pre:
                    List<JObject> saveList = saveLoad.getSaveList();
                    if (saveList.Count == 0)
                    {
                        startNewGame();
                    }
                    else
                    {
                        SceneManager.LoadSceneAsync("S0");
                    }
                    break;
                case Status.Run:
                    resumeGame();
                    break;
                case Status.OnUI:
                    pauseGame();
                    break;
                case Status.Onload:
                    pauseGame();
                    UIMask.SetActive(true);
                    sceneLoadComplete = false;
                    if (gameInfo != null)
                    {
                        string scene = (string)gameInfo["data"]["player"]["position"]["scene"];
                        SceneManager.LoadSceneAsync(scene);
                    }
                    break;
            }
            status = nextStatus;
            nextStatus = Status.None;
            statusTime = 0f;
        }
        else
        {
            switch (status)
            {
                case Status.Pre:
                    break;
                case Status.Run:
                    if(input.getBtnStart() > 0.5f)
                    {
                        nextStatus = Status.OnUI;
                        uiManager.enterUI(UIStatus.Menu1);
                    }
                    break;
                case Status.OnUI:
                    if(uiManager.status == UIStatus.Common)
                    {
                        nextStatus = Status.Run;
                    }
                    break;
                case Status.Onload:
                    if (statusTime > 1f && sceneLoadComplete == true)
                    {
                        nextStatus = Status.Run;
                        JToken pos = gameInfo["data"]["player"]["position"];
                        float x = (float)pos["x"];
                        float y = (float)pos["y"];
                        hero.transform.position = new Vector3(x, y, 0f);
                        UIMask.SetActive(false);
                    }
                    break;
            }
        }
    }

    public string unique16String()
    {
        return Guid.NewGuid().ToString("N").Substring(16,16);
    }
    public void registerAreaManager(AreaManager manager) { 
        areaManager = manager;
    }

    public void selectNewSave(int index) {
        gameInfo = saveLoad.getActiveSave(index);
        uiManager.initUI();
        MoveToNextScene();
    }

    public void setSceneLoadComplete() {
        sceneLoadComplete = true;
    }

    public void startNewGame() {
        VideoPlayer vp = videoPlayer.GetComponent<VideoPlayer>();
        vp.isLooping = false;
        vp.loopPointReached += endBeginingAnimation;
        videoPlayer.SetActive(true);
    }

    public void saveGame()
    {
        saveLoad.save(gameInfo);
    }

    private void endBeginingAnimation(VideoPlayer vp) {
        videoPlayer.SetActive(false);
        gameInfo = saveLoad.createBlankSave();
        uiManager.initUI();
        nextStatus = Status.Onload;
    }

    public void pauseGame()
    {
        Time.timeScale = 0;
    }

    public void resumeGame()
    {
        Time.timeScale = 1.0f;
    }

    public void MoveToNextScene() { 
        if(status == Status.Run)
        {
            areaManager.updateActiveSave();
            saveGame();
        }
        nextStatus = Status.Onload;
    }
}


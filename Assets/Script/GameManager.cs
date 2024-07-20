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
    private InputCtrl input;
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

    public uint frameNum;
    public float gameTime;
    public SaveLoad saveLoad;
    public GameObject videoPlayer;
    public TextInfo textInfo;

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
        input = GetComponent<InputCtrl>();
        DontDestroyOnLoad(gameObject);
        /*menuStatus = 1;
        menuObj = GameObject.FindGameObjectWithTag("Menu");

        //connect to sqlite database
        string connectPath = "Data Source = " + Application.streamingAssetsPath + "/database.db";
        sqliteHelper = new SqliteHelper(connectPath);
        string playerSql = "select * from player";
        SqliteDataReader elementsReader = sqliteHelper.ExecuteQuery(playerSql);
        while (elementsReader.Read()) {
            string sceneName = elementsReader.GetString(elementsReader.GetOrdinal("scene"));
            float x = elementsReader.GetFloat(elementsReader.GetOrdinal("posx"));
            float y = elementsReader.GetFloat(elementsReader.GetOrdinal("posy"));
            transformScene(sceneName, x, y);
            //SceneManager.LoadSceneAsync(sceneName);
            break;
        }*/
    }

    private void Start()
    {
        saveLoad = new SaveLoad();
        textInfo = new TextInfo();
        frameNum = 0;
        gameTime = 0.0f;
        status = Status.None;
        nextStatus = Status.Pre;
        statusTime = 0f;
        sceneLoadComplete = false;
    }

    private void OnDestroy()
    {
        //sqliteHelper.CloseConnection();
    }

    private void Update()
    {
        gameTime = gameTime + Time.deltaTime;
        statusTime = statusTime + Time.deltaTime;
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
                    break;
                case Status.OnUI:
                    break;
                case Status.Onload:
                    sceneLoadComplete = false;
                    JObject activeSave = saveLoad.activeSave;
                    if (activeSave != null)
                    {
                        string scene = (string)activeSave["data"]["player"]["position"]["scene"];
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
                    break;
                case Status.OnUI:
                    break;
                case Status.Onload:
                    if(statusTime > 2f && sceneLoadComplete == true)
                    {
                        nextStatus = Status.Run;
                    }
                    break;
            }
        }
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

    private void endBeginingAnimation(VideoPlayer vp) {
        videoPlayer.SetActive(false);
        saveLoad.createBlankSave();
        nextStatus = Status.Onload;
    }

/*    public SqliteHelper getConnetion() {
        return sqliteHelper;
    }*/

    public void pauseGame()
    {
        Time.timeScale = 0;
    }

    public void resumeGame()
    {
        Time.timeScale = 1.0f;
    }

    /*public void transformScene(string next, float x, float y)
    {
        nextStatus = Status.Onload;
        SceneManager.LoadSceneAsync(next);
        *//*GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = new Vector3(x, y, 0);*//*

    }*/

    public void saveGame() { 
    }

}


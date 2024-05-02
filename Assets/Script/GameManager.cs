using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public enum AttackMode
{
    None = 0,
    sword, dagger, Boom, Axe, Hammer,
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private SqliteHelper sqliteHelper;
    private InputCtrl input;
    private int menuStatus; // 0-close; 1-open
    private GameObject menuObj;
    public enum Status
    {
        None,
        Run,
        OnUI
    }

    public uint frameNum;
    public float gameTime;
    public bool start;

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
        start = true;
        input = GetComponent<InputCtrl>();
        menuStatus = 1;
        menuObj = GameObject.FindGameObjectWithTag("Menu");
        DontDestroyOnLoad(gameObject);

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
        }
    }

    private void Start()
    {
        frameNum = 0;
        gameTime = 0.0f;
        //loadScene("test");
    }

    private void OnDestroy()
    {
        sqliteHelper.CloseConnection();
    }

    private void Update()
    {
        gameTime = gameTime + Time.deltaTime;
    }

    private void FixedUpdate()
    {
        //if menu is open
        if (input.consumeBtnStartDown(0.2f)) {
            if (menuStatus == 0)
            {
                menuObj.SetActive(true);
                menuStatus = 1;
            }
            else if(menuStatus == 1){
                menuObj.SetActive(false);
                menuStatus = 0;
            }
        }
    }

    public SqliteHelper getConnetion() {
        return sqliteHelper;
    }

    public void pauseGame()
    {
        start = false;
        Time.timeScale = 0;
    }

    public void resumeGame()
    {
        start = true;
        Time.timeScale = 1.0f;
    }

    public void transformScene(string next, float x, float y) {
        SceneManager.LoadScene(next);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = new Vector3(x, y, 0);
    }

    public void saveGame() { 
    }

}


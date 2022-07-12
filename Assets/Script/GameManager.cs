using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private SqliteHelper sqliteHelper;
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
        DontDestroyOnLoad(gameObject);

        //connect to sqlite database
        string connectPath = "Data Source = " + Application.streamingAssetsPath + "/database.db";
        sqliteHelper = new SqliteHelper(connectPath);
    }

    private void Start()
    {
        frameNum = 0;
        gameTime = 0.0f;
        loadScene("S00A16");
    }

    private void OnDestroy()
    {
        sqliteHelper.CloseConnection();
    }

    private void Update()
    {
        gameTime = gameTime + Time.deltaTime;
    }

    public SqliteHelper getConnetion() {
        return sqliteHelper;
    }

    public void stopGame()
    {
        start = false;
        Time.timeScale = 0;
    }

    public void resumeGame()
    {
        start = true;
        Time.timeScale = 1.0f;
    }

    public void loadScene(string name) {
        SceneManager.LoadSceneAsync(name);
    }

}

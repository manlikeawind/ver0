using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System;

/*struct Element {
    public string id;
    public string scene;
    public string type;
    public int valid;
    public string description;
    public int lastUpdate;
};

struct MonsterInfo
{
    public string id;
    public string scene;
    public string type;
    public int valid;
    public float hp;
    public string description;
    public int lastUpdate;
}*/

public class AreaManager : MonoBehaviour
{
    private SqliteHelper sqliteHelper;
    /*private Hashtable prefabs;*/
    private Hashtable objects;
    private GameObject root;
    private GameManager gameManager;
    private string _name;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        root = GameObject.FindGameObjectWithTag("Root");
        sqliteHelper = gameManager.getConnetion();
        /*prefabs = new Hashtable();*/
        objects = new Hashtable();

/*        string[] elements = { "fire", "wind" };
        foreach (string e in elements) {
            string path = "Prefabs/scene/" + e;
            GameObject prefab = (GameObject)Resources.Load(path);
            prefabs.Add(e, prefab);
        }*/

        constructScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void constructScene() {
        _name = SceneManager.GetActiveScene().name;
        //init elements
        for (int i = 0; i < 8; i++) {
            string qureyStr = "select * from sceneElements where scene='" + _name + "' and sorted=" + i.ToString();
            SqliteDataReader elementsReader = sqliteHelper.ExecuteQuery(qureyStr);
            Hashtable elementsResult = new Hashtable();
            while (elementsReader.Read())
            {
                string id = elementsReader.GetString(elementsReader.GetOrdinal("id"));
                string type = elementsReader.GetString(elementsReader.GetOrdinal("type"));
                int valid = elementsReader.GetInt32(elementsReader.GetOrdinal("valid"));
                string description = elementsReader.GetString(elementsReader.GetOrdinal("description"));
                int lastUpdate = elementsReader.GetInt32(elementsReader.GetOrdinal("lastUpdate"));
                GameObject obj = root.transform.Find(id).gameObject;
                if (valid == 1 && obj != null)
                {
                    objects.Add(id, obj);
                    obj.GetComponent<BaseObj>().oid = id;
                    obj.GetComponent<BaseObj>().type = type;
                    obj.GetComponent<BaseObj>().lastUpdate = lastUpdate;
                    obj.GetComponent<BaseObj>().construct(description);
                }
            }
        }
        //init monsters
        string monsterQureyStr = "select * from monster where scene='" + _name + "'";
        SqliteDataReader monsterReader = sqliteHelper.ExecuteQuery(monsterQureyStr);
        while (monsterReader.Read())
        {
            string id = monsterReader.GetString(monsterReader.GetOrdinal("id"));
            string type = monsterReader.GetString(monsterReader.GetOrdinal("type"));
            int valid = monsterReader.GetInt32(monsterReader.GetOrdinal("valid"));
            float hp = monsterReader.GetFloat(monsterReader.GetOrdinal("hp"));
            string description = monsterReader.GetString(monsterReader.GetOrdinal("description"));
            int lastUpdate = monsterReader.GetInt32(monsterReader.GetOrdinal("lastUpdate"));
            GameObject obj = root.transform.Find(id).gameObject;
            if(valid == 1 && obj != null)
            {
                objects.Add(id, obj);
                obj.GetComponent<Monster>().oid = id;
                obj.GetComponent<Monster>().type = type;
                obj.GetComponent<Monster>().lastUpdate = lastUpdate;
                obj.GetComponent<Monster>().hp = hp;
                obj.GetComponent<Monster>().construct(description);
            }
        }
    }

    /*public GameObject instGameObject(string name) {
        GameObject instance;
        if (prefabs.ContainsKey(name))
        {
            instance = Instantiate((GameObject)prefabs[name]);
        }
        else
        {
            string path = "Prefabs/scene/" + name;
            GameObject prefab = (GameObject)Resources.Load(path);
            prefabs.Add(name, prefab);
            instance = Instantiate(prefab);
        }
        return instance;
    }*/

    public GameObject getAreaGameObject(string id)
    {
        GameObject obj = (GameObject)objects[id];
        return obj;
    }

    public void transformToNextScene(string next, float x, float y) {
        //save the scene
        //to do

        //transform to next scene
        gameManager.transformScene(next, x, y);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;

struct Element {
    public string oid;
    public string type;
    public float posX;
    public float posY;
};

public class AreaManager : MonoBehaviour
{
    private SqliteHelper sqliteHelper;
    private Hashtable prefabs;

    // Start is called before the first frame update
    void Start()
    {
        GameManager gameManager = GameManager.Instance;
        sqliteHelper = gameManager.getConnetion();
        prefabs = new Hashtable();

        string[] elements = { "fire", "wind" };
        foreach (string e in elements) {
            string path = "Prefabs/scene/" + e;
            GameObject prefab = (GameObject)Resources.Load(path);
            prefabs.Add(e, prefab);
        }

        constructScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void constructScene() {
        string name = SceneManager.GetActiveScene().name;
        string qureyStr = "select * from sceneElements where scene='" + name + "'";
        SqliteDataReader elementsReader = sqliteHelper.ExecuteQuery(qureyStr);
        Hashtable elements = new Hashtable();
        while (elementsReader.Read()) {
            Element tmp;
            string key = elementsReader.GetString(elementsReader.GetOrdinal("id"));
            tmp.oid = elementsReader.GetString(elementsReader.GetOrdinal("oid"));
            tmp.type = elementsReader.GetString(elementsReader.GetOrdinal("type"));
            tmp.posX = elementsReader.GetFloat(elementsReader.GetOrdinal("posX"));
            tmp.posY = elementsReader.GetFloat(elementsReader.GetOrdinal("posY"));
            elements[key] = tmp;
        }
        foreach(string key in elements.Keys) {
            Element e = (Element)elements[key];
            string sqlstr = "select * from " + e.type + " where id='" + e.oid + "'";
            SqliteDataReader reader = sqliteHelper.ExecuteQuery(sqlstr);
            reader.Read();
            GameObject instance;
            if (prefabs.ContainsKey(e.type))
            {
                instance = Instantiate((GameObject)prefabs[e.type]);
            }
            else 
            {
                string path = "Prefabs/scene/" + e.type;
                GameObject prefab = (GameObject)Resources.Load(path);
                prefabs.Add(e.type, prefab);
                instance = Instantiate(prefab);
            }
            instance.name = e.oid;
            instance.transform.position = new Vector2(e.posX, e.posY);
        }
    }

    public GameObject instGameObject(string name) {
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
    }
}

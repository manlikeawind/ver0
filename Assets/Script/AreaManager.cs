using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System;

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

    public void constructScene() {
        _name = SceneManager.GetActiveScene().name;
        gameManager.setSceneLoadComplete();
        gameManager.registerAreaManager(this);
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

    public void updateActiveSave() { }
}

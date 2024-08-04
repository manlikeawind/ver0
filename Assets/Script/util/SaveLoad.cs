using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveLoad
{
    public string basePath;
    public List<JObject> saveList;

    public SaveLoad() { 
        basePath = Application.persistentDataPath + "/";

        saveList = new List<JObject>();
        List<string> names = new List<string>();
        if (Directory.Exists(basePath))
        {
            DirectoryInfo direction = new DirectoryInfo(basePath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            foreach (FileInfo file in files)
            {
                /*if (file.Name.EndsWith(".meta"))
                {
                    continue;
                }*/
                if (file.Name.StartsWith("save") && file.Name.EndsWith(".json"))
                {
                    names.Add(file.Name);
                }
            }
            names.Sort();
            names.Reverse();
            foreach (string name in names)
            {
                string path = basePath + name;
                StreamReader sr = new StreamReader(path);
                JObject obj = JObject.Parse(sr.ReadToEnd());
                if ((int)obj["valid"] == 1)
                {
                    JObject obj2 = new JObject();
                    obj2.Add("name", name);
                    obj2.Add("scene", obj["scene"]);
                    obj2.Add("time", obj["time"]);
                    obj2.Add("playTime", obj["playTime"]);
                    obj2.Add("auto", obj["auto"]);
                    saveList.Add(obj2);
                }
            }
        }
    }

    public JObject getActiveSave(int index) {
        JObject save = saveList[index];
        string name = (string)save["name"];
        string readPath = basePath + name;
        StreamReader sr = new StreamReader(readPath);
        string s = sr.ReadToEnd();
        sr.Close();
        return JObject.Parse(s);
    }

    public List<JObject> getSaveList() { 
        return saveList;
    }

    public JObject createBlankSave() {
        long now = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
        string readPath = Application.streamingAssetsPath + "/blankSave.json";
        StreamReader sr = new StreamReader(readPath);
        string s = sr.ReadToEnd();
        JObject activeSave = JObject.Parse(s);
        sr.Close();
        save(activeSave);
        return activeSave;
    }

    public void addNewSave() {

        //updateSaveList();
    }

    public void updateSave() { 
    }

    public void save(JObject data) {
        long now = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
        string writePath = basePath + "save" + now.ToString() + ".json";
        StreamWriter sw = new StreamWriter(writePath);
        data["time"] = now;
        data["name"] = "save" + now.ToString() + ".json";
        sw.Write(data.ToString());
        sw.Close();
        updateSaveList(data);
    }

    public void updateSaveList(JObject obj) {
        JObject obj2 = new JObject();
        obj2.Add("name", obj["name"]);
        obj2.Add("scene", obj["scene"]);
        obj2.Add("time", obj["time"]);
        obj2.Add("playTime", obj["playTime"]);
        obj2.Add("auto", obj["auto"]);
        
        if(saveList.Count > 4)
        {
            for(int i = 4; i < saveList.Count; i++) { 
                string path = basePath + (string)saveList[i]["name"];
                File.Delete(path);
            }
            saveList = saveList.Take(4).ToList();
        }
        saveList.Reverse();
        saveList.Add(obj2);
        saveList.Reverse();
    }

    public void load() { 
    }
}

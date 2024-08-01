using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemsInfo
{
    private JArray itemList;
    public ItemsInfo() {
        string readPath = Application.streamingAssetsPath + "/item.json";
        StreamReader sr = new StreamReader(readPath);
        string s = sr.ReadToEnd();
        JObject itemsMap = JObject.Parse(s);
        itemList = (JArray)itemsMap["data"];
    }

    public JObject getItemInfo(string code) {
        int index = 0;
        int.TryParse(code, out index);
        return (JObject)itemList[index];
    }
}

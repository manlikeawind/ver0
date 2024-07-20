using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextInfo
{
    private JObject textMap;
    public TextInfo() {
        string readPath = Application.streamingAssetsPath + "/text.json";
        StreamReader sr = new StreamReader(readPath);
        string s = sr.ReadToEnd();
        textMap = JObject.Parse(s);
    }

    public string getText(string id, string lang) {
        return (string)textMap[id][lang];
    }
}

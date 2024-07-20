using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupCtrl : BaseObj
{
    private int token;
    private HashSet<string> monsters;

    public void Start()
    {
        token = 1;
    }
    public bool getToken() {
        if(token == 1)
        {
            token = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void releaseToken()
    {
        token = 1;
    }

    public void registerMonster(string name)
    {
        monsters.Add(name);
    }

    public void removeMonster(string name)
    {
        monsters.Remove(name);
        if(monsters.Count == 0)
        {

        }
    }
}

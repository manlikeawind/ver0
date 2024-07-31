using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource
{
    private Sprite[] abilityIconList;
    public Resource() {
        abilityIconList = Resources.LoadAll<Sprite>("Sprite/background/color");
    }

    public Sprite getAbilityIcon(int index)
    {
        if(index >= abilityIconList.Length)
        {
            return null;
        }
        else
        {
            return abilityIconList[index];
        }
    }

}

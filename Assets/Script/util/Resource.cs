using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource
{
    private Sprite[] abilityIconList;
    private Sprite[] itemIconsList;
    private Sprite[] UIIconsBgList;
    private Sprite blankPng;
    public Resource() {
        abilityIconList = Resources.LoadAll<Sprite>("Sprite/background/color");
        itemIconsList = Resources.LoadAll<Sprite>("Sprite/UI/item");
        blankPng = Resources.Load<Sprite>("Sprite/UI/blankIcon");
        UIIconsBgList = Resources.LoadAll<Sprite>("Sprite/UI/icon_bg");
    }

    public Sprite getBlankPng() {
        return blankPng; 
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

    public Sprite getItemIcon(int index)
    {
        if (index >= itemIconsList.Length)
        {
            return null;
        }
        else
        {
            return itemIconsList[index];
        }
    }

    public Sprite getUIIconBg(int index) {
        if (index >= UIIconsBgList.Length)
        {
            return null;
        }
        else
        {
            return UIIconsBgList[index];
        }
    }
}

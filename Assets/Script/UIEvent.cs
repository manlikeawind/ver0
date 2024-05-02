using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEvent : MonoBehaviour
{
    public Texture2D suit1;
    public Sprite suit1Sprite;
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void buttonClickEvent() {
        /*GameObject body = GameObject.FindGameObjectWithTag("Player").find("body");
        body.GetComponent<SpriteRenderer>().sprite = suit1Sprite;*/
    }
}

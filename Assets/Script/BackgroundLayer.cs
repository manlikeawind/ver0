using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLayer : MonoBehaviour
{
    private GameObject mainCamera;
    public int layer;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        switch (layer) { 
            case 0:
                break;
            case 1:
                transform.position = mainCamera.transform.position * 1 / 4;
                break;
            case 2:
                transform.position = mainCamera.transform.position * 1 / 2;
                break;
            case 3:
                transform.position = mainCamera.transform.position * 3 / 4;
                break;
            case 4:
                transform.position = mainCamera.transform.position * 11 / 12;
                break;
            case 5:
                transform.position = mainCamera.transform.position;
                break;
        }
    }
}

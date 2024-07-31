using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StartPage : MonoBehaviour
{
    private UIDocument document;
    private VisualElement rootElement;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        document = GetComponent<UIDocument>();
        rootElement = document.rootVisualElement;
        List<VisualElement> UISaveItem = rootElement.Query<VisualElement>("SaveItem").ToList();
        var UISaveList = rootElement.Q<VisualElement>("SaveList");

        List<JObject> saveList = gameManager.saveLoad.saveList;
        if(saveList.Count > UISaveItem.Count)
        {
            for(int i = 0; i < UISaveItem.Count; i++)
            {
                updateUISave(saveList[i], UISaveList[i]);
            }
            var list0 = UISaveItem[0];
            for (int i = UISaveItem.Count; i < saveList.Count; i++)
            {
                var visualElement = list0.visualTreeAssetSource.Instantiate();
                updateUISave(saveList[i], visualElement);
                UISaveList.Add(visualElement);
            }

        }else if(saveList.Count < UISaveItem.Count)
        {
            for (int i = 0; i < saveList.Count; i++)
            {
                updateUISave(saveList[i], UISaveList[i]);
            }
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void updateUISave(JObject data, VisualElement ve) { 
    }
}

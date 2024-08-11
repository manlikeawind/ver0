using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interact
{
    public GameObject tip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void focus()
    {
        tip.SetActive(true); 
    }

    public override void blur()
    {
        tip.SetActive(false);
    }

    public override void startInteract()
    {
        
    }
}

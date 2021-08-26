using System.Collections;
using System.Collections.Generic;
using Scripts.Systems;
using UnityEngine;
using ZenFulcrum.EmbeddedBrowser;

public class BrowserController : MonoBehaviour
{
    public Playercontroller player;
    // Start is called before the first frame update
    void Start()
    {
        var browser = GetComponent<Browser>();
        browser.RegisterFunction("move_forward",async (args) => await this.player.MoveForwardAsync());
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

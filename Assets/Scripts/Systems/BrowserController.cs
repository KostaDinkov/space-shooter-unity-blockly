using System.Collections;
using System.Collections.Generic;
using Scripts.Systems;
using UnityEngine;
using ZenFulcrum.EmbeddedBrowser;

public class BrowserController : MonoBehaviour
{
    public Playercontroller player;
    private static BrowserController instance;

    public static BrowserController Instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
        
        var browser = GetComponent<Browser>();
        browser.RegisterFunction("move_forward", async (args) => await this.player.MoveForwardAsync());
        var toolbox = @"{
        ""kind"": ""flyoutToolbox"",
        ""contents"": [
            {
                ""kind"": ""block"",
                ""type"": ""controls_repeat""
            },
            {
                ""kind"": ""block"",
                ""type"": ""controls_whileUntil""
            },
            {
                ""kind"": ""block"",
                ""type"": ""math_number""
            },
            {
                ""kind"": ""block"",
                ""type"": ""math_single""
            },
            {
                ""kind"": ""block"",
                ""type"": ""move_forward""
            }
        ]
        }";

        browser.CallFunction("setWorkSpace", toolbox).Done();

    }

    // Update is called once per frame
    void Update()
    {
    }
}
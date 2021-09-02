using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using Scripts.Systems;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZenFulcrum.EmbeddedBrowser;

public class BrowserController : MonoBehaviour
{
    public Playercontroller player;
    private static BrowserController instance;
    private string lastStartdedLevel;
    public static BrowserController Instance => instance;
    public ToolBox ToolBox;

    private Dictionary<string, string> levelToolboxes;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        
        SceneManager.sceneLoaded += this.OnSceneLoaded;
        this.lastStartdedLevel = SceneManager.GetActiveScene().name;
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        this.levelToolboxes = new Dictionary<string, string>()
        {{
                "l01p01", @"{
                ""kind"": ""flyoutToolbox"",
                ""contents"": [
                    {
                        ""kind"": ""block"",
                        ""type"": ""move_forward""
                    },
                    {
                        ""kind"": ""block"",
                        ""type"": ""rotate_left""
                    },
                    {
                        ""kind"": ""block"",
                        ""type"": ""rotate_right""
                    },
                    {
                        ""kind"": ""block"",
                        ""type"": ""scan_ahead""
                    },
                    {
                        ""kind"": ""block"",
                        ""type"": ""fire_weapon""
                    },
                    {
                        ""kind"": ""block"",
                        ""type"": ""pickup_object""
                    }
                ]
                }"
            },
            {
                "l01p02", @"{
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
                    },
                    {
                        ""kind"": ""block"",
                        ""type"": ""rotate_left""
                    },
                    {
                        ""kind"": ""block"",
                        ""type"": ""rotate_right""
                    },
                    {
                        ""kind"": ""block"",
                        ""type"": ""scan_ahead""
                    },
                    {
                        ""kind"": ""block"",
                        ""type"": ""fire_weapon""
                    },
                    {
                        ""kind"": ""block"",
                        ""type"": ""pickup_object""
                    }
                ]
                }"
            }
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        this.SetBlocklyWorkspace(this.lastStartdedLevel);

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        var currentLevel = SceneManager.GetActiveScene().name;
        if (currentLevel == this.lastStartdedLevel)
        {
            return;
        }

        this.lastStartdedLevel = currentLevel;
        this.SetBlocklyWorkspace(currentLevel);
    }

    public void SetBlocklyWorkspace(string levelName)
    {
        var browser = this.gameObject.GetComponent<Browser>();

        //var toolbox = JsonConvert.SerializeObject(this.ToolBox.CategoryToolbox);
        Debug.Log($"block kind: {this.ToolBox.contents[0].kind}");
        var toolbox = JsonUtility.ToJson(this.ToolBox);
        Debug.Log(toolbox);
        //TODO check if the scene is a level and it is in the dictionary
        browser.CallFunction("setWorkSpace", toolbox/*this.levelToolboxes[levelName]*/).Done();
    }

    // Update is called once per frame
}
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Scripts.Systems;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZenFulcrum.EmbeddedBrowser;

public class BrowserController : MonoBehaviour
{
    public ToolBox ToolBox;
    public static BrowserController Instance => instance;
    
    private static BrowserController instance;
    private Browser browser;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        this.browser = this.gameObject.GetComponent<Browser>();
        instance = this;
    }

    void Start()
    {
        this.browser.onLoad += node =>
        {
            Debug.Log("browser onload...");
            this.SetBlocklyWorkspace();
            var lastWorkspace = PlayerPrefs.GetString("lastWorkspace", "");
            this.browser.CallFunction("loadLastWorkspace", new JSONNode(lastWorkspace));
        };
    }

    public void SetBlocklyWorkspace()
    {
        Debug.Log($"block kind: {this.ToolBox.contents[0].kind}");
        var toolbox = JsonUtility.ToJson(this.ToolBox);
        Debug.Log(toolbox);
        browser.CallFunction("setWorkSpace", toolbox).Done();
    }
}
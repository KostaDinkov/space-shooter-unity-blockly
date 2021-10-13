using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Authentication;
using Cysharp.Threading.Tasks.Triggers;
using Scripts.Systems;
using UnityEngine.SceneManagement;
using ZenFulcrum.EmbeddedBrowser;

public class Auth: MonoBehaviour
{
    // Start is called before the first frame update
    
    private GameData gameData;
    private Browser browser;

    private void Awake()
    {
        this.gameData = GameData.Instance;
        

    }
    void Start()
    {
        this.browser = GameObject.Find("Browser").GetComponent<Browser>();
        this.browser.onLoad += Browser_onLoad;
        
    }

    private void Browser_onLoad(JSONNode obj)
    {
        this.browser.RegisterFunction("saveUsername", Login);
    }

    // Update is called once per frame


    private void Login(JSONNode args)
    {
        //Debug.Log(args[0].Value);
        this.gameData.Username = args[0].Value.ToString();
        //TODO fix scene number
        SceneManager.LoadScene(1);

    }
}

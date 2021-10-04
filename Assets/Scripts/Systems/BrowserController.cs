using System.Linq;
using Scripts.Systems;
using UnityEngine;
using ZenFulcrum.EmbeddedBrowser;

public class BrowserController : MonoBehaviour
{
    public ToolBox ToolBox;
    public static BrowserController Instance => instance;

    private static BrowserController instance;
    private Browser browser;
    private GameData gameData;

    void Awake()
    {
        this.gameData = GameData.Instance;
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

            //var lastWorkspace = PlayerPrefs.GetString("lastWorkspace", "");
            var currentProblemData = this.gameData.UserProblemStates[this.gameData.CurrentLevelName]
                .FirstOrDefault(p =>
                    p.ProblemName == this.gameData.CurrentProblemName
                );
            if (currentProblemData != null)
            {
                this.browser.CallFunction("loadLastWorkspace", new JSONNode(currentProblemData.ProblemBlocksXml));
            }
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
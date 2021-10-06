using System.Linq;
using Scripts.GameEvents;
using Scripts.Systems;
using UnityEngine;
using ZenFulcrum.EmbeddedBrowser;

public class BlocklyManager : MonoBehaviour
{
    //Pass the toolbox scriptable object for the
    //current problem in the Unity inspector
    public ToolBox ToolBox;
    public static BlocklyManager Instance => instance;
    private static BlocklyManager instance;
    private Browser browser;
    private GameData gameData;
    private GameEventManager gameEventManager;

    void Awake()
    {
        this.gameEventManager = GameEventManager.Instance;
        this.gameData = GameData.Instance;
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        this.browser = this.gameObject.GetComponent<Browser>();
        instance = this;
        this.gameEventManager.Subscribe(GameEventType.ProblemStarted, _ => { this.LoadSavedBlocks(); });
    }

    void Start()
    {
        this.browser.onLoad += node =>
        {
            this.SetBlocklyToolbox();
            this.LoadSavedBlocks();
        };
    }


    public void SetBlocklyToolbox()
    {
        
        var toolbox = JsonUtility.ToJson(this.ToolBox);
        browser.CallFunction("setToolbox", toolbox).Done();
    }

    public void LoadSavedBlocks()
    {
        var currentProblemData = this.gameData.UserProblemStates[this.gameData.CurrentLevelName]
            .FirstOrDefault(p =>
                p.ProblemName == this.gameData.CurrentProblemName
            );
        if (currentProblemData != null)
        {
            this.browser.CallFunction("loadLastWorkspace", new JSONNode(currentProblemData.ProblemBlocksXml));
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Scripts.Exceptions;
using Scripts.GameEvents;
using Scripts.Systems;
using UnityEngine;
using ZenFulcrum.EmbeddedBrowser;
using IPromise = ZenFulcrum.EmbeddedBrowser.IPromise;

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
        this.browser.RegisterFunction("blocksCountChanged",BlocksUpdated);
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

    public void SaveBlocksXml()
    {
        this.browser.CallFunction("saveWorkspace").Then(res =>
        {
            //save to local gameData
            var blocksXml = (string) res.Value;
            Debug.Log(blocksXml);
            var problemState = this.gameData.UserProblemStates[this.gameData.CurrentLevelName].Find(p =>
                p.ProblemName == this.gameData.CurrentProblemName);
            problemState.ProblemBlocksXml = blocksXml;

            //persist to DB
            //Note: score is 0 because the problem is not yet completed
            this.gameData.dbApi.SaveProblemState(
                this.gameData.Username,
                this.gameData.CurrentLevelName,
                this.gameData.CurrentProblemName,
                blocksXml,
                0);
        }).Done();
    }

    public Task<string> GetCode()
    {
        var tcs = new TaskCompletionSource<string>();
        this.browser.CallFunction("getCode").Then(res =>
        {
            var code = (string) res.Value;
            Debug.Log(code);
            tcs.SetResult(code);
        }).Catch((ex) => throw ex);
        return tcs.Task;
    }

    public Task<int> GetBlocksCount()
    {
        var tcs = new TaskCompletionSource<int>();
        this.browser.CallFunction("getBlocksCount").Catch(ex => throw ex)
            .Then(res => tcs.SetResult(int.Parse(res.Value.ToString())));
        return tcs.Task;
    }

    public void BlocksUpdated(JSONNode args)
    {
        
        this.gameEventManager.Publish(new GameEvent(){EventType = GameEventType.BlocksUpdated, EventArgs = (int)args[0]});
    }


}
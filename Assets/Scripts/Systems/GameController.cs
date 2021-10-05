#define isDebug

using System;
using AzureSqlDbConnect;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CSharp.RuntimeBinder;
using Scripts.GameEvents;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZenFulcrum.EmbeddedBrowser;

namespace Scripts.Systems
{
    /// <summary>
    ///     API for controlling the game flow and getting information about
    ///     the current state.
    /// </summary>
    public class GameController : MonoBehaviour
    {
        public static Objectives.Objectives Objectives;

        private static bool isOverlayMenuOpen;
        private Browser browser;
        private GameData gameData;
        private GameEventManager gameEventManager;
        public Playercontroller Player;
        public GameObject OverlayMenu;
        private const string DebugTag = "GameController";
        public bool IsProblemComplete { get; set; }
        public bool IsPlayerDead { get; set; }
        public static GameController Instance { get; private set; }

        private void Awake()
        {
            //Make sure there is only one instance of the GameController class (Singleton)
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            this.gameData = GameData.Instance;
            this.gameEventManager = GameEventManager.Instance;

            Objectives = this.GetComponent<Objectives.Objectives>();
            foreach (var objective in Objectives.ObjectiveList)
            {
                objective.Init();
            }

            // get level and problem names
            var sceneName = SceneManager.GetActiveScene().name;
            this.gameData.CurrentLevelName = sceneName.Substring(0, 3);
            this.gameData.CurrentProblemName = sceneName.Substring(3, 3);
        }

        private void Start()
        {
            this.gameEventManager.Subscribe(GameEventType.ObjectiveCompleted, this.IsProblemCompleted);
            this.gameEventManager.Subscribe(GameEventType.ProblemCompleted, this.UnlockNextProblem);
            this.gameEventManager.Subscribe(GameEventType.PlayerDied, value => this.IsPlayerDead = true);
            this.gameEventManager.Publish(new GameEvent {EventType = GameEventType.ProblemStarted});

            this.gameData.CurrentProblem = SceneManager.GetActiveScene().buildIndex;
            this.browser = GameObject.Find("Browser (GUI)").GetComponent<Browser>();
        }

        public void RunCode()
        {
            this.gameEventManager.Publish(new GameEvent {EventType = GameEventType.ScriptStarted});

            //save workspace blocks
            this.SaveBlocksXml();

            this.browser.CallFunction("getCode").Then(async res =>
            {
                var code = (string) res.Value;

                Debug.Log(code);
                var globals = new Globals {Player = this.Player};
                try
                {
                    await CSharpScript.EvaluateAsync(
                        code, ScriptOptions.Default
                            .WithImports("UnityEngine", "System", "System.Collections.Generic")
                            .WithReferences(typeof(MonoBehaviour).Assembly, typeof(CSharpArgumentInfo).Assembly),
                        globals);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                if (!this.IsProblemComplete)
                {
                    this.gameEventManager.Publish(new GameEvent {EventType = GameEventType.SolutionFailed});
                }
            }).Done();
        }


        /// <summary>
        ///     Resets / reloads the currently active challenge.
        /// </summary>
        public void RestartChallenge()
        {
            this.Player.gameObject.SetActive(true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            this.gameEventManager.Publish(new GameEvent {EventType = GameEventType.ProblemStarted});
        }

        /// <summary>
        ///     Load the next problem
        /// </summary>
        public void NextChallenge()
        {
            if (this.gameData.GameComplete)
            {
                Debug.LogWarning($"{DebugTag}: No more problems");
                return;
            }

            var nextProblemSceneName = this.gameData.GetNextProblemSceneName();
            var nextProblemState = this.gameData.FindProblemState(nextProblemSceneName);

            if (nextProblemState.ProblemLocked)
            {
                Debug.LogWarning($"{DebugTag}: Problem '{nextProblemSceneName}' is locked");
                return;
            }

            SceneManager.LoadScene(nextProblemSceneName);
        }


        private void IsProblemCompleted(int value)
        {
            foreach (var objective in Objectives.ObjectiveList)
            {
                if (!objective.IsComplete())
                {
                    return;
                }
            }

            this.gameEventManager.Publish(new GameEvent {EventType = GameEventType.ProblemCompleted});
            this.IsProblemComplete = true;
        }


        public void UnlockNextProblem(int value)
        {
            //Set current problem as completed
            this.SaveBlocksXml();
            this.browser.CallFunction("getBlocksCount").Then(res =>
            {
                this.gameData.dbApi.SetProblemScore(
                    this.gameData.Username,
                    this.gameData.CurrentLevelName,
                    this.gameData.CurrentProblemName,
                    int.Parse(res.Value.ToString()));
            }).Done();

            //Unlock next problem
            var nextProblemSceneName = this.gameData.GetNextProblemSceneName();
            if (nextProblemSceneName == null)
            {
                Debug.LogWarning($"{DebugTag}: Couldn't find next problem");
                this.gameData.GameComplete = true;
                return;
            }

            var nextProblemState = this.gameData.FindProblemState(nextProblemSceneName);

            if (!nextProblemState.ProblemLocked)
            {
                Debug.Log($"{DebugTag}: Problem '{nextProblemSceneName}' already unlocked");
                return;
            }

            nextProblemState.ProblemLocked = false;
            this.gameData.dbApi.SetProblemLocked(
                this.gameData.Username,
                nextProblemState.LevelName,
                nextProblemState.ProblemName, false);
            //NOTE this could be done on database level with a trigger
            this.gameData.dbApi.SetLastUnlockedProblem(this.gameData.Username, nextProblemState.Id);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isOverlayMenuOpen)
                {
                    this.ResumeGame();
                }
                else
                {
                    this.OpenMenu();
                }
            }
        }

        private void ResumeGame()
        {
            isOverlayMenuOpen = false;
            this.OverlayMenu.SetActive(false);
        }

        private void OpenMenu()
        {
            isOverlayMenuOpen = true;
            this.OverlayMenu.SetActive(true);
        }

        public class Globals
        {
            public Playercontroller Player;
        }

        private void SaveBlocksXml()
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
                //Todo fix score
                this.gameData.dbApi.SaveProblemState(
                    this.gameData.Username,
                    this.gameData.CurrentLevelName,
                    this.gameData.CurrentProblemName,
                    blocksXml,
                    0);
            }).Done();
        }
    }
}
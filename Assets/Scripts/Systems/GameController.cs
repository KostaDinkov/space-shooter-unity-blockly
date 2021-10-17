#define isDebug

using System;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CSharp.RuntimeBinder;
using Scripts.GameEvents;
using UnityEngine;
using UnityEngine.SceneManagement;


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

        private BlocklyManager blocklyManager;
        private GameData gameData;
        private GameEventManager gameEventManager;
        public Playercontroller Player;
        public GameObject OverlayMenu;
        private const string DebugTag = "GameController";
        public bool IsProblemComplete { get; set; }
        public bool IsPlayerDead { get; set; }
        public static GameController Instance { get; private set; }

        public ToolBox Toolbox;

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
            this.blocklyManager = GameObject.Find("Browser (GUI)").GetComponent<BlocklyManager>();
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
        public async void RunCode()
        {
            this.gameEventManager.Publish(new GameEvent {EventType = GameEventType.ScriptStarted});

            //save workspace blocks
            await this.gameData.SaveWorkspace();

            var code = await this.blocklyManager.GetCode();

            //Debug.Log(code);
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
                Debug.LogWarning($"{DebugTag}: Няма повече задачи!");
                SceneManager.LoadScene("FinalScene");
                return;
            }

            var nextProblemSceneName = this.gameData.GetNextProblemSceneName();
            var nextProblemState = this.gameData.FindProblemState(nextProblemSceneName);

            if (nextProblemState.ProblemLocked)
            {
                Debug.LogWarning($"{DebugTag}: Проблемът '{nextProblemSceneName}' е заключен");
                return;
            }

            SceneManager.LoadScene(nextProblemSceneName);
        }


        private void IsProblemCompleted(object args)
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


        public async void UnlockNextProblem(object args)
        {
            
            await this.gameData.SaveWorkspace();
            
            //Set the score and problem completed
            //TODO move to GameData
            int blockCount = await this.blocklyManager.GetBlocksCount();
            await this.gameData.dbApi.SetProblemScore(
                this.gameData.Username,
                this.gameData.CurrentLevelName,
                this.gameData.CurrentProblemName,
                blockCount);

            //Unlock next problem
            var nextProblemSceneName = this.gameData.GetNextProblemSceneName();
            if (nextProblemSceneName == null)
            {
                Debug.LogWarning($"{DebugTag}: Не може да бъде намерена следваща задача");
                this.gameData.GameComplete = true;
                return;
            }

            var nextProblemState = this.gameData.FindProblemState(nextProblemSceneName);

            if (!nextProblemState.ProblemLocked)
            {
                //Debug.Log($"{DebugTag}: Задачата '{nextProblemSceneName}' е вече отключена");
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
    }
}
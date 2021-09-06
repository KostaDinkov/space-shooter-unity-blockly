#define isDebug

using System;

using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Scripts.GameEvents;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZenFulcrum.EmbeddedBrowser;
using Microsoft.CSharp;
using Microsoft.CSharp.RuntimeBinder;

namespace Scripts.Systems
{
    /// <summary>
    /// API for controlling the game flow and getting information about
    /// the current state.
    /// </summary>
    public class GameController : MonoBehaviour
    {
        public bool IsProblemComplete { get; set; }
        public bool IsPlayerDead { get; set; }
        private GameEventManager gameEventManager;
        private GameData gameData;
        public static GameController Instance { get; private set; }
        public static Objectives.Objectives Objectives;
        public Playercontroller Player;
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
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += this.OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //TODO: publish event
        }

        private void Start()
        {
            this.gameEventManager.Subscribe(GameEventType.ObjectiveCompleted, this.IsProblemCompleted);
            this.gameEventManager.Subscribe(GameEventType.ProblemCompleted, this.UnlockNextProblem);
            this.gameEventManager.Subscribe(GameEventType.PlayerDied, (value) => this.IsPlayerDead = true);
            this.gameEventManager.Publish(new GameEvent() {EventType = GameEventType.ProblemStarted});


            this.gameData.CurrentProblem = SceneManager.GetActiveScene().buildIndex;
        }
        public class Globals
        {
            public Playercontroller Player;
        }

        public void RunCode()
        {
            var browser = GameObject.Find("Browser (GUI)").GetComponent<Browser>();
            this.gameEventManager.Publish(new GameEvent() { EventType = GameEventType.ScriptStarted });
            browser.CallFunction("getCode").Then(async res =>
            {
                
                string code = (string)res.Value;
                //code = $"try {{{code}}} catch (Exception e){{Debug.LogException(e);}}";
                Debug.Log(code);
                var globals = new Globals { Player = this.Player };
                try
                {
                    await CSharpScript.EvaluateAsync(
                        code, ScriptOptions.Default
                            .WithImports("UnityEngine","System")
                            .WithReferences(typeof(MonoBehaviour).Assembly, typeof(CSharpArgumentInfo).Assembly), globals: globals);
                }
                catch (Exception e)
                {
                    Debug.LogException( e);
                }
                
                if (!this.IsProblemComplete)
                {
                    this.gameEventManager.Publish(new GameEvent() { EventType = GameEventType.SolutionFailed });
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
            this.gameEventManager.Publish(new GameEvent() {EventType = GameEventType.ProblemStarted});
        }

        /// <summary>
        ///     Destroys the currently active challenge and instantiates the next one.
        /// </summary>
        public void NextChallenge()
        {
            if (this.gameData.LastUnlockedProblem >= this.gameData.ProblemsCount )
            {
                //TODO no more problems - implement game win condition
                Debug.Log($"No more problems");
                return;
            }

            if (this.gameData.CurrentProblem == this.gameData.LastUnlockedProblem)
            {
                //TODO next problem is still locked, update UI
                Debug.Log($"Next level: ({this.gameData.CurrentProblem + 1}) is locked");
                return;
            }

            SceneManager.LoadScene(this.gameData.CurrentProblem+1);
           
        }

        private void InitObjectives(int value)
        {
            
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
            
            if (this.gameData.LastUnlockedProblem >= this.gameData.ProblemsCount)
            {
                return;
            }
            this.gameData.LastUnlockedProblem = this.gameData.CurrentProblem + 1;
            

        }


    }
}
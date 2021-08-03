#define isDebug

using Assets.Scripts.GameEvents;
using Game.GameEvents;
using Game.Objectives;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Systems
{
    /// <summary>
    /// API for controlling the game flow and getting information about
    /// the current state.
    /// </summary>
    public class GameController : MonoBehaviour
    {
        public static GameController Instance;

        private BrowserManager browserManager;
        //public GameObject[] Challenges;
        public GameData gameData;
        private GameEventManager gameEventManager;
        private bool gameOver = false;
        private ObjectivesManager objectivesManager;
        public GameObject Player;

        private void Awake()
        {
            //Make sure there is only one instance of the GameController class (Singleton)
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            gameData = GameData.Instance;
            gameEventManager = GameEventManager.Instance;
            browserManager = new BrowserManager();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //TODO: publish event
            
        }

        private void Start()
        {
            //gameData.ChallengeCount = Challenges.Length;

            InitLevel();
            objectivesManager = new ObjectivesManager();
        }


        private void InitLevel()
        {

            gameData.Objectives = this.GetComponent<Objectives.Objectives>();
        }

        /// <summary>
        ///     Resets / reloads the currently active challenge.
        /// </summary>
        public void RestartChallenge()
        {
#if isDebug
            Debug.Log("Restart Challenge Called");
#endif
            
            Player.SetActive(true);
            InitLevel();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            this.gameEventManager.Publish(new GameEvent() { EventType = GameEventType.ChallangeStarted });
        }

        /// <summary>
        ///     Destroys the currently active challenge and instantiates the next one.
        /// </summary>
        public void NextChallenge()
        {
            
            if (SceneManager.GetActiveScene().buildIndex >= SceneManager.sceneCount - 1)
            {
                //this is the last scene so there is no more scenes to load
            }
            
        }

        
        
    }
}
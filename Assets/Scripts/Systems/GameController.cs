using UnityEngine;
using UnityEngine.SceneManagement;
using Easy.MessageHub;
using Game.GameEvents;
using Game.Objectives;

namespace Game.Systems
{
    public class GameController : MonoBehaviour
    {
        public GameObject Player;
        public GameObject[] Challenges;
        public static GameController Instance;
        private bool gameOver = false;
        private MessageHub hub;
        private BrowserManager browserManager;
        public GameData gameData;
        private ObjectivesManager objectivesManager;
        private GameEventManager gameEventManager;

        void Awake()
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
            
            this.hub = MessageHub.Instance;
            this.browserManager = new BrowserManager();
            
            
            
            
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            hub.Publish(new Game.Systems.GameEvents.SceneLoaded());
            ;
        }

        void Start()
        {
            this.gameData.ChallengeCount = this.Challenges.Length;
            
            InitLevel();
            this.objectivesManager = new ObjectivesManager();
        }


        void InitLevel()
        {
            this.Player.transform.position = new Vector3(0, 0, 0);
            this.Player.transform.rotation = Quaternion.identity;
            this.gameData.CurrentChallenge = Instantiate(Challenges[this.gameData.CurrentChallengeNumber]);
        }

        /// <summary>
        /// Resets / reloads the currently active challenge.
        /// </summary>
        public void RestartChallenge()
        {
            Destroy(this.gameData.CurrentChallenge);
            Player.SetActive(true);
            InitLevel();
        }

        /// <summary>
        /// Destroys the currently active challenge and instantiates the next one.
        /// </summary>
        public void NextChallenge()
        {
            if (this.gameData.CurrentChallengeNumber + 1 >= this.Challenges.Length)
            {
                this.hub.Publish(new GameEvents.LevelCompleted());
                return;
            }

            Destroy(this.gameData.CurrentChallenge);
            this.gameData.CurrentChallengeNumber += 1;
            InitLevel();
            this.hub.Publish(new GameEvents.NewChallangeStarted(this.gameData.CurrentChallengeNumber));
        }

        /// <summary>
        /// Destroys the currently active challenge and loads new challenge by provided index
        /// </summary>
        /// <param name="challengeNumber">The zero based index of the challenge to be loaded</param>
        public void StartNthChallenge(int challengeNumber)
        {
            if (challengeNumber < 0 || challengeNumber >= this.Challenges.Length)
            {
                Debug.Log($"Challange number out of range: {challengeNumber}");
                return;
            }

            Destroy(this.gameData.CurrentChallenge);
            this.gameData.CurrentChallengeNumber = challengeNumber;
            InitLevel();
            this.hub.Publish(new GameEvents.NewChallangeStarted(challengeNumber));
        }
    }
}
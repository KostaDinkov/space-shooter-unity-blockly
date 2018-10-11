using Game.GameEvents;
using Game.Objectives;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Systems
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance;

        private BrowserManager browserManager;
        public GameObject[] Challenges;
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
            gameData.ChallengeCount = Challenges.Length;

            InitLevel();
            objectivesManager = new ObjectivesManager();
        }


        private void InitLevel()
        {
            Player.transform.position = new Vector3(0, 0, 0);
            Player.transform.rotation = Quaternion.identity;
            gameData.CurrentChallenge = Instantiate(Challenges[gameData.CurrentChallengeNumber]);
        }

        /// <summary>
        ///     Resets / reloads the currently active challenge.
        /// </summary>
        public void RestartChallenge()
        {
            Destroy(gameData.CurrentChallenge);
            Player.SetActive(true);
            InitLevel();
        }

        /// <summary>
        ///     Destroys the currently active challenge and instantiates the next one.
        /// </summary>
        public void NextChallenge()
        {
            if (gameData.CurrentChallengeNumber + 1 >= Challenges.Length)
            {
                //TODO publish event
                //this.gameEventManager.Publish(LevelCompletedEvent);
                return;
            }

            Destroy(gameData.CurrentChallenge);
            gameData.CurrentChallengeNumber += 1;
            InitLevel();
            //TODO publish event
            //this.gameEventManager.Publish(ChallangeStartedEvent);
        }

        /// <summary>
        ///     Destroys the currently active challenge and loads new challenge by provided index
        /// </summary>
        /// <param name="challengeNumber">The zero based index of the challenge to be loaded</param>
        public void StartNthChallenge(int challengeNumber)
        {
            if (challengeNumber < 0 || challengeNumber >= Challenges.Length)
            {
                Debug.Log($"Challange number out of range: {challengeNumber}");
                return;
            }

            Destroy(gameData.CurrentChallenge);
            gameData.CurrentChallengeNumber = challengeNumber;
            InitLevel();
            //TODO publish event
            //this.gameEventManager.Publish(ChallangeStartedEvent);
            
        }
    }
}
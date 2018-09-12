using UnityEngine;
using UnityEngine.SceneManagement;
using Easy.MessageHub;

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

            this.hub = MessageHub.Instance;
            this.browserManager = new BrowserManager();
            this.gameData =  GameData.Instance;
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            hub.Publish(new Game.Systems.GameEvents.SceneLoaded());
            Debug.Log("Publishing event SceneLoaded");
        }

        void Start()
        {
            this.gameData.ChallengeCount = this.Challenges.Length;
            this.hub.Publish(new Game.Systems.GameEvents.NewChallangeStarted(this.gameData.CurrentChallengeNumber));
            InitLevel();
        }


        void InitLevel()
        {
            this.Player.transform.position = new Vector3(0, 0, 0);
            this.Player.transform.rotation = Quaternion.identity;
            this.gameData.CurrentChallenge = Instantiate(Challenges[this.gameData.CurrentChallengeNumber]);
        }


        public void RestartChallenge()
        {
            Destroy(this.gameData.CurrentChallenge);
            Player.SetActive(true);
            InitLevel();
        }

        public void NextChallenge()
        {
            if (this.gameData.CurrentChallengeNumber + 1 >= this.Challenges.Length)
            {
                this.hub.Publish(new Game.Systems.GameEvents.LevelCompleted());
                return;
            }

            Destroy(this.gameData.CurrentChallenge);
            this.gameData.CurrentChallengeNumber += 1;
            InitLevel();
            this.hub.Publish(new Game.Systems.GameEvents.NewChallangeStarted(this.gameData.CurrentChallengeNumber));
        }

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
            this.hub.Publish(new Game.Systems.GameEvents.NewChallangeStarted(challengeNumber));
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using Easy.MessageHub;

namespace Game.Systems
{
  public class GameController : MonoBehaviour
  {

    public GameObject Player;
    public GameObject[] Challenges;

    public static GameController instance;
    private bool gameOver = false;
    private MessageHub hub;

    void Awake()
    {
      //Make sure there is only one instance of the GameController class (Singleton)
      if (instance == null)
      {
        instance = this;
      }
      else if (instance != this)
      {
        Destroy(this.gameObject);
        return;
      }

    }
    void Start()
    {
      this.hub = MessageHub.Instance;
      GameData.ChallengeCount = this.Challenges.Length;
      GameProgressBar.instance.Init(GameData.ChallengeCount);
      this.hub.Publish(new Game.Events.NewChallangeStarted(GameData.CurrentChallengeIndex));
      InitLevel();
    }



    void InitLevel()
    {
      this.Player.transform.position = new Vector3(0, 0, 0);
      this.Player.transform.rotation = Quaternion.identity;
      GameData.CurrentChallenge = Instantiate(Challenges[GameData.CurrentChallengeIndex]);
    }


    public void RestartLevel()
    {
      Destroy(GameData.CurrentChallenge);
      Player.SetActive(true);
      InitLevel();
    }

    public void NextLevel()
    {
      if (GameData.CurrentChallengeIndex + 1 >= this.Challenges.Length)
      {
        this.hub.Publish(new Game.Events.LevelCompleted());
        return;
      }

      Destroy(GameData.CurrentChallenge);
      GameData.CurrentChallengeIndex += 1;
      InitLevel();
      this.hub.Publish(new Game.Events.NewChallangeStarted(GameData.CurrentChallengeIndex));

    }
  }
}
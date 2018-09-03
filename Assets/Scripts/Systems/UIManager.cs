using System;
using UnityEngine;
using UnityEngine.UI;
using Easy.MessageHub;
using Game.Systems;
public class UIManager : MonoBehaviour
{
    public Button RestartButton;
    public Button NextLevelButton;
    public static UIManager instance;
    
    void Awake()
    {
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
        this.RestartButton.onClick.AddListener(GameController.instance.RestartLevel);
        this.NextLevelButton.onClick.AddListener(GameController.instance.NextLevel);
        this.NextLevelButton.interactable = false;
        var hub = MessageHub.Instance;
        var OCtoken = hub.Subscribe<Game.Systems.GameEvents.ChallengeCompleted>(onObjectiveCompleted);
        var NextLevelStartedToken = hub.Subscribe<Game.Systems.GameEvents.NewChallangeStarted>(onNewLevelStarted);

    }
    private Action<Game.Systems.GameEvents.NewChallangeStarted> onNewLevelStarted = nls =>
    {
      instance.ActivateChallenge(nls.ChallengeNumber);
    };
    
    private Action<Game.Systems.GameEvents.ChallengeCompleted> onObjectiveCompleted = oc => 
    {
      instance.NextLevelButton.interactable = true;
      
    };
    
    public void ActivateChallenge(int challenge)
    {
        this.NextLevelButton.interactable = false;
        //check if we are at the last challenge;
        if(challenge+1 == GameData.ChallengeCount){
          this.NextLevelButton.GetComponentInChildren<Text>().text = "Complete Level";
        }
        GameProgressBar.instance.ActivateLevel(challenge);
    }


}
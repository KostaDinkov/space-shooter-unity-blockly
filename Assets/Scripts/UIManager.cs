using System;
using UnityEngine;
using UnityEngine.UI;
using Easy.MessageHub;
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
        var token = hub.Subscribe<ObjectiveCompleted>(onObjectiveCompleted);
    }

    private Action<ObjectiveCompleted> onObjectiveCompleted = oc=> 
    {
      instance.NextLevelButton.interactable = true;
      Debug.Log("onObjectiveCompleted");
    };
    
    public void ActivateLevel(int level)
    {
        GameProgressBar.instance.ActivateLevel(level);
    }


}
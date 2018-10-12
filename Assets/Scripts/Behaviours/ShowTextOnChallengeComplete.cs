using Assets.Scripts.GameEvents;
using Game.GameEvents;
using UnityEngine;
using UnityEngine.UI;


public class ShowTextOnChallengeComplete : MonoBehaviour
{
    private GameEventManager eventManager;
    private Text textMesh;
    

    void Start()
    {
        this.textMesh = GetComponent<Text>();
        this.eventManager = GameEventManager.Instance;
        textMesh.enabled = false;
        eventManager.Subscribe(new GameEvent{EventType = GameEventType.ChallangeCompleted} , this.ShowChallangeCompletedText);
        eventManager.Subscribe(new GameEvent { EventType = GameEventType.PlayerDied }, this.ShowPlayerDiedText);
        eventManager.Subscribe(new GameEvent { EventType = GameEventType.ChallangeStarted }, this.HideText);
    }

    public void ShowChallangeCompletedText()
    {
        textMesh.text = "ChallangeCompleted";
        textMesh.enabled = true;
    }
    public void ShowPlayerDiedText()
    {
        textMesh.text = "Player Died.\nRestart Challange.";
        textMesh.enabled = true;
    }
    public void HideText()
    {
        textMesh.text = "";
        textMesh.enabled = false;
    }
}
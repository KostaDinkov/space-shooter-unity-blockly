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
        eventManager.Subscribe(new GameEvent{EventType = GameEventType.ChallangeCompleted} , this.ShowText);
    }

    public void ShowText()
    {
        textMesh.enabled = true;
    }
}
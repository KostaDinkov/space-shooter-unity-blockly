using Assets.Scripts.GameEvents;
using Game.GameEvents;
using UnityEngine;
using UnityEngine.UI;


public class ShowTextOnChallengeComplete : MonoBehaviour
{
    private GameEventManager eventManager;
    private Text textMesh;
    public GameEvent Event;

    void Start()
    {
        this.textMesh = GetComponent<Text>();
        this.eventManager = GameEventManager.Instance;
        textMesh.enabled = false;
        eventManager.Subscribe(Event, ShowText);
    }

    public void ShowText()
    {
        textMesh.enabled = true;
    }
}
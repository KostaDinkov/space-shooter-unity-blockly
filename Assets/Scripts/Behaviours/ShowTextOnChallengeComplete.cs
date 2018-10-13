using Game.GameEvents;
using UnityEngine;
using UnityEngine.UI;

public class ShowTextOnChallengeComplete : MonoBehaviour
{
    private GameEventManager eventManager;
    private Text textMesh;


    private void Start()
    {
        textMesh = GetComponent<Text>();
        eventManager = GameEventManager.Instance;
        textMesh.enabled = false;
        eventManager.Subscribe(GameEventType.ChallangeCompleted, ShowChallangeCompletedText);
        eventManager.Subscribe(GameEventType.PlayerDied, ShowPlayerDiedText);
        eventManager.Subscribe(GameEventType.ChallangeStarted, HideText);
    }

    public void ShowChallangeCompletedText(int value)
    {
        textMesh.text = "ChallangeCompleted";
        textMesh.enabled = true;
    }

    public void ShowPlayerDiedText(int value)
    {
        textMesh.text = "Player Died.\nRestart Challange.";
        textMesh.enabled = true;
    }

    public void HideText(int value)
    {
        textMesh.text = "";
        textMesh.enabled = false;
    }
}
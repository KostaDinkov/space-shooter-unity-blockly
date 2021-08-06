using Scripts.GameEvents;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Behaviours
{
    public class ShowTextOnChallengeComplete : MonoBehaviour
    {
        private GameEventManager eventManager;
        private Text textMesh;


        private void Awake()
        {
            this.textMesh = this.GetComponent<Text>();
            this.eventManager = GameEventManager.Instance;
            this.textMesh.enabled = false;
            this.eventManager.Subscribe(GameEventType.ProblemCompleted, this.ShowChallangeCompletedText);
            this.eventManager.Subscribe(GameEventType.PlayerDied, this.ShowPlayerDiedText);
            this.eventManager.Subscribe(GameEventType.ProblemStarted, this.HideText);
        
        }

        public void ShowChallangeCompletedText(int value)
        {
            this.textMesh.text = "ChallangeCompleted";
            this.textMesh.enabled = true;
        }

        public void ShowPlayerDiedText(int value)
        {
            this.textMesh.text = "Player Died.\nRestart Challange.";
            this.textMesh.enabled = true;
        }

        public void HideText(int value)
        {
            this.textMesh.text = "";
            this.textMesh.enabled = false;
        }
    }
}
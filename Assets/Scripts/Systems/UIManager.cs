using System.Collections.Generic;
using Scripts.GameEvents;
using Scripts.Objectives;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Systems
{
    public class UIManager : MonoBehaviour
    {
        private GameEventManager eventManager;
        private Text textMesh;
        private GameObject objectivesListView;
        private Dictionary<Objective, UnityEngine.UI.Text> objectivesTexts;



        private void Awake()
        {
            this.textMesh = GameObject.Find("StatusText").GetComponent<Text>();
            this.textMesh.enabled = false;

            this.objectivesTexts = new Dictionary<Objective, UnityEngine.UI.Text>();
            this.objectivesListView = GameObject.Find("ObjectivesListView");

            this.eventManager = GameEventManager.Instance;
            this.eventManager.Subscribe(GameEventType.ProblemCompleted, this.ShowChallangeCompletedText);
            this.eventManager.Subscribe(GameEventType.PlayerDied, this.ShowPlayerDiedText);
            this.eventManager.Subscribe(GameEventType.ProblemStarted, this.HideText);
            this.eventManager.Subscribe(GameEventType.ProblemStarted, this.InitUI);
            this.eventManager.Subscribe(GameEventType.ObjectiveUpdated, this.UpdateUI);
        }

        public void ShowChallangeCompletedText(int value)
        {
            this.textMesh.text = "ProblemCompleted";
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

        public void InitUI(int value)
        {
            var objectives = GameController.Objectives;
        
            //clear children in any
            this.objectivesTexts.Clear();
            foreach (Transform transform in this.objectivesListView.transform)
            {
                Object.Destroy(transform.gameObject);
            }
            foreach (var objective in objectives.ObjectiveList)
            {

                GameObject textContainer = new GameObject(objective.Description);
                var rectTransform = textContainer.AddComponent<RectTransform>();
                rectTransform.SetParent(this.objectivesListView.GetComponent<RectTransform>());
                rectTransform.localPosition = Vector3.zero;
                rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
                rectTransform.localScale = new Vector3(1, 1, 1);
                Text textComponent = textContainer.AddComponent<Text>();
                textComponent.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
                textComponent.color = new Color32(35, 190, 255, 255);
                textComponent.text = $"{objective.Description} : {objective.CurrentValue} of {objective.TargetValue}";
                this.objectivesTexts.Add(objective, textComponent);
            }
        }
        private void UpdateUI(int value)
        {
            foreach (var objective in this.objectivesTexts.Keys)
            {
                if (objective.IsComplete())
                {
                    this.objectivesTexts[objective].color = new Color32(168, 224, 65, 255);
                }
                else
                {
                    this.objectivesTexts[objective].color = new Color32(35, 190, 255, 255);
                }
                this.objectivesTexts[objective].text = $"{objective.Description} : {objective.CurrentValue} of {objective.TargetValue}";

            }
        }
    }
}

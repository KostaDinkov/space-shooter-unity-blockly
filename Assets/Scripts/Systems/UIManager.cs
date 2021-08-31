using System.Collections.Generic;
using Scripts.GameEvents;
using Scripts.Objectives;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Scripts.Systems
{
    public class UIManager : MonoBehaviour
    {
        private GameEventManager eventManager;
        private Text textMesh;
        private GameObject objectivesListView;
        private Dictionary<Objective, TMPro.TextMeshProUGUI> objectivesTexts;
        private Button runButton;
        private Button nextButton;



        private void Awake()
        {
            this.textMesh = GameObject.Find("StatusText").GetComponent<Text>();
            this.textMesh.enabled = false;
            
            this.runButton = GameObject.Find("RunBtn").GetComponent<Button>();
            this.nextButton = GameObject.Find("NextBtn").GetComponent<Button>();
            this.nextButton.interactable = false;
            this.objectivesTexts = new Dictionary<Objective, TextMeshProUGUI>();
            this.objectivesListView = GameObject.Find("ObjectivesListView");

            this.eventManager = GameEventManager.Instance;
            this.eventManager.Subscribe(GameEventType.ProblemCompleted, this.ShowProblemCompletedText);
            this.eventManager.Subscribe(GameEventType.PlayerDied, this.ShowPlayerDiedText);
            this.eventManager.Subscribe(GameEventType.ProblemStarted, this.HideText);
            this.eventManager.Subscribe(GameEventType.ProblemStarted, this.InitUI);
            this.eventManager.Subscribe(GameEventType.ObjectiveUpdated, this.UpdateUI);
            this.eventManager.Subscribe(GameEventType.SolutionFailed, this.ShowSolutionFailedText);
            this.eventManager.Subscribe(GameEventType.ScriptStarted, this.OnScriptStarted);
        }

      

        public void OnScriptStarted(int value)
        {
            runButton.interactable = false;
        }

        public void ShowProblemCompletedText(int value)
        {
            this.textMesh.text = "Проблемът е решен!";
            this.textMesh.enabled = true;
            this.nextButton.interactable = true;
        }

        public void ShowPlayerDiedText(int value)
        {
            this.textMesh.text = "Дронът е унищожен.\nРестартирайте проблема.";
            this.textMesh.enabled = true;
        }

        public void ShowSolutionFailedText(int value)
        {
            this.textMesh.text = "Непълно решение.Опитай пак.\nРестартирайте проблема.";
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

                GameObject textContainer = new GameObject(objectives.ObjectiveList.IndexOf(objective).ToString());
                var rectTransform = textContainer.AddComponent<RectTransform>();
                
                rectTransform.SetParent(this.objectivesListView.GetComponent<RectTransform>());
                rectTransform.localPosition = Vector3.zero;
                rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
                rectTransform.localScale = new Vector3(1, 1, 1);
                
                var tmpro = textContainer.AddComponent<TextMeshProUGUI>();
                tmpro.SetText($"{objective.Description} : {objective.CurrentValue} от {objective.TargetValue}");
                tmpro.fontSize = 20;
                tmpro.color = new Color32(35, 190, 255, 255);
                tmpro.font = Resources.Load<TMP_FontAsset>("Fonts/AnkaCoderItalicSDF");
                tmpro.alignment = TextAlignmentOptions.BaselineRight;

                this.objectivesTexts.Add(objective, tmpro);
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

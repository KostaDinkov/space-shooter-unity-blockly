using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Scripts.GameEvents;
using Scripts.Objectives;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Scripts.Systems
{
    public class UIManager : MonoBehaviour
    {
        private GameEventManager eventManager;
        private Text statusText;
        private GameObject objectivesListView;
        private Dictionary<Objective, TMPro.TextMeshProUGUI> objectivesTexts;
        private Button runButton;
        private Button nextButton;
        private TMPro.TextMeshProUGUI infoTitle;
        private TMPro.TextMeshProUGUI problemTitle;
        private TMPro.TextMeshProUGUI infoText;
        [SerializeField]private GameObject infoPanel;

        private void Awake()
        {
            this.statusText = GameObject.Find("StatusText").GetComponent<Text>();
            this.statusText.enabled = false;

            this.infoTitle=GameObject.Find("InfoTitle").GetComponent<TMPro.TextMeshProUGUI>();
            this.infoText = GameObject.Find("InfoText").GetComponent<TMPro.TextMeshProUGUI>();
            this.problemTitle = GameObject.Find("ProblemTitle").GetComponent<TMPro.TextMeshProUGUI>();

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

            //Set next button active if next problem is unlocked
            var nextScene = GameData.Instance.GetNextProblemSceneName();
            var nextProblemState = GameData.Instance.UserProblemStates[nextScene.Substring(0, 3)]
                .FirstOrDefault(p => p.ProblemName == nextScene.Substring(3, 3));
            if (!nextProblemState.ProblemLocked)
            {
                this.nextButton.interactable = true;
            }
        }

      

        public void OnScriptStarted(int value)
        {
            runButton.interactable = false;
        }

        public void ShowProblemCompletedText(int value)
        {
            this.statusText.text = "Проблемът е решен!";
            this.statusText.enabled = true;
            this.nextButton.interactable = true;
        }

        public void ShowPlayerDiedText(int value)
        {
            this.statusText.text = "Дронът е унищожен.\nРестартирайте проблема.";
            this.statusText.enabled = true;
        }

        public void ShowSolutionFailedText(int value)
        {
            this.statusText.text = "Непълно решение.Опитай пак.\nРестартирайте проблема.";
            this.statusText.enabled = true;
        }

        public void HideText(int value)
        {
            this.statusText.text = "";
            this.statusText.enabled = false;
        }

        public void InitUI(int value)
        {

            this.InitObjectives();
            this.InitInfoPanel();
   
        }

        private void InitObjectives()
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
                tmpro.alignment = TextAlignmentOptions.BaselineLeft;

                this.objectivesTexts.Add(objective, tmpro);
            }
        }

        private void InitInfoPanel()
        {
            var sceneName = SceneManager.GetActiveScene().name;
            var levelParams = Regex.Matches(sceneName, @"[0-9]{2}");
            if (levelParams.Count != 2)
            {
                throw new ArgumentException($"Scene name not following convention -l[number]p[number], but instead was {sceneName}");
            }
            var infoTitleText = $"Ниво {int.Parse(levelParams[0].Value)} | Задача {int.Parse(levelParams[1].Value)}";
            
            this.infoTitle.SetText(infoTitleText);
            this.problemTitle.SetText(infoTitleText);
            this.infoText.SetText(GameController.Objectives.ProblemDescription);

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

        public void OpenInfoPanel()
        {
            this.infoPanel.SetActive(true);
        }

        public void CloseInfoPanel()
        {
            this.infoPanel.SetActive(false);
        }
    }
}

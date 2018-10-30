using System.Collections.Generic;
using Assets.Scripts.GameEvents;
using Game.GameEvents;
using Game.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Objectives
{
    /// <summary>
    ///     Keeps track of the completeness of a challenge based on the completed objectives;
    /// </summary>
    public class ObjectivesManager
    {
        private readonly GameEvent challangeCompletedEvent;
        private readonly GameEventManager eventManager;
        private readonly GameEvent objectiveCompletedEvent;
        private Objectives objectives;
        private GameObject objectivesListView;
        private Dictionary<Objective, UnityEngine.UI.Text> objectivesTexts;

        public ObjectivesManager()
        {
            objectivesTexts = new Dictionary<Objective, UnityEngine.UI.Text>();
            
            objectivesListView = GameObject.Find("ObjectivesListView");
            InitUI(0);

            eventManager = GameEventManager.Instance;
            eventManager.Subscribe(GameEventType.ObjectiveCompleted, IsChallangeCompleted);
            eventManager.Subscribe(GameEventType.ChallangeStarted, InitUI);
            eventManager.Subscribe(GameEventType.ObjectiveUpdated, UpdateUI);
        }

        public void InitUI(int value)
        {
            objectives = GameData.Instance.CurrentChallenge.GetComponent<Objectives>();
            InitChallanges(0);
            //clear children in any
            objectivesTexts.Clear();
            foreach (Transform transform in objectivesListView.transform)
            {
                GameObject.Destroy(transform.gameObject);
            }
            foreach (var objective in objectives.ObjectivesList)
            {
            
                GameObject textContainer = new GameObject(objective.Description);
                var rectTransform = textContainer.AddComponent<RectTransform>();
                rectTransform.SetParent(objectivesListView.GetComponent<RectTransform>());
                rectTransform.localPosition = Vector3.zero;
                rectTransform.localRotation = Quaternion.Euler(0,0,0);
                rectTransform.localScale = new Vector3(1,1,1);
                Text textComponent = textContainer.AddComponent<Text>();
                textComponent.font = Font.CreateDynamicFontFromOSFont("Arial",14);
                textComponent.color = new Color32(35,190,255,255);
                textComponent.text = $"{objective.Description} : {objective.CurrentValue} of {objective.TargetValue}";
                objectivesTexts.Add(objective, textComponent);
            }
        }

        private void UpdateUI(int  value)
        {
            foreach (var objective in objectivesTexts.Keys)
            {
                if (objective.IsComplete())
                {
                    objectivesTexts[objective].color = new Color32(168,224,65,255);
                }
                else
                {
                    objectivesTexts[objective].color = new Color32(35, 190, 255, 255);
                }
                objectivesTexts[objective].text = $"{objective.Description} : {objective.CurrentValue} of {objective.TargetValue}";
                
            }
        }

        private void InitChallanges(int value)
        {
            foreach (var objective in objectives.ObjectivesList)
            {
                objective.Init();
            }
            
        }

        private void IsChallangeCompleted(int value)
        {
            foreach (var objective in objectives.ObjectivesList)
            {
                if (!objective.IsComplete())
                {
                    return;
                }
            }

            eventManager.Publish(new GameEvent {EventType = GameEventType.ChallangeCompleted});
        }
    }
}
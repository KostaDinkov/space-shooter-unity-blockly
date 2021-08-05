using System.Collections;
using System.Collections.Generic;
using Game.GameEvents;
using Game.Objectives;
using Game.Systems;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameEventManager eventManager;
    private Text textMesh;
    private GameObject objectivesListView;
    private Dictionary<Objective, UnityEngine.UI.Text> objectivesTexts;



    private void Awake()
    {
        textMesh = GameObject.Find("StatusText").GetComponent<Text>();
        textMesh.enabled = false;

        objectivesTexts = new Dictionary<Objective, UnityEngine.UI.Text>();
        objectivesListView = GameObject.Find("ObjectivesListView");

        eventManager = GameEventManager.Instance;
        eventManager.Subscribe(GameEventType.ProblemCompleted, ShowChallangeCompletedText);
        eventManager.Subscribe(GameEventType.PlayerDied, ShowPlayerDiedText);
        eventManager.Subscribe(GameEventType.ProblemStarted, HideText);
        eventManager.Subscribe(GameEventType.ProblemStarted, InitUI);
        eventManager.Subscribe(GameEventType.ObjectiveUpdated, UpdateUI);
    }

    public void ShowChallangeCompletedText(int value)
    {
        textMesh.text = "ProblemCompleted";
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

    public void InitUI(int value)
    {
        var objectives = GameController.Objectives;
        
        //clear children in any
        objectivesTexts.Clear();
        foreach (Transform transform in objectivesListView.transform)
        {
            Object.Destroy(transform.gameObject);
        }
        foreach (var objective in objectives.ObjectiveList)
        {

            GameObject textContainer = new GameObject(objective.Description);
            var rectTransform = textContainer.AddComponent<RectTransform>();
            rectTransform.SetParent(objectivesListView.GetComponent<RectTransform>());
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
            rectTransform.localScale = new Vector3(1, 1, 1);
            Text textComponent = textContainer.AddComponent<Text>();
            textComponent.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
            textComponent.color = new Color32(35, 190, 255, 255);
            textComponent.text = $"{objective.Description} : {objective.CurrentValue} of {objective.TargetValue}";
            objectivesTexts.Add(objective, textComponent);
        }
    }
    private void UpdateUI(int value)
    {
        foreach (var objective in objectivesTexts.Keys)
        {
            if (objective.IsComplete())
            {
                objectivesTexts[objective].color = new Color32(168, 224, 65, 255);
            }
            else
            {
                objectivesTexts[objective].color = new Color32(35, 190, 255, 255);
            }
            objectivesTexts[objective].text = $"{objective.Description} : {objective.CurrentValue} of {objective.TargetValue}";

        }
    }
}

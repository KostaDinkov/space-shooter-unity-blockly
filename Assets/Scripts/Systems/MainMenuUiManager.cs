
using System.Linq;
using Scripts.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUiManager : MonoBehaviour
{
    // Start is called before the first frame update
    private GameData gameData;
    private GameObject levelsGroup;

    void Awake()
    {
        this.gameData = GameData.Instance;
        
    }
    void Start()
    {
        this.levelsGroup = GameObject.Find("levels");
        this.UpdateLevels();
    }

    

    private void UpdateLevels()
    {
        var sortedProblemStates = this.gameData.UserProblemStates.GroupBy(p => p.LevelName).OrderBy(p => p.Key).ToList();
        foreach (var problemStates in sortedProblemStates)
        {

            var levelTitle = new GameObject(problemStates.Key);
            var rectTranform = levelTitle.AddComponent<RectTransform>();
            rectTranform.sizeDelta = new Vector2(700,75);
            
            var textComponent = levelTitle.AddComponent<TextMeshProUGUI>();
            textComponent.SetText(this.gameData.LevelNames[problemStates.Key]);
            textComponent.fontSize = 36; 
            
            var horLayoutGroup = levelTitle.AddComponent<HorizontalLayoutGroup>();
            horLayoutGroup.padding = new RectOffset(30, 0, 0, 0);
            horLayoutGroup.childAlignment = TextAnchor.LowerLeft;
            horLayoutGroup.childForceExpandHeight = true;
            horLayoutGroup.childForceExpandWidth = false;
            horLayoutGroup.spacing = 5;
            horLayoutGroup.childControlHeight = false;
            horLayoutGroup.childControlWidth = false;

            levelTitle.transform.parent = this.levelsGroup.transform;

            foreach (var state in problemStates)
            {
                var button = Resources.Load<GameObject>("Prefabs/MainMenu/problemBtn");
                button.name = state.ProblemName;
                var rectTransform = button.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(25,25);
                Button btn = button.GetComponent<Button>();
                btn.enabled = true;
                var colorBlock = new ColorBlock()
                {
                    normalColor = new Color(1, 1, 1),
                    disabledColor = new Color(0.9f, 0.9f, 0.9f, 0.5f),
                    colorMultiplier = 1,
                    fadeDuration = 0.1f
                };

                btn.colors = colorBlock;
                if (state.ProblemLocked)
                {
                    btn.interactable = false;
                }
                else
                {
                    btn.interactable = true;
                    colorBlock.normalColor = new Color(0.3716f,0.8975f, 1,1);
                    btn.colors = colorBlock;
                }
                
                Instantiate(button, Vector3.zero, Quaternion.identity, levelTitle.transform);
            }
        }
    }
}

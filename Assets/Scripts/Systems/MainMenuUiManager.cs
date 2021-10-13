
using Scripts.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUiManager : MonoBehaviour
{
    // Start is called before the first frame update
    private GameData gameData;
    private GameObject levelsGroup;
    private string selectedScene;
    private Button loadSelectedBtn;

    void Awake()
    {
        this.gameData = GameData.Instance;

        
    }
    void Start()
    {
        this.loadSelectedBtn = GameObject.Find("LoadSelected").GetComponent<Button>(); 
        this.loadSelectedBtn.interactable = false;
        this.levelsGroup = GameObject.Find("levels");
        this.UpdateLevels();
    }

   private void UpdateLevels()
    {
        foreach (var problemStates in this.gameData.UserProblemStates)
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

            levelTitle.transform.SetParent(this.levelsGroup.transform);
            
            foreach (var state in problemStates.Value)
            {
                var button = Resources.Load<GameObject>("Prefabs/MainMenu/problemBtn");
                button.name = state.ProblemName;
                //button.GetComponent<MainMenuProblemBtn>().SceneName = state.LevelName+state.ProblemName;
                

                var rectTransform = button.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(25,25);
                Button btn = button.GetComponent<Button>();
                
                btn.enabled = true;
                var colorBlock = new ColorBlock()
                {
                    normalColor = new Color(1, 1, 1),
                    disabledColor = new Color(0.9f, 0.9f, 0.9f, 0.5f),
                    highlightedColor = new Color(1f,1f,1f),
                    pressedColor = new Color(0.7113208f, 0.9531364f,1,1),
                    selectedColor = new Color(1,0.7653183f,1),
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
                
                var btnInstance = Instantiate(button, Vector3.zero, Quaternion.identity, levelTitle.transform);
                
               btnInstance.GetComponent<Button>().onClick.AddListener(() => this.ProblemBtnClick(state.LevelName+state.ProblemName));

            }
        }
    }

    public void ProblemBtnClick(string sceneName)
    {
        this.selectedScene = sceneName;
        this.loadSelectedBtn.interactable = true;
        //Debug.Log(sceneName);
    }

    public void LoadSelectedScene()
    {
        SceneManager.LoadScene(this.selectedScene);
    }
}

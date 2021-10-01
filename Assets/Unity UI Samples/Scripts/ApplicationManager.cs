using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AzureSqlDbConnect;
using Scripts.Systems;
using UnityEngine.SceneManagement;

public class ApplicationManager : MonoBehaviour
{
    private GameData gameData;

    private void Awake()
    {
        this.gameData = GameData.Instance;
        this.gameData.dbApi = new DbApi(new GameDbContext());
        var dbApi = this.gameData.dbApi;
        if (!dbApi.UserExists(this.gameData.Username))
        {
            var allProblems = this.GetAllProblems();
            dbApi.CreateUser(this.gameData.Username);
            dbApi.NewUserProblemStateInit(this.gameData.Username, allProblems);
        }

        this.gameData.UserProblemStates = dbApi.GetAllProblemStates(this.gameData.Username);
    }

    private Dictionary<string, List<string>> GetAllProblems()
    {
        var problems = new Dictionary<string, List<string>>();

        var sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            var scene = SceneUtility.GetScenePathByBuildIndex(i);
            var sceneName = scene;
            var match = Regex.Match(sceneName, @"[\/|\\]([l|L]{1}[0-9]{2})([p|P]{1}[0-9]{2})\.unity$");
            if (match.Success)
            {
                var levelName = match.Groups[1].Value;
                var problemName = match.Groups[2].Value;
                if (problems.ContainsKey(levelName))
                {
                    problems[levelName].Add(problemName);
                }
                else
                {
                    problems.Add(levelName, new List<string>());
                    problems[levelName].Add(problemName);
                }
            }
        }

        return problems;
    }

    public void Continue()
    {
        SceneManager.LoadScene(this.gameData.LastUnlockedProblem);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }
}
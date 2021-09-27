using UnityEngine;
using System.Collections;
using Scripts.Systems;
using UnityEngine.SceneManagement;

public class ApplicationManager : MonoBehaviour
{
    private GameData gameData;

    private void Awake()
    {
		this.gameData = GameData.Instance;
    }

    public void Continue()
    {
        SceneManager.LoadScene(this.gameData.LastUnlockedProblem);
    }
	public void Quit () 
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}

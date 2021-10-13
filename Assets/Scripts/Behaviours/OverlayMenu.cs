using UnityEngine;
using UnityEngine.SceneManagement;

public class OverlayMenu : MonoBehaviour
{
    public void LoadMainMenu()
    {
        //TODO fix hardcoded scene index
        //SceneManager.MoveGameObjectToScene(GameObject.Find("BrowserCanvas"), SceneManager.GetActiveScene());
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }
}

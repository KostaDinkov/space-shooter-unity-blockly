using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Game.Systems;
using UnityEngine.SceneManagement;

public class GameControllerTests
{
    private Scene scene;
    private GameController gameController;
    private GameData gameData;


    [OneTimeSetUp]
    public void Init()
    {

        
        SceneManager.LoadScene(0);
        this.scene = SceneManager.GetActiveScene();
        
        
    }

   
    [UnityTest]
    public IEnumerator ShouldHaveGameObjects()
    {   
        // Use the Assert class to test conditions.
        yield return null;
        var gameObjects = scene.GetRootGameObjects();
        Debug.Log("Objects in scene:");
        foreach (var gameObject in gameObjects)
        {
            
            Debug.Log(gameObject.name);
        }
        Assert.IsNotEmpty(gameObjects,"No game objects in scene");

    }

   


}

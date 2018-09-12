
//Play Mode tests file

using System;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Game.Systems;
using UnityEngine.SceneManagement;

public class GameControllerTests
{
    
    private GameObject gameControllerObj;
    private GameController gameController;
    private GameData gameData;
    
    [OneTimeSetUp]
    public void Init()
    {
        var sharedObjects = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/SharedObjects"));
        this.gameControllerObj = sharedObjects.transform.Find("Game Controller").gameObject;
        this.gameController = gameControllerObj.GetComponent<GameController>();
        this.gameData = GameData.Instance;

    }
    
    [UnityTest]
    public IEnumerator StartNthChallenge_CorrectIndices_shouldLoadNthChallenge()
    {
        Assert.True(gameData.CurrentChallenge.name.Contains(gameController.Challenges[0].name));
        yield return new WaitForSeconds(3);

        gameController.StartNthChallenge(1);
        Assert.True(gameData.CurrentChallenge.name.Contains(gameController.Challenges[1].name));
        yield return new WaitForSeconds(3);

        gameController.StartNthChallenge(2);
        Assert.True(gameData.CurrentChallenge.name.Contains(gameController.Challenges[2].name));
        yield return new WaitForSeconds(3);
    }

    [UnityTest]
    public IEnumerator StartNthChallenge_IncorrectIndices_shouldDoNothing()
    {
        gameController.StartNthChallenge(0);

        gameController.StartNthChallenge(5);
        Assert.True(gameData.CurrentChallenge.name.Contains(gameController.Challenges[0].name));
        yield return new WaitForSeconds(3);

        gameController.StartNthChallenge(-1);
        Assert.True(gameData.CurrentChallenge.name.Contains(gameController.Challenges[0].name));
        yield return new WaitForSeconds(3);
    }

    [UnityTest]
    public IEnumerator Start_ShouldUpdateGameDataCorrectly()
    {
        Assert.AreEqual(gameController.Challenges.Length, gameData.ChallengeCount);
        yield return null;
    }




}

using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using Scripts.Systems;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class BasicMechanicsTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void PlayerCollisionShouldMakePlayerDie()
        {
           
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator BasicMechanicsTestsWithEnumeratorPasses()
        {
            Debug.Log(SceneManager.sceneCountInBuildSettings);
            AsyncOperation loadScene =  SceneManager.LoadSceneAsync(0);

            while (!loadScene.isDone)
            {
                yield return null;
            }
            var player = GameObject.Find("Player-Drone").GetComponent<Playercontroller>();
            var game = GameObject.Find("GameController").GetComponent<GameController>();

            player.MoveForwardAsync();
            Assert.IsTrue(!game.IsPlayerDead);

            

            
        }
    }
}
using System;
using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using Scripts.Exceptions;
using Scripts.Systems;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class Problem1BasicTests
    {
        private Playercontroller player;

        private GameController game;

        // A Test behaves as an ordinary method
        [Test]
        public void PlayerCollisionShouldMakePlayerDie()
        {
        }

        [SetUp]
        public async void Setup()
        {
            SceneManager.LoadScene(0);
            await UniTask.NextFrame();
            this.player = GameObject.Find("Player-Drone").GetComponent<Playercontroller>();
            this.game = GameObject.Find("GameController").GetComponent<GameController>();
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator PlayerShouldDieOnCollision()
        {
            var controlTask = MovePlayerToCollide(player);
            while (controlTask.Status == UniTaskStatus.Pending)
            {
                yield return null;
            }

            Assert.IsTrue(game.IsPlayerDead);
        }

        [UnityTest]
        public IEnumerator PlayerShouldNotDieOnPlatform()
        {
            var controlTask = MovePlayerToPlatform(player);
            while (controlTask.Status == UniTaskStatus.Pending)
            {
                yield return null;
            }

            Assert.IsTrue(!game.IsPlayerDead);
        }

        [UnityTest]
        public IEnumerator CommandAfterPlayerDiedShouldThrow()
        {

            IEnumerator result;
            var task = CommandsAfterPlayerDies(player);
            do
            {
                result = AsyncTest.Execute(task);
                yield return result;
               
            }
            while((result.Current) == null);
            
            Assert.True(typeof(PlayerDiedException) == result.Current.GetType() );
            


        }

        private async Task<Exception> CommandsAfterPlayerDies(Playercontroller player)
        {
            try
            {
                await player.MoveForwardAsync();
                await player.MoveForwardAsync();
                await player.MoveForwardAsync();
                await player.MoveForwardAsync();
                await player.MoveForwardAsync();
                throw new NotImplementedException();
            }
            
            catch (Exception ex)
            {
                return ex;
            }
            
        }

        private async UniTask MovePlayerToPlatform(Playercontroller player)
        {
            await player.RotateLeftAsync();
            await player.MoveForwardAsync();
            await player.RotateRightAsync();
            await player.MoveForwardAsync();
            await player.MoveForwardAsync();
            await player.MoveForwardAsync();
            await player.RotateLeftAsync();
            await player.MoveForwardAsync();
            await player.MoveForwardAsync();
            await player.RotateRightAsync();
            await player.MoveForwardAsync();
            await player.MoveForwardAsync();
            await player.MoveForwardAsync();
            await player.MoveForwardAsync();
            await player.RotateRightAsync();
            await player.MoveForwardAsync();
            await player.MoveForwardAsync();
            await player.MoveForwardAsync();
        }

        private async UniTask MovePlayerToCollide(Playercontroller player)
        {
            await player.MoveForwardAsync();
            await player.MoveForwardAsync();
        }
    }
}
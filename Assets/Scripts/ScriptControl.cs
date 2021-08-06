using System.Threading.Tasks;
using Scripts.Systems;
using Scripts.Systems;
using Scripts.Exceptions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class ScriptControl : MonoBehaviour
    {
        // Start is called before the first frame update
        public Playercontroller player;
        public GameController GameController;
    
        async void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            this.GameController = GameObject.Find("GameController").GetComponent<GameController>();
            Assert.raiseExceptions = false;
        
            try
            {
                switch (SceneManager.GetActiveScene().buildIndex)
                {
                    case 0: 
                        //Test for successful option
                        await this.P1Solution();
                        Assert.IsTrue(this.GameController.IsProblemComplete);
                        this.GameController.RestartChallenge();

                        //Test for player died
                        await this.P1PlayerDead();

                        break;
                    case 1: this.P2Solution();
                        break;
                }

            }
            catch (PlayerDiedException ex) 
            {
                Debug.Log(ex.Message);
            }
            //result = await this.player.PickupObject();
            //Debug.Log($"<color=orange>Pickup:</color> {result}");

            //await this.player.RotateLeftAsync();

            //var cargoBay = this.player.GetCargo();
            //for (int i = 0; i < cargoBay.Length; i++)
            //{
            //    if (cargoBay[i] != "null")
            //    {
            //        result = await this.player.UnloadCargoAt(i);
            //        Debug.Log($"<color=orange>Unload cargo:</color> {result}");
            //        break;

            //    }
            //}


        }

        private async Task P1Solution()

        {
            await this.player.RotateLeftAsync();
            await this.player.MoveForwardAsync();
            await this.player.MoveForwardAsync();
            await this.player.RotateRightAsync();

            await this.player.FireWeaponAsync();
            await this.player.FireWeaponAsync();
            await this.player.MoveForwardAsync();
            await this.player.MoveForwardAsync();
            await this.player.MoveForwardAsync();
            await this.player.MoveForwardAsync();
            await this.player.MoveForwardAsync();
            await this.player.MoveForwardAsync();
            await this.player.MoveForwardAsync();

            await this.player.RotateRightAsync();
            await this.player.MoveForwardAsync();
            await this.player.MoveForwardAsync();
        

        }

        private async Task P1PlayerDead()
        {
            await this.player.MoveForwardAsync();
            await this.player.MoveForwardAsync();
        }

        private async void P2Solution()
        {
            await this.player.RotateLeftAsync();
            await this.player.MoveForwardAsync();
            await this.player.MoveForwardAsync();
            await this.player.RotateRightAsync();

            await this.player.MoveForwardAsync();
            await this.player.MoveForwardAsync();
            await this.player.MoveForwardAsync();
            await this.player.MoveForwardAsync();

            await this.player.RotateRightAsync();
            await this.player.MoveForwardAsync();
            await this.player.MoveForwardAsync();
            await this.player.RotateLeftAsync();


            await this.player.MoveForwardAsync();
            await this.player.MoveForwardAsync();
            await this.player.MoveForwardAsync();
        }


    }
}

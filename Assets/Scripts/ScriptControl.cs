using System.Threading.Tasks;
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

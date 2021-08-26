using System.Threading.Tasks;
using Scripts.Systems;
using UnityEngine;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Scripts
{
    public class ScriptControl : MonoBehaviour
    {
        // Start is called before the first frame update
        public Playercontroller player;
        public GameController GameController;

        public class Globals
        {
            public Playercontroller Player;
        }

        async void Start()
        {
            var globals = new Globals() {Player = this.player};
            await CSharpScript.EvaluateAsync(
                "await Player.MoveForwardAsync(); await Player.RotateLeftAsync();await Player.MoveForwardAsync();",
                ScriptOptions.Default
                    .WithImports("UnityEngine")
                    .WithReferences(typeof(UnityEngine.MonoBehaviour).Assembly),
                globals: globals);
            
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
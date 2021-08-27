using System;
using System.Threading.Tasks;
using Scripts.Systems;
using UnityEngine;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using ZenFulcrum.EmbeddedBrowser;

namespace Scripts
{
    public class ScriptControl : MonoBehaviour
    {
        // Start is called before the first frame update
        public Playercontroller player;
        public BrowserController BrowserController;
        public GameController GameController;
        private Browser browser;
        public class Globals
        {
            public Playercontroller Player;
        }

        void Start()
        {
            this.browser = this.BrowserController.GetComponent<Browser>();
            
        }

        public void RunCode()
        {
             this.browser.CallFunction("getCode").Then(async res =>
             {
                 string code = (string)res.Value;
                 var globals = new Globals() { Player = this.player };
                 await CSharpScript.EvaluateAsync(
                     code,
                     ScriptOptions.Default
                         .WithImports("UnityEngine")
                         .WithReferences(typeof(UnityEngine.MonoBehaviour).Assembly),
                     globals: globals);
             }).Done();
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
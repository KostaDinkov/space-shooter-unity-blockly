using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Commands;
using Game.Systems;
using UnityEngine;

public class ScriptControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Playercontroller player;
    
    async void Start()
    {
        // Test sequence;
        var result = await this.player.MoveForward();
        Debug.Log($"After awaiting result: {result}");
        result = await this.player.MoveForward();
        Debug.Log($"After awaiting result: {result}");
        result = await this.player.RotateLeft();
        Debug.Log($"After awaiting result: {result}");
        result = await this.player.MoveForward();
        Debug.Log($"After awaiting result: {result}");
        result = await this.player.ScanAhead();
        Debug.Log($"After awaiting result: {result}");
    }

    private async Task<string> ScanTest()
    {
        await Task.Delay(3000);
        Debug.Log("Working in scanTest ...");
        var sceneName = this.gameObject.scene.name;
        Debug.Log(sceneName);
        return "result from task";
    }

    // Update is called once per frame
    async void  Update()
    {
      
        
    }
}

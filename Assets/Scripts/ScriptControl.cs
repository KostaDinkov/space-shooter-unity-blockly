using Game.Systems;
using UnityEngine;

public class ScriptControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Playercontroller player;
    
    async void Start()
    {

        string result;
        // Test sequence;
        //result = await this.player.MoveForwardAsync();
        //Debug.Log($"After awaiting result: {result}");
        
        //result = await this.player.MoveForwardAsync();
        //Debug.Log($"After awaiting result: {result}");
        
        //result = await this.player.RotateLeftAsync();
        //Debug.Log($"After awaiting result: {result}");
        
        //result = await this.player.MoveForwardAsync();
        //Debug.Log($"<color=orange>After awaiting result:</color> {result}");

        //result = await this.player.ScanAheadAsync();
        //Debug.Log($"After awaiting result: {result}");
        

        result = await this.player.PickupObject();
        Debug.Log($"<color=orange>Pickup:</color> {result}");

        await this.player.RotateLeftAsync();
        await this.player.MoveForwardAsync();

        var cargoBay = this.player.GetCargo();
        for (int i = 0; i < cargoBay.Length; i++)
        {
            if (cargoBay[i] != "null")
            {
                result = await this.player.UnloadCargoAt(i);
                Debug.Log($"<color=orange>Unload cargo:</color> {result}");
                break;

            }
        }
        

    }


}

using Game.Systems;
using UnityEngine;

public class ScriptControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Playercontroller player;
    
    async void Start()
    {
        // Test sequence;
        var result = await this.player.MoveForwardAsync();
        Debug.Log($"After awaiting result: {result}");
        result = await this.player.MoveForwardAsync();
        Debug.Log($"After awaiting result: {result}");
        result = await this.player.RotateLeftAsync();
        Debug.Log($"After awaiting result: {result}");
        result = await this.player.MoveForwardAsync();
        Debug.Log($"<color=orange>After awaiting result:</color> {result}");
        result = await this.player.ScanAheadAsync();
        Debug.Log($"After awaiting result: {result}");
        result = await this.player.FireWeaponAsync();
        Debug.Log($"After awaiting result: {result}");
    }


}

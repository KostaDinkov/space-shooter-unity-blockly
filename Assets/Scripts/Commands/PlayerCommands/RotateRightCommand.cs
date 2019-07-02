using Game.Systems;
using UnityEngine;
namespace Game.Commands.PlayerCommands

{
  public class RotateRightCommand : PlayerCommand, ICommand
  {
    private Quaternion degrees;
    private float speed;
    public RotateRightCommand(Playercontroller playercontroller, float degrees, float speed ):base(playercontroller)
    {
      this.degrees = Quaternion.Euler(0, degrees, 0);
      this.speed = speed;
    }

    public  override void Execute()
    {
      playercontroller.StartCoroutine(playercontroller.RotateOverSpeedCoroutine(playercontroller.gameObject, this.degrees, speed));
    }
  }
}
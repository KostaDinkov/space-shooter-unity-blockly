
using Game.Systems;

namespace Game.Commands.PlayerCommands
{
  public class FireWeaponCommand : PlayerCommand
  {
    public FireWeaponCommand(Playercontroller playerController) : base(playerController)
    {
    }

    public override void Execute()
    {
      playercontroller.FireWeapon();
    }
  }
}

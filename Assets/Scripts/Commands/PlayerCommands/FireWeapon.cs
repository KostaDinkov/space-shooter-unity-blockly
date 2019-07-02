
using Game.Systems;

namespace Game.Commands.PlayerCommands
{
  public class FireWeapon : Command
  {
    public FireWeapon(Playercontroller playerController) : base(playerController)
    {
    }

    public override void Execute()
    {
      ((Playercontroller)this.receiver).FireWeapon();
    }
  }
}

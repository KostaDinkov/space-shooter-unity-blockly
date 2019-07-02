using Game.Systems;

namespace Game.Commands.PlayerCommands
{
  public class MoveForward : Command
  {
    public MoveForward(Playercontroller playerController) : base(playerController)
    {
    }

    public override void Execute()
    {
      ((Playercontroller)this.receiver).MoveForwardProcedure();
    }
  }
}

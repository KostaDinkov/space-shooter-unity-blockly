using Game.Systems;
namespace Game.Systems.GameEvents.Commands.PlayerCommands
{
    public class RotateRight : Command
    {
        public RotateRight(Playercontroller playerController) : base(playerController)
        {
        }

        public override void Execute()
        {
            ((Playercontroller) this.receiver).RotateRightProcedure();
        }
    }
}
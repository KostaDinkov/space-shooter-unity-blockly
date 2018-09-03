using Game.Systems;
namespace Game.Commands.PlayerCommands
{
    public class RotateLeft : Command
    {
        public RotateLeft(Playercontroller playerController) : base(playerController)
        {
        }

        public override void Execute()
        {
            ((Playercontroller)this.receiver).RotateLeftProcedure();
        }
    }
}

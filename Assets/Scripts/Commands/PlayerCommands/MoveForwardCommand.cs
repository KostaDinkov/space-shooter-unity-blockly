using Game.Systems;

namespace Game.Commands.PlayerCommands
{
  public class MoveForwardCommand : PlayerCommand
  {
    private int distance;
    private float speed;
    public MoveForwardCommand(Playercontroller playerController, int distance, float speed) : base(playerController)
    {
      this.distance = distance;
      this.speed = speed;
    }

    public override void Execute()
    {
      this.playercontroller.StartCoroutine(playercontroller.MoveForwardCoroutine(playercontroller.gameObject, distance, speed));
    }

    
  }
}

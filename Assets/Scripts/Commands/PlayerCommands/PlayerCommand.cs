using Game.Systems;
using Game.Commands;

namespace Game.Commands.PlayerCommands
{
  public abstract class PlayerCommand: ICommand
  {
    public Playercontroller playercontroller;
    
    public PlayerCommand(Playercontroller playercontroller){
      this.playercontroller = playercontroller;
    }

    public abstract void Execute();
    
  }
}
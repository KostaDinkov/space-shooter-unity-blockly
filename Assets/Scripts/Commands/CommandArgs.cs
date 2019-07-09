namespace Game.Commands
{
  public class CommandArgs : ICommandArgs

  {
    public float Degrees { get;set; }
    public float Speed { get; set; }
    public int Distance { get; set; }
  }
}
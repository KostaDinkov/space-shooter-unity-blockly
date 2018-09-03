namespace Game.Events
{
  public class NewChallangeStarted
  {
    public NewChallangeStarted(int challengeNumber)
    {
      this.ChallengeNumber = challengeNumber;
    }
    public int ChallengeNumber { get; private set; }
  };

  internal class LevelCompleted { }

  public class ChallengeCompleted { }

}

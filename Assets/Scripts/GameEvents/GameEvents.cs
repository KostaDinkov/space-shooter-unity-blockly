namespace Game.Systems.GameEvents
{
    
    public class SceneLoaded
    {
    }

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

    public class PlayerDied { }

    public class ObjectiveCompleted { }

    public class DestinationReached { }

    
}
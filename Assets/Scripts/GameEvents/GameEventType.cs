using System;

namespace Scripts.GameEvents
{
    [Serializable]
    public enum GameEventType
    {
        LevelStarted,
        ProblemStarted,
        ObjectiveCompleted,
        ObjectiveUpdated,
        ProblemCompleted,
        SolutionFailed,
        ScriptStarted,
        LevelCompleted,
        TargetReached,
        PlayerDied,
        PlayerHit,
        EnemyDestroyed,
        TargetPickedUp,
        AchievementEarned,
        AsteroidDestroyed,
        ContainerDestroyed,
        MineExploded,
        BlocksUpdated,
        Null,
        PrintCalled
    }
}
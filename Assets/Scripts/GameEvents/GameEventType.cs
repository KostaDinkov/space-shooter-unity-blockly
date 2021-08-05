using System;

namespace Game.GameEvents
{
    [Serializable]
    public enum GameEventType
    {
        LevelStarted,
        ProblemStarted,
        ObjectiveCompleted,
        ObjectiveUpdated,
        ProblemCompleted,
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
    }
}
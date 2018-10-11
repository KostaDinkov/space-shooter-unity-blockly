using System;

namespace Game.GameEvents
{
    [Serializable]
    public enum GameEventType
    {
        LevelStarted,
        ChallangeStarted,
        ObjectiveCompleted,
        ChallangeCompleted,
        LevelCompleted,
        TargetReached,
        PlayerDied,
        PlayerHit,
        EnemyDestroyed,
        TargetPickedUp,
        AchievementEarned,
     }
}
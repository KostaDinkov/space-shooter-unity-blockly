using Assets.Scripts.GameEvents;
using Game.GameEvents;
using UnityEngine;

namespace Game.Objectives
{
    /// <summary>
    ///     Base Objective class.
    /// </summary>
    [CreateAssetMenu]
    internal class Objective : ScriptableObject
    {
        public int DefaultValue;
        public string Description;
        private GameEventManager eventManager;

        [Tooltip("The Event that will triger this objective to update it's state.")]
        public GameEventType ListenEvent;

        public int TargetValue;
        public int CurrentValue { get; private set; }


        public void Init()
        {
            CurrentValue = DefaultValue;
            eventManager = GameEventManager.Instance;
            eventManager.Subscribe(ListenEvent, ObjectiveUpdated);
        }

        private void ObjectiveUpdated(int value)
        {
            CurrentValue += value;
            if (IsComplete())
            {
                eventManager.Publish(new GameEvent {EventType = GameEventType.ObjectiveCompleted});
            }

            eventManager.Publish(new GameEvent {EventType = GameEventType.ObjectiveUpdated});
        }

        public bool IsComplete()
        {
            return CurrentValue >= TargetValue;
        }
    }
}
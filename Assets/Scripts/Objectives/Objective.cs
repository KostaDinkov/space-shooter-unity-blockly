using System;
using Assets.Scripts.GameEvents;
using Game.GameEvents;
using UnityEngine;

namespace Game.Objectives
{
    /// <summary>
    ///     Base Objective class.
    /// </summary>
    [Serializable]
    public class Objective 
    {
        public string Description;
        public int DefaultValue;
        public int TargetValue;
        [Tooltip("The Event that will triger this objective to update it's state.")]
        public GameEventType ListenEvent;

        public int CurrentValue;
        
        private GameEventManager eventManager;
        

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
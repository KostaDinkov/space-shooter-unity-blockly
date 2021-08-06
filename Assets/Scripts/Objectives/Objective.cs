using System;
using Scripts.GameEvents;
using UnityEngine;

namespace Scripts.Objectives
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
            this.CurrentValue = this.DefaultValue;
            this.eventManager = GameEventManager.Instance;
            this.eventManager.Subscribe(this.ListenEvent, this.ObjectiveUpdated);
        }

        private void ObjectiveUpdated(int value)
        {
            this.CurrentValue += value;
            if (this.IsComplete())
            {
                this.eventManager.Publish(new GameEvent {EventType = GameEventType.ObjectiveCompleted});
            }

            this.eventManager.Publish(new GameEvent {EventType = GameEventType.ObjectiveUpdated});
        }

        public bool IsComplete()
        {
            return this.CurrentValue >= this.TargetValue;
        }
    }
}
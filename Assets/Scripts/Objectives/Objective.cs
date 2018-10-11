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
        private int currentValue;
        public int DefaultValue;
        public string Description;
        private GameEventManager eventManager;

        [Tooltip("The Event that will triger this objective to update it's state.")]
        public GameEvent ListenEvent;

        public int TargetValue;


        public void Init()
        {
            this.currentValue = this.DefaultValue;
            this.eventManager = GameEventManager.Instance;
            this.eventManager.Subscribe(this.ListenEvent, this.ObjectiveUpdated);
        }

        private void ObjectiveUpdated()
        {
            this.currentValue += 1;
            if (this.IsComplete())
            {
                this.eventManager.Publish(new GameEvent {EventType = GameEventType.ObjectiveCompleted});
            }
        }

        public bool IsComplete()
        {
            return this.currentValue >= this.TargetValue;
        }
    }
}
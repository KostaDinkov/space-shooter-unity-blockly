using System;
using System.Globalization;
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
        public string DefaultValue;
        public string TargetValue;
        [Tooltip("The Event that will triger this objective to update it's state.")]
        public GameEventType ListenEvent;

        public string CurrentValue;
        
        private GameEventManager eventManager;
        

        public void Init()
        {
            this.CurrentValue = this.DefaultValue;
            this.eventManager = GameEventManager.Instance;
            this.eventManager.Subscribe(this.ListenEvent, this.ObjectiveUpdated);
        }

        private void ObjectiveUpdated(object args)
        {
            if (IsNumber(args))
            {
                var num = float.Parse(args.ToString());
                this.CurrentValue = (float.Parse(this.CurrentValue)+ num).ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                this.CurrentValue = args.ToString();
            }
            
            if (this.IsComplete())
            {
                this.eventManager.Publish(new GameEvent {EventType = GameEventType.ObjectiveCompleted});
            }
            this.eventManager.Publish(new GameEvent {EventType = GameEventType.ObjectiveUpdated});
        }

        public bool IsComplete()
        {
            if (IsNumber(this.CurrentValue))
            {
                return float.Parse(this.CurrentValue) >= float.Parse(this.TargetValue);
            }

            return this.CurrentValue == this.TargetValue;

        }

        private bool IsNumber(object arg)
        {
            
            float num;
            var isNum = float.TryParse( arg.ToString(), out num);
            return isNum;
        }
    }
}
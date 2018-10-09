using System;
using Assets.Scripts.GameEvents;
using Easy.MessageHub;
using Game.GameEvents;
using Game.Systems.GameEvents;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Objectives
{
    /// <summary>
    /// Base Objective class.
    /// </summary>
    /// 
    [CreateAssetMenu]
    class Objective : ScriptableObject
    {
        public string Description;
        public int CurrentValue;
        public int TargetValue;
        public GameEvent FireEvent;
        public GameEvent ListenEvent;
        private GameEventManager eventManager;


        public void Init()
        {
            this.eventManager = GameEventManager.Instance;
            this.eventManager.Subscribe(ListenEvent, ObjectiveUpdated);
        }

        private void ObjectiveUpdated()
        {
            this.CurrentValue += 1;
            if (this.IsComplete())
            {
                this.eventManager.Publish(FireEvent);

                return;
            }
        }

        public bool IsComplete()
        {
            return this.CurrentValue >= this.TargetValue;
        }
    }
}
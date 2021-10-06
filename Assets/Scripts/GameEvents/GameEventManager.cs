using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.GameEvents
{
    internal class GameEventManager
    {
        private static GameEventManager instance;
        private readonly Dictionary<GameEventType, Dictionary<string, Action<object>>> events;

        private GameEventManager()
        {
            if (instance == null) instance = this;

            this.events = new Dictionary<GameEventType, Dictionary<string, Action<object>>>();
            foreach (var gameEventType in (GameEventType[]) Enum.GetValues(typeof(GameEventType)))
            {
                this.events.Add(gameEventType, new Dictionary<string, Action<object>>());
            }
        }

        public static GameEventManager Instance
        {
            get
            {
                if (instance == null) instance = new GameEventManager();

                return instance;
            }
        }

        public void Publish(GameEvent gameEvent)
        {
            Debug.Log($"Event fired {gameEvent.EventType.ToString()}");
            foreach (var action in this.events[gameEvent.EventType].Values)
            {
                action.Invoke(gameEvent.EventArgs);
            }

            
        }

        public string Subscribe(GameEventType gameEventType, Action<object> action)
        {
            var token = action.Method.Name;
            if (this.events[gameEventType].ContainsKey(token))
            {
                this.events[gameEventType][token] = action;
                return token;
            }

            this.events[gameEventType].Add(token, action);
           
            return token;
        }
        
        public void Unsubscribe(GameEventType gameEventType, string token)
        {
            this.events[gameEventType].Remove(token);
            
        }

        
    }
}
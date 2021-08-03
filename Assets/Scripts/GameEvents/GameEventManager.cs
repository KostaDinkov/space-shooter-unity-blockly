using System;
using System.Collections.Generic;
using Assets.Scripts.GameEvents;
using UnityEngine;


namespace Game.GameEvents
{
    internal class GameEventManager
    {
        private static GameEventManager instance;
        private readonly Dictionary<GameEventType, Dictionary<string, Action<int>>> events;

        private GameEventManager()
        {
            if (instance == null) instance = this;

            this.events = new Dictionary<GameEventType, Dictionary<string, Action<int>>>();
            foreach (var gameEventType in (GameEventType[]) Enum.GetValues(typeof(GameEventType)))
            {
                this.events.Add(gameEventType, new Dictionary<string, Action<int>>());
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
                action.Invoke(gameEvent.EventValue);
            }

            
        }

        public string Subscribe(GameEventType gameEventType, Action<int> action)
        {
            var token = action.Method.Name;
            if (events[gameEventType].ContainsKey(token))
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
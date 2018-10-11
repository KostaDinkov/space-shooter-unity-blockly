using System;
using System.Collections.Generic;
using Assets.Scripts.GameEvents;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.GameEvents
{
    internal class GameEventManager
    {
        private static GameEventManager instance;
        private readonly Dictionary<GameEventType, Dictionary<int, Action>> events;

        private GameEventManager()
        {
            if (instance == null) instance = this;

            this.events = new Dictionary<GameEventType, Dictionary<int, Action>>();
            foreach (var gameEventType in (GameEventType[]) Enum.GetValues(typeof(GameEventType)))
            {
                this.events.Add(gameEventType, new Dictionary<int, Action>());
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
                action.Invoke();
            }

            
        }

        public int Subscribe(GameEvent gameEvent, Action action)
        {
            var token = action.GetHashCode();
            this.events[gameEvent.EventType].Add(token, action);
           
            return token;
        }

        public int Subscribe<T>(Action action)
        {
            var token = action.GetHashCode();
            foreach (var gameEvent in this.events.Keys)
                if (gameEvent.GetType() == typeof(T))
                    this.events[gameEvent].Add(token, action);
            return token;
        }

        public void Unsubscribe(GameEvent gameEvent, int token)
        {
            this.events[gameEvent.EventType].Remove(token);
            
        }

        public void Unsubscribe<T>(int token)
        {
            foreach (var gameEvent in this.events.Keys)
            {
                if (gameEvent.GetType() == typeof(T))
                {
                    this.events[gameEvent].Remove(token);
                }
            }
        }
    }
}
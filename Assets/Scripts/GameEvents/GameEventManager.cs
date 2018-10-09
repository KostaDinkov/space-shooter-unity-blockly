using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.GameEvents;
using UnityEngine;
using UnityEngine.XR.WSA.Persistence;

namespace Game.GameEvents
{
    class GameEventManager
    {
        private Dictionary<GameEvent, Dictionary<int,Action>> events;
        private static GameEventManager instance;

        private GameEventManager()
        {
            if (instance == null)
            {
                instance = this;
            }
            var allEvents = Resources.LoadAll<GameEvent>("Scriptable Objects/GameEvents");
            this.events = new Dictionary<GameEvent, Dictionary<int, Action>>();           
            foreach (var gameEvent in allEvents)
            {
                events.Add(gameEvent,new Dictionary<int,Action>());
            }
        }

        public static GameEventManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameEventManager();
                }

                return instance;
            }
        }

        public void Publish(GameEvent gameEvent)
        {
            Debug.Log($"Event fired {gameEvent.name}");
            foreach (var action in events[gameEvent].Values)
            {
                action.Invoke();
            }
        }

        public int Subscribe(GameEvent gameEvent, Action action)
        {
            var token = action.GetHashCode();
            events[gameEvent].Add(token,action);
            
            return token;
        }
        public int Subscribe<T>(Action action)
        {
            var token = action.GetHashCode();
            foreach (var gameEvent in events.Keys)
            {
                if (gameEvent.GetType() == typeof(T))
                {
                    events[gameEvent].Add(token, action);
                }
            }
            return token;
        }

        public void Unsubscribe(GameEvent gameEvent , int token)
        {
            events[gameEvent].Remove(token);
        }

        public void Unsubscribe<T>(int token)
        {
            foreach (var gameEvent in events.Keys)
            {
                if (gameEvent.GetType() == typeof(T))
                {
                    events[gameEvent].Remove(token);
                }
            }
            
        }
    }
}
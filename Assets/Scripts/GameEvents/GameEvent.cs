
using System;
using Game.GameEvents;



namespace Assets.Scripts.GameEvents
{
   [Serializable]
    public class GameEvent
    {
        public GameEventType EventType;
        public int EventValue;
    }
}

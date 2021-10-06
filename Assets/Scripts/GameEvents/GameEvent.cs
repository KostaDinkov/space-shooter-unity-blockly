
using System;

namespace Scripts.GameEvents
{
   [Serializable]
    public class GameEvent
    {
        public GameEventType EventType;
        public object EventArgs;
    }
}

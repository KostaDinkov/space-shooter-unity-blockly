using System;
using UnityEngine;

namespace Game.Systems.GameEvents.Commands
{
    
    public abstract class Command
    {
        protected MonoBehaviour receiver;

        public Command(MonoBehaviour receiver)
        {
            this.receiver = receiver;
        }

        public abstract void Execute();

    }

}



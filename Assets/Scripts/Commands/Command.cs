using System.Dynamic;
using System;
using Game.Systems;
using UnityEngine;

namespace Game.Commands
{
    public class Command : ICommand
    {
        public Func<object, string> Action { get; private set; }
        protected ICommandArgs args;
        protected bool isAsync;
        protected Playercontroller pc;

        public Command(Playercontroller pc, Func<object, string> action, ICommandArgs args, bool isAsync = true)
        {
            this.Action = action;
            this.args = args;
            this.pc = pc;
            this.isAsync = isAsync;
        }

        public object Execute()
        {
            Debug.Log(this.Action.Method.Name);
            if (this.isAsync)
            {
                this.pc.StartCoroutine(this.Action.Method.Name, args);
                return null;
            }
            
            return this.Action(args);
        }
    }
}
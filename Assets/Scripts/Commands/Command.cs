using System.Dynamic;
using System;
using Game.Systems;
using UnityEngine;

namespace Game.Commands
{
    public class Command : ICommand
    {
        protected Func<ICommandArgs, object> action;
        protected ICommandArgs args;
        protected bool isAsync;
        protected Playercontroller pc;

        public Command(Playercontroller pc, Func<ICommandArgs, object> action, ICommandArgs args, bool isAsync = true)
        {
            this.action = action;
            this.args = args;
            this.pc = pc;
            this.isAsync = isAsync;
        }

        public object Execute()
        {
            Debug.Log(action.Method.Name);
            if (this.isAsync)
            {
                this.pc.StartCoroutine(action.Method.Name, args);
                return null;
            }
            
            return this.action(args);
        }
    }
}
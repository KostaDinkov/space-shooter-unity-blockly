using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace Game.Commands
{
    public class CommandQueue
    {
        private Queue<Task<Task<string>>> commands;

        public CommandQueue()
        {
            
            this.commands = new Queue<Task<Task<string>>>();
        }

        

        public void Enqueue(Task<Task<string>> task)
        {
            this.commands.Enqueue(task);
            
        }

        

        public void Execute()
        {
            
            var commandTask = commands.Dequeue();
            commandTask.RunSynchronously();
            
        }

        public bool IsEmpty()
        {
            return this.commands.Count == 0;
        }

        public void Clear()
        {
            this.commands.Clear();
        }
    }
}

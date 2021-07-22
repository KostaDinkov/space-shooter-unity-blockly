using System.Collections.Generic;


namespace Game.Commands
{
    public class CommandQueue
    {
        private Queue<ICommand> commands;

        public CommandQueue()
        {
            this.commands = new Queue<ICommand>();
        }

        public void Enqueue(ICommand command)
        {
            this.commands.Enqueue(command);
        }

        public void Execute()
        {
            
            var command = commands.Dequeue();
            var result = command.Execute();
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

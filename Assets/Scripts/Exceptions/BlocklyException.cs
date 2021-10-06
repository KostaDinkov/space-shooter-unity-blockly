using System;

namespace Scripts.Exceptions
{
    public class BlocklyException:Exception
    {
        public BlocklyException()
        {
        }

        public BlocklyException(string message) : base(message)
        {
        }

        public BlocklyException(string message, Exception inner)
            : base(message, inner)
        {

        }

    }
}

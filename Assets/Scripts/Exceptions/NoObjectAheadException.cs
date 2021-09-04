using System;


namespace Scripts.Exceptions
{
    public class NoObjectAheadException:Exception
    {
        public NoObjectAheadException()
        {
        }

        public NoObjectAheadException(string message):base(message)
        {
        }

        public NoObjectAheadException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}

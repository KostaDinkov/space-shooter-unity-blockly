using System;

namespace Scripts.Exceptions
{
    public class DroneOperationException : Exception
    {

        public DroneOperationException()
        {
        }

        public DroneOperationException(string message) : base(message)
        {
        }

        public DroneOperationException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
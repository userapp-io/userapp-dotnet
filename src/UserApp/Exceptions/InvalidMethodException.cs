using System;
namespace UserApp.Exceptions
{
    public class TransportException : UserAppException
    {
        public TransportException(string message)
            : base(message) { }

        public TransportException(string message, Exception innerException)
            : base(message, innerException) { }
    }
    public class InvalidMethodException : UserAppException
    {
        public InvalidMethodException(string message)
            : base(message) {}
    }
}
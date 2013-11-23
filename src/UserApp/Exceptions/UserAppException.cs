using System;

namespace UserApp.Exceptions
{
    public class UserAppException : Exception
    {
        public UserAppException(string message)
            : base(message) { }

        public UserAppException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
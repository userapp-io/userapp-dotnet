namespace UserApp.Exceptions
{
    public class InvalidServiceException : UserAppException
    {
        public InvalidServiceException(string message)
            : base(message) {}
    }
}
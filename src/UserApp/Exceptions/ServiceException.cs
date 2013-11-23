namespace UserApp.Exceptions
{
    public class ServiceException : UserAppException
    {
        public string ErrorCode { get; private set; }

        public ServiceException(string errorCode, string message) : base(message)
        {
            this.ErrorCode = errorCode;
        }
    }
}
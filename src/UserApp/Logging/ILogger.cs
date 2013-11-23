using System;

namespace UserApp.Logging
{
    public interface ILogger
    {
        event Action<Log> OnLogAdded;
        void Log(string message, string type = "info", DateTime? created = null);
    }
}

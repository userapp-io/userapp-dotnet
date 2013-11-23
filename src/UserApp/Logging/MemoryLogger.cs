using System;
using System.Collections.Generic;

namespace UserApp.Logging
{
    public class MemoryLogger : ILogger
    {
        public event Action<Log> OnLogAdded;
        private readonly List<Log> _logs;

        public MemoryLogger()
        {
            this._logs = new List<Log>();
        }

        public void Log(string message, string type = "info", DateTime? created = null)
        {
            var log = new Log()
            {
                Type = type,
                Message = message,
                Created = created ?? DateTime.Now
            };
            
            if (this.OnLogAdded != null)
            {
                this.OnLogAdded(log);
            }

            this._logs.Add(log);
        }

        public IEnumerable<Log> GetLogs()
        {
            return this._logs.ToArray();
        }
    }
}
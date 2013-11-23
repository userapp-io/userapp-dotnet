using System;
using UserApp.Logging;
using UserApp.Transport;

namespace UserApp
{
    public abstract class ClientBase
    {
        private ClientOptions _options;

        public ClientOptions GetOptions()
        {
            return this._options ?? (this._options = ClientOptions.GetGlobal().CreateCopy());
        }

        public bool DebugMode()
        {
            return this.GetOptions().Debug;
        }

        public void SetOptions(dynamic argument)
        {
            this.GetOptions().Set(argument);
        }

        public bool HasLogger()
        {
            return this._options.Logger != null;
        }

        protected void Log(string message)
        {
            this.Log("info", message);
        }

        protected void Log(string type, string message)
        {
            var logger = this.GetOptions().Logger;
            if (logger != null)
            {
                if (!this.DebugMode() && type == "debug")
                {
                    return;
                }
                logger.Log(message, type);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UserApp.CodeConventions;
using UserApp.Core;
using UserApp.Logging;
using UserApp.Transport;

namespace UserApp
{
    public class ClientOptions
    {
        private static readonly ClientOptions Instance = new ClientOptions();

        private ILogger _logger;
        private ITransport _transport;
        private ICodeConvention _codeConvention;

        public bool Debug { get; set; }
        public bool Secure { get; set; }
        public int Version { get; set; }
        public string AppId { get; set; }
        public string Token { get; set; }
        public string BaseAddress { get; set; }
        public bool ThrowErrors { get; set; }

        public ITransport Transport
        {
            get
            {
                if (this._transport == null)
                {
                    this._transport = new NativeTransport();
                }

                return this._transport;
            }
            set
            {
                this._transport = value;
            }
        }

        public ILogger Logger
        {
            get
            {
                if (this.Debug && this._logger == null)
                {
                    this._logger = new MemoryLogger();
                }

                return this._logger;
            }
            set
            {
                this._logger = value;
            }
        }

        public ICodeConvention CodeConvention
        {
            get
            {
                if (this._codeConvention == null)
                {
                    this._codeConvention = new CSharpCodeConvention();
                }

                return this._codeConvention;
            }
            set
            {
                this._codeConvention = value;
            }
        }

        public ClientOptions()
        {
            this.Secure = true;
            this.Version = 1;
            this.BaseAddress = "api.userapp.io";
            this.ThrowErrors = true;
        }

        public void Set(dynamic argument)
        {
            if (TypeUtility.IsAnonymousType(argument.GetType()))
            {
                ReflectionUtility.CopyProperties(argument, this);
            }
        }

        public void Set(string name, object value)
        {
            var optionType = typeof(ClientOptions);
            var propertyInfo = optionType.GetProperty(name);

            if (propertyInfo == null)
            {
                throw new ArgumentException("Option with name '" + name + "' does not exist.");
            }

            propertyInfo.SetValue(this, value);
        }

        public ClientOptions CreateCopy()
        {
            return new ClientOptions()
            {
                Debug = this.Debug,
                Secure = this.Secure,
                Version = this.Version,
                AppId = this.AppId,
                Token = this.Token,
                BaseAddress = this.BaseAddress,
                ThrowErrors = this.ThrowErrors,
                CodeConvention = this.CodeConvention
            };
        }

        public static ClientOptions GetGlobal()
        {
            return Instance;
        }
    }
}

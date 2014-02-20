using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UserApp.CodeConventions;
using UserApp.Core;

namespace UserApp
{
    public class ClientProxy : DynamicObject
    {
        private readonly Client _client;
        private readonly string _serviceName;
        private readonly int _serviceVersion = 1;

        public ClientProxy(string appId, object options = null)
        {
            this._client = new Client(appId, options);
        }

        public ClientProxy(string appId, string token)
        {
            this._client = new Client(appId, token);
        }

        public ClientProxy(string appId, string token, object options = null)
        {
            this._client = new Client(appId, token, options);
        }

        public ClientProxy(object options)
        {
            this._client = new Client(options);
        }

        protected ClientProxy(Client client, string serviceName, int serviceVersion)
        {
            this._client = client;
            this._serviceName = serviceName;
            this._serviceVersion = serviceVersion;
        }

        public ClientOptions GetOptions()
        {
            return this._client.GetOptions();
        }

        public void SetOption(string name, string value)
        {
            this._client.GetOptions().Set(name, value);
        }

        public void SetOptions(dynamic arguments)
        {
            this._client.GetOptions().Set(arguments);
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] arguments, out object result)
        {
            dynamic callArguments = null;
            var convention = this._client.GetOptions().CodeConvention;
            var name = convention.ConvertMethodName(binder.Name);

            if (arguments.Length == 1 && TypeUtility.IsAnonymousType(arguments[0].GetType()))
            {
                callArguments = arguments[0];
            }
            else if (binder.CallInfo.ArgumentNames.Count > 0)
            {
                callArguments = new ExpandoObject();
                var argumentsDictionary = (IDictionary<string, Object>)callArguments;
                for (var i=0;i<binder.CallInfo.ArgumentNames.Count;++i)
                {
                    var argumentName = convention.ConvertArgumentName(binder.CallInfo.ArgumentNames[i]);
                    argumentsDictionary[argumentName] = arguments[i];
                }
            }
            else
            {
                callArguments = new {};
            }

            result = this._client.Call(this._serviceVersion, this._serviceName, name, callArguments);

            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var targetVersion = 0;
            string targetService = null;

            var convention = this._client.GetOptions().CodeConvention;
            var name = convention.ConvertServiceName(binder.Name) ?? "";

            if (this._serviceName == null && name.Length >= 2 && char.ToLowerInvariant(name[0]) == 'v')
            {
                int parsedVersion;
                if (Int32.TryParse(name.Substring(1), out parsedVersion))
                {
                    targetVersion = parsedVersion;
                }
            }

            if (targetVersion == 0)
            {
                targetVersion = this._serviceVersion;
                if (this._serviceName != null)
                {
                    targetService = string.Format("{0}.{1}", this._serviceName, name);
                }
                else
                {
                    targetService = name;
                }
            }

            result = new ClientProxy(this._client, targetService, targetVersion);

            return true;
        }
    }
}
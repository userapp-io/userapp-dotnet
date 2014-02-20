using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using JsonFx.Json;
using UserApp.CodeConventions;
using UserApp.Core;
using UserApp.Exceptions;

namespace UserApp
{
    public class Client : ClientBase
    {
        private const string ClientVersion = "0.1";

        private readonly JsonWriter _jsonWriter = new JsonWriter();
        private readonly JsonReader _jsonReader = new JsonReader();

        public Client(string appId, string token)
        {
            this.GetOptions().AppId = appId;
            this.GetOptions().Token = token;
        }

        public Client(string appId, object options = null)
        {
            this.GetOptions().AppId = appId;
            if (options != null && TypeUtility.IsAnonymousType(options.GetType()))
            {
                this.SetOptions(options);
            }
        }

        public Client(string appId, string token, object options = null)
        {
            this.GetOptions().AppId = appId;
            this.GetOptions().Token = token;
            if (options != null && TypeUtility.IsAnonymousType(options.GetType()))
            {
                this.SetOptions(options);
            }
        }

        public Client(object options)
        {
            if (options != null && TypeUtility.IsAnonymousType(options.GetType()))
            {
                this.SetOptions(options);
            }
        }

        public dynamic Call(int version, string service, string method, object arguments = null)
        {
            ExpandoObject[] logs = null;
            Exception generatedException = null;

            var options = this.GetOptions();
            var transport = options.Transport;

            var protocol = options.Secure ? "https" : "http";
            var serviceUrl = String.Format("{0}://{1}/v{2}/{3}.{4}", protocol, options.BaseAddress, version, service, method);

            if (options.Debug)
            {
                serviceUrl += "?$debug";
            }

            var headers = new WebHeaderCollection()
            {
                {"Content-Type", "application/json"},
                {"User-Agent", String.Format("UserApp/{0} .NET/{1}.{2}", ClientVersion, Environment.Version.Major, Environment.Version.Minor)},
                {"Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(options.AppId+":"+options.Token))},
            };

            var encodedArguments = arguments == null ? "{}" : this._jsonWriter.Write(arguments);

            this.Log("debug", String.Format("Calling URL '{0}' with headers '{1}' and arguments '{2}'", serviceUrl, headers, encodedArguments));
            var response = transport.Request("POST", serviceUrl, headers, encodedArguments);
            this.Log("debug", String.Format("Recieved response '{0}'", response));

            if (response.Status.Code != HttpStatusCode.OK)
            {
                throw new Exception(String.Format("Expected 200 OK response. Recieved {0} {1}.", response.Status.Code, response.Status.Message));
            }

            var result = ProcessContentType(response.Headers["Content-Type"], response.Body);
            
            if(result == null)
            {
                return null;
            }

            var expandoObject = result as IDictionary<string, Object>;
            var resultIsArray = ((Type)result.GetType()).IsArray;

            if (expandoObject != null)
            {
                if (expandoObject.ContainsKey("error_code"))
                {
                    switch (result.error_code as string)
                    {
                        case "INVALID_SERVICE":
                            generatedException = new InvalidServiceException(string.Format("Service '{0}' does not exist.", service));
                            break;
                        case "INVALID_METHOD":
                            generatedException = new InvalidMethodException(string.Format("Method '{0}' does not exist.", method));
                            break;
                        default:
                            if (options.ThrowErrors)
                            {
                                generatedException = new ServiceException(result.error_code, result.message);
                            }
                            break;
                    }
                }
                else
                {
                    if (this.DebugMode() && expandoObject.ContainsKey("__logs"))
                    {
                        logs = (ExpandoObject[])expandoObject["__logs"];
                        expandoObject.Remove("__logs");
                    }
                    if (service == "user")
                    {
                        if (method == "login" && String.IsNullOrEmpty(options.Token))
                        {
                            options.Token = result.token;
                        }
                        else if (method == "logout" && !String.IsNullOrEmpty(options.Token))
                        {
                            options.Token = null;
                        }
                    }
                }
            }
            else if (resultIsArray && this.DebugMode())
            {
                var expandoObjects = result as IDictionary<string, Object>[];
                if (expandoObjects != null)
                {
                    logs = expandoObjects.Where(x => !x.GetType().IsValueType && x.ContainsKey("__logs")).Cast<ExpandoObject>().ToArray();
                    result = expandoObjects.Where(x => !x.GetType().IsValueType && !x.ContainsKey("__logs")).ToArray();
                }
            }

            if (logs != null && this.HasLogger())
            {
                var logger = this.GetOptions().Logger;
                foreach (dynamic log in logs)
                {
                    if(((IDictionary<string, Object>)log).ContainsKey("message") && log.message is string)
                    {
                        logger.Log(log.message, log.type, DateTimeUtility.ParseUnixTimestamp((double)log.created_at));
                    }
                }
            }

            if (generatedException != null)
            {
                throw generatedException;
            }

            if (options.CodeConvention != null)
            {
                if (result is ExpandoObject)
                {
                    result = new ObjectAccessDecorator(options.CodeConvention, (ExpandoObject)result);
                }
                else if (resultIsArray)
                {
                    var resultArray = result as object[];
                    var newResultArray = new object[resultArray.Length];

                    for (var i = 0; i < resultArray.Length; ++i)
                    {
                        var item = resultArray[i];

                        if (item == null)
                        {
                            continue;
                        }

                        var itemType = item.GetType();

                        if (itemType == typeof(ExpandoObject))
                        {
                            item = new ObjectAccessDecorator(options.CodeConvention, (ExpandoObject)item);
                        }

                        newResultArray[i] = item;
                    }

                    result = newResultArray;
                }
            }

            return result;
        }

        private dynamic ProcessContentType(string contentType, string body)
        {
            switch (contentType)
            {
                case "application/json":
                    return this._jsonReader.Read(body);
                default:
                    throw new NotSupportedException("Type '" + contentType + "' not supported.");
            }
        }
    }
}

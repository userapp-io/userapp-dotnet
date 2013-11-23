using System.Net;
using EasyHttp.Http;
using UserApp.Exceptions;

namespace UserApp.Transport
{
    public class NativeTransport : ITransport
    {
        public HttpResponse Request(string method, string url, WebHeaderCollection headers, string body)
        {
            var client = new HttpClient();

            client.ThrowExceptionOnHttpError = false;

            foreach (var header in headers.AllKeys)
            {
                var value = headers[header];

                switch (header)
                {
                    case "Content-Type":
                        client.Request.ContentType = value;
                        break;
                    case "User-Agent":
                        client.Request.UserAgent = value;
                        break;
                    default:
                        client.Request.AddExtraHeader(header, value);
                        break;
                }
            }

            try
            {
                var response = client.Post(url, body, headers["Content-Type"]);

                return new HttpResponse()
                {
                    Status = new HttpStatus()
                    {
                        Code = response.StatusCode,
                        Message = response.StatusDescription
                    },
                    Headers = response.RawHeaders,
                    Body = response.RawText
                };
            }
            catch (WebException exception)
            {
                throw new UserAppException(exception.Message, exception);
            }
        }
    }
}

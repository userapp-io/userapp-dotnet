using System.Collections.Generic;
using System.Net;

namespace UserApp.Transport
{
    /// <summary>
    /// Represents a HTTP Transport
    /// </summary>
    public interface ITransport
    {
        HttpResponse Request(string method, string url, WebHeaderCollection headers, string body);
    }
}
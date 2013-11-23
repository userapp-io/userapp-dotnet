using System.Collections.Generic;
using System.Net;

namespace UserApp.Transport
{
    public class HttpResponse
    {
        public HttpStatus Status { get; set; }
        public WebHeaderCollection Headers { get; set; }
        public string Body { get; set; }

        public override string ToString()
        {
            return string.Format("Status={0} ({1}), Headers={2}, Body={3}",
                (int)this.Status.Code, this.Status.Message, this.Headers, this.Body);
        }
    }
}
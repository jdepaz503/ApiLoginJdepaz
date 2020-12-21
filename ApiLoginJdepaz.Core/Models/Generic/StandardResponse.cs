using System.Collections.Generic;
using System.Net;


namespace ApiLoginJdepaz.Core.Models.Generic
{
    public class StandardResponse<T>
    {
        public IList<T> Data { get; set; }
        public int TotalItems { get; set; }
        public Errors Error { get; set; }
    }

    public class Errors
    {
        public HttpStatusCode HttpCode { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
    }
}

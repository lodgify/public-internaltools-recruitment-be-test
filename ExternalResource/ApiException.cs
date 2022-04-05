using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExternalResource
{
    public class ApiException : Exception
    {
        public ApiException(string message, int statusCode, string response,
            IReadOnlyDictionary<string, IEnumerable<string>> headers, Exception innerException) : base(
            JsonConvert.DeserializeObject<ApiResponse>(response)?.Message, innerException)
        {
            StatusCode = statusCode;
            Response = response;
            Headers = headers;
        }


        public int StatusCode { get; private set; }

        public string Response { get; private set; }

        public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; private set; }


        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
        }
    }



    internal class ApiResponse
    {
        public string StatusCode { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
    }
}

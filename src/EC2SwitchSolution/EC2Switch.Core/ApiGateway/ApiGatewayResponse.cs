using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace EC2Switch.Core.ApiGateway
{
    [JsonObject]
    public class ApiGatewayResponse
    {
        [JsonProperty("statusCode")]
        public HttpStatusCode StatusCode { get; set; }

        [JsonProperty("headers")]
        public IDictionary<string, string> Headers { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        public static IDictionary<string, string> DefaultHeaders => new Dictionary<string, string>();
    }
}

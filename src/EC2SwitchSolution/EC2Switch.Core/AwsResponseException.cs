using Amazon.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace EC2Switch.Core
{
    public class AwsResponseException<TRequest, TResponse> : Exception
        where TRequest : AmazonWebServiceRequest
        where TResponse : AmazonWebServiceResponse
    {
        public AwsResponseException(TRequest request, TResponse response)
            : this(request, response, exception: null)
        { }

        public AwsResponseException(TRequest request, TResponse response, Exception exception)
            : base(_ToMessage(request, response), exception)
        {
            Request = request;
            Response = response;
        }

        public TRequest Request { get; private set; }
        public TResponse Response { get; private set; }

        private static string _ToMessage(TRequest request, TResponse response)
        {
            var obj = JObject.FromObject(new
            {
                Request = request,
                Response = response,
            });
            return $"AWSへのアクセスでエラーが発生しました。{obj.ToString(Formatting.None)}";
        }
    }
}

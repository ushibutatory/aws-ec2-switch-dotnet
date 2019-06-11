using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.Lambda.Core;
using EC2Switch.Core.ApiGateway;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace EC2Switch.Off
{
    public class Function
    {
        private readonly IServiceProvider _provider;

        public Function()
        {
            _provider = new ServiceCollection()
                .AddOptions()
                .Configure<AmazonEC2Config>(_ =>
                {
                    _.RegionEndpoint = RegionEndpoint.APNortheast1;
                })
                .AddSingleton<Application>()
                .BuildServiceProvider();
        }

        public async Task<ApiGatewayResponse> FunctionHandlerAsync(Parameter parameter, ILambdaContext context)
        {
            var app = _provider.GetService<Application>();

            var response = await app.StartAsync(parameter.Request);

            return new ApiGatewayResponse
            {
                StatusCode = response.HttpStatusCode,
                Headers = ApiGatewayResponse.DefaultHeaders,
                Body = JsonConvert.SerializeObject(response),
            };
        }

        public class Parameter
        {
            public StartInstancesRequest Request { get; set; }
        }
    }
}

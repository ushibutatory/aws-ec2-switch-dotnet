using Amazon.EC2;
using Amazon.EC2.Model;
using EC2Switch.Core;
using Microsoft.Extensions.Options;
using System.Net;
using System.Threading.Tasks;

namespace EC2Switch.List
{
    public class Application
    {
        private readonly AmazonEC2Config _config;

        public Application(IOptions<AmazonEC2Config> config)
        {
            _config = config.Value;
        }

        public async Task<DescribeInstancesResponse> ListAsync(DescribeInstancesRequest request)
        {
            using (var ec2 = new AmazonEC2Client(_config))
            {
                var response = await ec2.DescribeInstancesAsync(request);
                if (response.HttpStatusCode != HttpStatusCode.OK)
                    throw new AwsResponseException<DescribeInstancesRequest, DescribeInstancesResponse>(request, response);
                return response;
            }
        }
    }
}

using Amazon.EC2.Model;
using System.Linq;

namespace EC2Switch.Core.EC2.Model.Extensions
{
    public static class InstanceExtensions
    {
        public static InstanceState InstanceState(this Instance _)
        {
            return (InstanceState)_.State.Code;
        }

        public static string InstanceName(this Instance _, string @default = "")
        {
            return _.Tags.Where(tag => tag.Key == "Name").FirstOrDefault()?.Value ?? @default;
        }
    }
}

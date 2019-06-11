namespace EC2Switch.Core.EC2.Model
{
    // https://docs.aws.amazon.com/AWSEC2/latest/APIReference/API_InstanceState.html
    public enum InstanceState
    {
        Pending = 0,
        Running = 16,
        ShuttingDown = 32,
        Terminated = 48,
        Stopping = 64,
        Stopped = 80,
    }
}

namespace ServiceBusProducer;

internal class Settings
{
    internal string ConnectionString { get; set; }
    internal string TopicName { get; set; }
    internal string SubscriptionName { get; set; }
    internal int MaxDeliveryCount { get; set; }
    internal int AutoDeleteOnIdle { get; set; }
    internal int DefaultMessageTimeToLive { get; set; }
    internal int LockDuration { get; set; }
    internal string SqlFilter { get; set; }
}

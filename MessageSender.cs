using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Azure.Messaging.ServiceBus.Administration;

namespace ServiceBusProducer;

internal static class MessageSender
{
    internal static async Task SendMessageAsync(Settings settings)
    {
        try
        {
            string[] fileExtensions = { "txt", "json" };
            var fileExtension = Selector.ConsoleSelector(fileExtensions, "extension");

            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string[] textFiles = Directory.GetFiles(appDirectory, $"*.{fileExtension}");

            if (textFiles.Length == 0)
            {
                Console.WriteLine("There is no txt file.");
                return;
            }

            var file = Selector.ConsoleSelector(textFiles);

            Console.WriteLine($"Processing: {Path.GetFileName(file)}");

            string fileContent = File.ReadAllText(file);
            if (IsValidJson(fileContent))
            {
                Console.WriteLine("JSON validation complete.");
            }
            else
            {
                Console.WriteLine("JSON validation failed. Invalid JSON format");
            }

            ServiceBusAdministrationClient adminClient = new ServiceBusAdministrationClient(settings.ConnectionString);
            var isExistsSubscription = await adminClient.SubscriptionExistsAsync(settings.TopicName, settings.SubscriptionName);
            if (!isExistsSubscription)
            {

                Console.WriteLine($"Create subscription {settings.SubscriptionName} for topic {settings.TopicName}.");

                var subscriptionOptions = new CreateSubscriptionOptions(settings.TopicName, settings.SubscriptionName)
                {
                    MaxDeliveryCount = settings.MaxDeliveryCount,
                    AutoDeleteOnIdle = TimeSpan.FromDays(settings.AutoDeleteOnIdle),
                    DefaultMessageTimeToLive = TimeSpan.FromDays(settings.DefaultMessageTimeToLive),
                    LockDuration = TimeSpan.FromMinutes(settings.LockDuration)
                };

                var sqlExpression = "name like '%test%'";
                var ruleOptions = new CreateRuleOptions
                {
                    Filter = new SqlRuleFilter(sqlExpression)
                };

                var response = await adminClient.CreateSubscriptionAsync(subscriptionOptions, ruleOptions);
                if (response.Value is null)
                {
                    throw new ArgumentNullException($"Exception during create Subscription {settings.SubscriptionName} for topic {settings.TopicName}.");
                }

                Console.WriteLine($"Subcription {settings.SubscriptionName} for topic {settings.TopicName} created successfully.");
            }
            Console.WriteLine($"Subcription {settings.SubscriptionName} for topic {settings.TopicName} exists.");

            ServiceBusClient serviceBusClient = new ServiceBusClient(settings.ConnectionString);
            ServiceBusSender sender = serviceBusClient.CreateSender(settings.TopicName);

            var message = PrepareMessage(fileContent);
            await sender.SendMessageAsync(message);

            Console.WriteLine($"Message send successfully. Message: {message.Body}");
        }
        catch (Exception ex)
        {
        }
    }

    private static bool IsValidJson(string content)
    {
        try
        {
            JToken.Parse(content);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static ServiceBusMessage PrepareMessage(string payload)
    {
        string json = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(payload), Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });

        ServiceBusMessage serviceBusMessage = new ServiceBusMessage((ReadOnlyMemory<byte>)Encoding.UTF8.GetBytes(json));
        serviceBusMessage.ApplicationProperties.Add("name", "test");
        serviceBusMessage.ContentType = "application/json";

        return serviceBusMessage;
    }
}

using System.Text.Json;

namespace ServiceBusProducer;
internal static class SettingsManager
{
    private const string SettingsFileName = ".settings";

    internal static void ConfigureSettings()
    {
        Console.WriteLine("\nConfiguring settings file...\n");

        string connectionString = PromptForInput("Connection string", "DefaultConnectionString");
        string topicName = PromptForInput("Topic name", "DefaultTopic");
        string subscriptionName = PromptForInput("Subscription name", "DefaultSubscription");
        int maxDeliveryCount = int.Parse(PromptForInput("Max delivery count", "10"));
        int autoDeleteAfterIdleDays = int.Parse(PromptForInput("Auto-delete after idle for (days)", "7"));
        int messageTimeToLiveDays = int.Parse(PromptForInput("Message time to live (days)", "14"));
        int messageLockDurationMinutes = int.Parse(PromptForInput("Message lock duration (minutes)", "5"));
        string sqlFilter = PromptForInput("SQL Filter", "name like '%test%'");

        var settings = new Settings
        {
            ConnectionString = connectionString,
            TopicName = topicName,
            SubscriptionName = subscriptionName,
            MaxDeliveryCount = maxDeliveryCount,
            AutoDeleteOnIdle = autoDeleteAfterIdleDays,
            DefaultMessageTimeToLive = messageTimeToLiveDays,
            LockDuration = messageLockDurationMinutes,
            SqlFilter = sqlFilter
        };

        string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(SettingsFileName, json);

        Console.WriteLine("\nSettings saved successfully!");
        Console.WriteLine("Press any key to return to the main menu...");
        Console.ReadKey();
    }

    internal static bool ValidateSettingsFile()
    {
        if (!File.Exists(SettingsFileName))
        {
            Console.WriteLine("\nThe settings file does not exist, please generate a settings file...");
            return false;
        }

        try
        {
            string json = File.ReadAllText(SettingsFileName);
            JsonDocument.Parse(json);
            return true;
        }
        catch
        {
            return false;
        }
    }

    internal static Settings LoadSettings()
    {
        if (!File.Exists(SettingsFileName))
            return null;

        try
        {
            string json = File.ReadAllText(SettingsFileName);
            return JsonSerializer.Deserialize<Settings>(json);
        }
        catch
        {
            return null;
        }
    }

    private static string PromptForInput(string prompt, string defaultValue)
    {
        Console.Write($"{prompt} (default: {defaultValue}): ");
        string input = Console.ReadLine();
        return string.IsNullOrWhiteSpace(input) ? defaultValue : input;
    }
}

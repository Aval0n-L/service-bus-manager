using ServiceBusProducer;

internal class Program
{
    private static async Task Main(string[] args)
    {
        try
        {
            string option = string.Empty;
            while (true)
            {
                Console.Clear(); 
                Artist.DrawHeader("Console application", "Configure connection to Azure Service Bus", "And send test message to the topic");
                
                string[] options = { "1. Send message", "2. Configure settings", "3. Exit" };
                option = Selector.ConsoleSelector(options);
                switch (option)
                {
                    case "1. Send message":
                        if (SettingsManager.ValidateSettingsFile())
                        {
                            Console.Clear();
                            Artist.DrawHeader(string.Empty, "GOOD LUCK!", string.Empty);
                            var settings = SettingsManager.LoadSettings();
                            await MessageSender.SendMessageAsync(settings).ConfigureAwait(false);
                        }

                        Console.WriteLine("\nPress any key to return to the main menu...");
                        Console.ReadKey();
                        break;
                    case "2. Configure settings":
                        Console.Clear();
                        Artist.DrawHeader(string.Empty, "Settings", string.Empty);
                        SettingsManager.ConfigureSettings();

                        Console.WriteLine("\nPress any key to return to the main menu...");
                        Console.ReadKey();
                        break;
                    case "3. Exit":
                        Console.WriteLine();
                        Console.WriteLine("Exiting application...");
                        return;
                    default:
                        Artist.DrawHeader("Console application", "Configure connection to Azure Service Bus", "And send test message to the topic");
                        break;
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }
}
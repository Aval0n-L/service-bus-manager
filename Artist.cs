namespace ServiceBusProducer;

public static class Artist
{
    public static void PrintMainMenu()
    {
        DrawHeader("Console application", "Configure connection to Azure Service Bus", "And send test message to the topic");
        Console.WriteLine();
        Console.WriteLine(" * 1. Send message");
        Console.WriteLine(" * 2. Configure settings");
        Console.WriteLine(" * 3. Exit");
        Console.WriteLine();
        Console.Write("Choose an option: ");
    }
    public static void DrawHeader(string firstLine, string secondLine, string thirdLine)
    {
        Console.Clear();
        string line = "==========================================================";
        Console.WriteLine(line);
        Console.WriteLine($"# {CenterText(firstLine, line.Length - 4)} #");
        Console.WriteLine($"# {CenterText(secondLine, line.Length - 4)} #");
        Console.WriteLine($"# {CenterText(thirdLine, line.Length - 4)} #");
        Console.WriteLine(line);
    }
    private static string CenterText(string text, int width)
    {
        if (string.IsNullOrEmpty(text))
            return new string(' ', width);

        int padding = (width - text.Length) / 2;
        return new string(' ', padding) + text + new string(' ', width - text.Length - padding);
    }
}

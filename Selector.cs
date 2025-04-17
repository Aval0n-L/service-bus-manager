namespace ServiceBusProducer;

public static class Selector
{
    public static string ConsoleSelector(string[] options, string additionalText = "")
    {
        Console.WriteLine();

        if (!string.IsNullOrEmpty(additionalText))
        {
            Console.WriteLine($"Chose file {additionalText}:");
        }

        int selectedIndex = 0;
        int startCursorTop = Console.CursorTop;

        while (true)
        {
            for (int i = 0; i < options.Length; i++)
            {
                Console.SetCursorPosition(0, startCursorTop + i);
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"> {options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {options[i]}");
                }
            }

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = (selectedIndex - 1 + options.Length) % options.Length;
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = (selectedIndex + 1) % options.Length;
                    break;
                case ConsoleKey.Enter:
                    Console.SetCursorPosition(0, startCursorTop + options.Length);
                    return options[selectedIndex];
                default:
                    break;
            }
        }
    }
}

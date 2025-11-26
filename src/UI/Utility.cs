namespace Game;
/// <summary>
/// Abstract Utility class for reusable code. 
/// </summary>
abstract class Utility
{
    /// <summary>
    /// Generates a numbered menu with a title and menu options based on the amount of input strings in 'choices'.
    /// </summary>
    /// <param name="title">string value with a default state to be the title in a menu</param>
    /// <param name="choices">string array with params keyword for inputting infinite menu options </param>
    public static void GenerateMenu(string title = "Choose a Menu Option:", params string[] choices)
    {
        string msg = "_______________________________\n";
        msg += title + "\n";
        for (int i = 0; i < choices.Length; i++)
        {
            msg += $"\n[{i + 1}] [{choices[i]}]\n";
        }
        msg += "_______________________________";
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
    public static ConsoleKeyInfo PromptKey(string input = "", bool clear = true)
    {
        if (clear)
        { try { Console.Clear(); } catch { } }
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\nPress [ESC] to cancel...");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write(input);
        Console.ResetColor();
        return Console.ReadKey(true)!;
    }
    /// <summary>
    /// Prints a colored Success message with a bool option for returning to menu
    /// </summary>
    /// <param name="msg">Specific Success message</param>
    /// <param name="menuChoice">boolean option to include menu message</param>
    public static void Success(string msg, bool menuChoice = false)
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine(msg);
        if (menuChoice)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("'ENTER' to return to menu...");
            Console.ReadKey(true);
        }
        Console.ResetColor();
        try { Console.Clear(); } catch { }
    }
    public static void PrintColor(string msg, ConsoleColor consoleColor, bool NoLineWrite = false)
    {
        Console.ForegroundColor = consoleColor;
        if (NoLineWrite)
        {
            Console.Write(msg);
        }
        else
        {
            Console.WriteLine(msg);
        }
        Console.ResetColor();
    }
    public static void Narrate(string text, ConsoleColor color = ConsoleColor.DarkYellow, bool slow = true, int charDelayMs = 30, bool waitForKey = true)
    {
        var prev = Console.ForegroundColor;
        Console.ForegroundColor = color;

        if (!slow)
        {
            Console.WriteLine(text);
        }
        else
        {
            foreach (var ch in text)
            {
                Console.Write(ch);
                System.Threading.Thread.Sleep(charDelayMs);
            }
            Console.WriteLine();
        }

        Console.ForegroundColor = prev;

        if (waitForKey)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ResetColor();
            Console.ReadKey(true);
            try { Console.Clear(); } catch { }
        }
    }
    public static void GenerateMenuActions(int selectedIndex, string[] menuOptions,ConsoleColor menuColor = ConsoleColor.DarkYellow)
    {
        int colorShiftOffset = 0;

        ConsoleColor[] colors = { ConsoleColor.Yellow, ConsoleColor.DarkYellow };

        for (int i = 0; i < menuOptions.Length; i++)
        {
            if (i == selectedIndex)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"  > {menuOptions[i]}");
            }
            else
            {
                int colorIndex = (i + colorShiftOffset) % colors.Length;  // Alternates between 0 and 1
                Console.ForegroundColor = colors[colorIndex];
                Console.WriteLine($" {menuOptions[i]}");
            }
            Console.ResetColor();
        }

        colorShiftOffset = (colorShiftOffset + 1) % 2;  // Change to % 2
    }
    
}
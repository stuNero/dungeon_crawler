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
    /// <summary>
    /// Prompts the user with an input prompt, prints option to cancel prompt and then returns Console input.
    /// </summary>
    /// <remarks>
    /// DOES NOT HANDLE USER INPUT, this requires outside implementation. 
    /// </remarks>
    /// <param name="input">Consoles prompt</param>
    /// <param name="clear">Boolean variable for option to clear previous console text</param>
    /// <returns></returns>
    public static string Prompt(string input, bool clear = true)
    {
        if (clear)
        { try { Console.Clear(); } catch { } }
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\n(Empty line and 'ENTER' to cancel..)");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write(input);
        Console.ResetColor();
        return Console.ReadLine()!;
    }
    public static ConsoleKeyInfo PromptKey(string input = "", bool clear = true)
    {
        if (clear)
        { try { Console.Clear(); } catch { } }
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\n(Empty line and 'ENTER' to cancel..)");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write(input);
        Console.ResetColor();
        return Console.ReadKey()!;
    }
    /// <summary>
    /// Prints a colored Error message and an option to return to menu
    /// </summary>
    /// <param name="msg">Specific Error message</param>
    public static void Error(string msg)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(msg);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("'ENTER' to return to menu...");
        Console.ResetColor();
        Console.ReadKey(true);
        try { Console.Clear(); } catch { }
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
    public static void PrintColor(string msg, ConsoleColor consoleColor)
    {
        Console.ForegroundColor = consoleColor;
        Console.WriteLine(msg);
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
    public static void GenerateMenuActions(int selectedIndex, string[] menuOptions)
    {
        for (int i = 0; i<menuOptions.Length; ++i)
        {
            if (i == selectedIndex)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"    {menuOptions[i]}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"{menuOptions[i]}");
                Console.ResetColor();
            }
        }
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\n(Press 'ESC' to return to previous menu..)");
        Console.ResetColor();
    }
}
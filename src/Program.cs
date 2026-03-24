using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Game;

Menu currentMenu = Menu.Start;
bool running = true;
bool narration = true;
int selectedIndex = 0;
int selectedSaveSlot = 0;

Player? player = null;
Dictionary<int, List<Entity>> entitiesPerSave = new Dictionary<int, List<Entity>>();
List<Player> playerClasses = DataManager.LoadGlobalClasses();
List<Entity> entities = new List<Entity>();
List<Item> items = DataManager.LoadGlobalItems();

while (running)
{
    try { Console.Clear(); } catch { }
    bool subRunning;
    switch (currentMenu)
    {
        case Menu.Start:
            subRunning = true;
            string[] startOptions = ["START", "QUIT"];

            selectedIndex = 0;
            Dictionary<string, Menu> startMenuOptions = new Dictionary<string, Menu>();
            startMenuOptions.Add(startOptions[0], Menu.Creation);
            startMenuOptions.Add(startOptions[1], Menu.Quit);

            while (subRunning)
            {
                try { Console.Clear(); } catch { }
                Utility.GenerateMenu(title: "D U N G E O N  C R A W L E R");
                Utility.GenerateMenuActions(selectedIndex, startOptions);
                Utility.PrintColor("\n\n\nCONTROLS: \nNavigate:  [^] [v]" +
                                                   "\nSelection: [ENTER]" +
                                                   "\nCancel:    [ESC]", ConsoleColor.DarkGray);
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex--;
                        if (selectedIndex < 0)
                            selectedIndex = startOptions.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex++;
                        if (selectedIndex >= startOptions.Length)
                            selectedIndex = 0;
                        break;
                    case ConsoleKey.Enter:
                        Console.Clear();
                        currentMenu = startMenuOptions[startOptions[selectedIndex]];
                        subRunning = false;
                        break;
                }
            }
            break;
        
        case Menu.Creation:
            int chosenSlot = 0;
            subRunning = true;
            while (subRunning)
            {
                Console.Clear();
                string[] saveSlotsOutput = [
                    "\n _ _ _\n"+
                    "|     |\n" +
                    "|  1  |\n" +
                    "|_ _ _|\n",
                    "\n _ _ _\n"+
                    "|     |\n" +
                    "|  2  |\n" +
                    "|_ _ _|\n",
                    "\n _ _ _\n"+
                    "|     |\n" +
                    "|  3  |\n" +
                    "|_ _ _|\n"];
                Utility.GenerateMenu("CHOOSE SAVE SLOT ");
                Utility.GenerateMenuActions(selectedIndex, saveSlotsOutput);
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex--;
                        if (selectedIndex < 0)
                            selectedIndex = saveSlotsOutput.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex++;
                        if (selectedIndex > saveSlotsOutput.Length - 1)
                            selectedIndex = 0;
                        break;
                    case ConsoleKey.Enter:
                        switch (selectedIndex)
                        {
                            case 0:
                                chosenSlot = 1;
                                Utility.PromptKey("Save slot 1 chosen");
                                break;
                            case 2:
                                chosenSlot = 2;
                                Utility.PromptKey("Save slot 2 chosen");
                                break;
                            case 3:
                                chosenSlot = 3;
                                Utility.PromptKey("Save slot 3 chosen");
                                break;
                        }
                        subRunning = false;
                        break;
                }
            }
            bool slotExists = DataManager.CheckSaveSlot(chosenSlot);
            string[] yesNo = ["Yes", "No"];
            subRunning = true;
            while (subRunning)
            {
                Console.Clear();
                Utility.GenerateMenu("Skip narration?");
                Utility.GenerateMenuActions(selectedIndex, yesNo);
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex--;
                        if (selectedIndex < 0)
                            selectedIndex = yesNo.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex++;
                        if (selectedIndex > yesNo.Length - 1)
                            selectedIndex = 0;
                        break;
                    case ConsoleKey.Enter:
                        if (yesNo[selectedIndex] == "Yes")
                        { narration = false; subRunning = false; }
                        else if (yesNo[selectedIndex] == "No") { narration = true; subRunning = false; }
                        break;
                }
            }
            List<string> playCharNames = new List<string>();
            foreach (Player pClass in playerClasses)
            {
                playCharNames.Add(pClass.Name);
            }
            subRunning = true;
            int selectedCharIndex = 0;
            while (subRunning)
            {
                Console.Clear();
                Utility.GenerateMenu("Choose your character");
                Utility.GenerateMenuActions(selectedCharIndex, playCharNames.ToArray());
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedCharIndex--;
                        if (selectedCharIndex < 0)
                            selectedCharIndex = playerClasses.Count - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedCharIndex++;
                        if (selectedCharIndex > playerClasses.Count - 1)
                            selectedCharIndex = 0;
                        break;
                    case ConsoleKey.Enter:
                        selectedIndex = 0;
                        bool boolYesNo = true;
                        while (boolYesNo)
                        {
                            Console.Clear();
                            Utility.GenerateMenu("Are you sure?");
                            Utility.PrintColor(playerClasses[selectedCharIndex].Info(), ConsoleColor.DarkCyan);
                            Utility.GenerateMenuActions(selectedIndex, yesNo);
                            switch(Console.ReadKey().Key)
                            {
                                case ConsoleKey.UpArrow:
                                    selectedIndex--;
                                    if (selectedIndex < 0)
                                        selectedIndex = yesNo.Length - 1;
                                    break;
                                case ConsoleKey.DownArrow:
                                    selectedIndex++;
                                    if (selectedIndex > yesNo.Length - 1)
                                        selectedIndex = 0;
                                    break;
                                case ConsoleKey.Enter:
                                    if (yesNo[selectedIndex] == "Yes")
                                    {
                                        player = playerClasses[selectedCharIndex];
                                        boolYesNo = false;
                                        subRunning = false;
                                    }
                                    else if (yesNo[selectedIndex] == "No") { boolYesNo = false; }
                                    break;
                            }
                        }
                        break;
                }
            }
            if (narration)
            {
                Console.Clear();
                Utility.Narrate(text: "The hinges of the door creaks and you enter the room,\nyour torch light slowly " +
                "illuminates an unlocked rusty, matte padlock..\n");
                Utility.Narrate(text: "As you step closer you make out the outlines of an\n" +
                "old oak chest which materializes from the black, seemingly infinite void room. ");
            }
            List<Item> tempItems = new();
            foreach (Item item1 in items) { tempItems.Add(item1); }
            selectedIndex = 0;
            subRunning = true;
            selectedIndex = 0;
            while (player!.InventoryRange() < 3 && subRunning)
            {
                List<string> itemList = new();
                // Print available items
                foreach (Item item in tempItems)
                { itemList.Add("\n" + item.Info()); }

                string[] itemArray = itemList.ToArray();
                try { Console.Clear(); } catch { }
                Utility.GenerateMenu(title: $"\nChoose Your Starting Items ({3 - player.InventoryRange()})");
                Utility.GenerateMenuActions(selectedIndex, itemArray, menuColor: ConsoleColor.DarkMagenta);
                Utility.PrintColor("Press [ESC] to quit choosing", ConsoleColor.DarkGray);
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex--;
                        if (selectedIndex < 0)
                            selectedIndex = itemArray.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex++;
                        if (selectedIndex >= itemArray.Length)
                            selectedIndex = 0;
                        break;
                    case ConsoleKey.Enter:
                        player.AddItem(tempItems[selectedIndex]);
                        tempItems.Remove(tempItems[selectedIndex]);
                        selectedIndex = 0;
                        break;
                    case ConsoleKey.Escape:
                        subRunning = false;
                        break;
                }
            }
            foreach (Item item in tempItems)
            {
                if (item == null) continue;
                foreach (Entity entity in entities)
                {
                    entity.AddItem(item);
                }
            }

            Console.Clear();
            Utility.GenerateMenu("Your Inventory");
            player.CheckInventory();
            foreach (Item item in items)
            {
                Console.WriteLine(item.Id);
            }
            Utility.PrintColor("Press Any Key to continue", ConsoleColor.DarkGray);
            Console.ReadKey(true);
            currentMenu = Menu.Main;
            if (narration) Utility.Narrate("You delve into the depths of the dungeon...");
            break;
        case Menu.Main:
            subRunning = true;
            selectedIndex = 0;
            string[] mainOptions = ["Attack enemy WIP", "Character"];
            Dictionary<string, Menu> menuOptions = new Dictionary<string, Menu>();
            menuOptions.Add(mainOptions[0], Menu.Battle);
            menuOptions.Add(mainOptions[1], Menu.Character);
            while (subRunning)
            {
                try { Console.Clear(); } catch { }
                Utility.GenerateMenu(title: "MAIN MENU\nPlaying as " + player!.Name);
                Utility.GenerateMenuActions(selectedIndex, mainOptions);
                Utility.PrintColor("\n[ESC] - Back To Previous Menu", ConsoleColor.DarkGray);
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex--;
                        if (selectedIndex < 0)
                            selectedIndex = mainOptions.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex++;
                        if (selectedIndex > mainOptions.Length - 1)
                            selectedIndex = 0;
                        break;
                    case ConsoleKey.Enter:
                        subRunning = false;
                        currentMenu = menuOptions[mainOptions[selectedIndex]];
                        break;
                    case ConsoleKey.Escape:
                        selectedIndex = 0;
                        yesNo = ["Yes", "No"];

                        while (subRunning)
                        {
                            Console.Clear();
                            Utility.GenerateMenu("You are about to quit and will lose all progress!\nAre you sure?");
                            Utility.GenerateMenuActions(selectedIndex, yesNo);
                            switch (Console.ReadKey().Key)
                            {
                                case ConsoleKey.UpArrow:
                                    selectedIndex--;
                                    if (selectedIndex < 0)
                                        selectedIndex = yesNo.Length - 1;
                                    break;
                                case ConsoleKey.DownArrow:
                                    selectedIndex++;
                                    if (selectedIndex > yesNo.Length - 1)
                                        selectedIndex = 0;
                                    break;
                                case ConsoleKey.Enter:
                                    if (yesNo[selectedIndex] == "Yes")
                                    {
                                        currentMenu = Menu.Start;
                                        subRunning = false;
                                    }
                                    else if (yesNo[selectedIndex] == "No")
                                    { currentMenu = Menu.Main; subRunning = false; }
                                    break;
                            }
                        }
                        break;
                }
            }
            break;
        case Menu.Battle:
            Debug.Assert(player != null);
            foreach (Enemy enemy in entities)
            {
                BattleSystem battle = new(player, enemy);
                currentMenu = battle.BattleLoop();
            }
            currentMenu = Menu.Main;
            break;
        case Menu.Character:
            selectedIndex = 0;
            subRunning = true;
            CharMenu charMenu = CharMenu.None;
            try { Console.Clear(); } catch { }

            string[] charOptions = ["Take Damage DEBUG", "Inventory", "Equipped", "Stats"];
            Dictionary<string, CharMenu> charDict = new();
            charDict.Add(charOptions[0], CharMenu.TakeDamage);
            charDict.Add(charOptions[1], CharMenu.Inventory);
            charDict.Add(charOptions[2], CharMenu.Equipped);
            charDict.Add(charOptions[3], CharMenu.Stats);
            while (subRunning)
            {
                Console.Clear();
                Utility.GenerateMenu("CHARACTER MENU");
                Utility.GenerateMenuActions(selectedIndex, charOptions);
                Utility.PrintColor("\n[ESC] - Back To Previous Menu", ConsoleColor.DarkGray);
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex--;
                        if (selectedIndex < 0)
                            selectedIndex = charOptions.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex++;
                        if (selectedIndex > charOptions.Length - 1)
                            selectedIndex = 0;
                        break;
                    case ConsoleKey.Enter:
                        charMenu = charDict[charOptions[selectedIndex]];
                        subRunning = false;
                        break;
                    case ConsoleKey.Escape:
                        currentMenu = Menu.Main;
                        subRunning = false;
                        continue;
                }
                switch (charMenu)
                {
                    case CharMenu.TakeDamage:
                        try { Console.Clear(); } catch { }
                        #pragma warning disable CA1416 // Suppress: Console.Beep is only supported on Windows
                        player!.TakeDamage(player);
                        Console.Beep(700, 400);
                        
                        Utility.PrintColor(player.Info(),ConsoleColor.DarkCyan);
                        if (!player.Alive)
                        {
                            Utility.PrintColor("You died!", ConsoleColor.DarkRed);
                            Console.Beep(100, 1600);
                            currentMenu = Menu.Start;
                        }
                        Console.ReadKey(true);
                        break;
                    case CharMenu.Inventory: player!.CheckInventory(equip: true); break;
                    case CharMenu.Equipped: player!.CheckEquipped(unequip: true); break;
                    case CharMenu.Stats: 
                        Console.Clear();
                        Utility.PrintColor(player!.Info(), ConsoleColor.DarkCyan); 
                        Utility.PrintColor("Press Any Key to continue", ConsoleColor.DarkGray);
                        Console.ReadKey(true);
                        break;
                    default: break;
                }
            }
            break;
        case Menu.Quit:
            selectedIndex = 0;
            running = true;
            yesNo = ["Yes", "No"];
            while (running)
            {
                Console.Clear();
                Utility.GenerateMenu("You are about to quit the program.\nAre you sure?");
                Utility.GenerateMenuActions(selectedIndex, yesNo);
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex--;
                        if (selectedIndex < 0)
                            selectedIndex = yesNo.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex++;
                        if (selectedIndex > yesNo.Length - 1)
                            selectedIndex = 0;
                        break;
                    case ConsoleKey.Enter:
                        if (yesNo[selectedIndex] == "Yes")
                        { running = false; }
                        else if (yesNo[selectedIndex] == "No")
                        { currentMenu = Menu.Start; running = false; }
                        break;
                }
            }
            break;
        case Menu.GameOver:
            Utility.PrintColor("--- Y O U  D I E D ---",ConsoleColor.DarkRed);
            Console.ReadKey(true);
            break;
        default: break;
    }
}
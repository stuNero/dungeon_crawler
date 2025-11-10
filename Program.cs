using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Game;

Player? player = null;
Enemy? goblin1 = null;
Menu currentMenu = Menu.Start;
bool running = true;
bool narration = true;
int selectedIndex = 0;

List<Item> items = new();
items.Add(new Weapon("Longsword", 2, WeaponType.Sword));
items.Add(new Weapon("Bearded Axe", 2, WeaponType.Axe));
items.Add(new Weapon("Quillon Dagger", 2, WeaponType.Dagger));
items.Add(new Weapon("Flanged Mace", 2, WeaponType.Mace));
items.Add(new Consumable("Health Potion", 2));

while (running)
{
    try { Console.Clear(); } catch { }
    bool subRunning;
    switch (currentMenu)
    {
        case Menu.Start:
            subRunning = true;
            goblin1 = new Enemy(name: "Goblin Soldier", maxHP: 5.0, mp: 5, dmg: 1, xp: 100, lvl: 1, 3, "Goblin");
            string[] startOptions = ["START", "QUIT"];

            selectedIndex = 0;
            Dictionary<string, Menu> startMenuOptions = new Dictionary<string, Menu>();
            startMenuOptions.Add(startOptions[0], Menu.Creation);
            startMenuOptions.Add(startOptions[1], Menu.Quit);

            while (subRunning)
            {
                try { Console.Clear(); } catch { }
                Utility.GenerateMenu(title: "D U N G E O N  C R A W L E R");
                Utility.GenerateMenuActions(selectedIndex, startOptions, previousMenu: false);
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
            Utility.PrintColor(
         "         ,&@@@@@&             " +
         "     .@@@@@@@@@@@@@@@         " +
         "    @@@@@@@@@@@@@@@@@@@       " +
         "   &@@@@@@@@@@@@@@@@@@@#      " +
         "  @@@@@@@@@@@@@@@@@@@@@@@     " +
         "   @.      @@@@@      &@      " +
         "   @@@@@@        ,@@@@@@      " +
         "   @@@@@@@@*   &@@@@@@@@      " +
         "   @@@@@@@@*   &@@@@@@@@      " +
         "   @@@@@@@@*   &@@@@@@@@      " +
         "   &@@@@@@@*   &@@@@@@@%      " +
         "        #@@*   &@@/           ",ConsoleColor.Gray);
            subRunning = true;
            Player char1 = new Player(name: "Knight",    maxHP: 25.0, mp: 10, dmg: 1.0, xp: 100, lvl: 1, inventorySize: 4);
            player = char1;
            Player char2 = new Player(name: "Rogue",     maxHP: 10.0, mp: 15, dmg: 2.0, xp: 100, lvl: 1, inventorySize: 6);
            Player char3 = new Player(name: "Barbarian", maxHP: 15.0, mp: 8,  dmg: 1.5, xp: 100, lvl: 1, inventorySize: 5);
            List<Item> tempItems = new();
            foreach (Item item1 in items) { tempItems.Add(item1); }
            selectedIndex = 0;
            string[] yesNo = ["Yes", "No"];
            while (subRunning)
            {
                Console.Clear();
                Utility.GenerateMenu("Skip narration?");
                Utility.GenerateMenuActions(selectedIndex, yesNo, previousMenu: false);
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
            if (narration)
            {
                Console.Clear();
                Utility.Narrate(text: "The hinges of the door creaks and you enter the room,\nyour torch light slowly " +
                "illuminates an unlocked rusty, matte padlock..\n");
                Utility.Narrate(text: "As you step closer you make out the outlines of an\n" +
                "old oak chest which materializes from the black, seemingly infinite void room. ");
            }
            subRunning = true;
            selectedIndex = 0;
            while (player.InventoryRange() < 3 && subRunning)
            {
                List<string> itemList = new();
                // Print available items
                foreach (Item item in tempItems)
                { itemList.Add(item.Info()); }

                string[] itemArray = itemList.ToArray();
                try { Console.Clear(); } catch { }
                Utility.PrintColor(
                "         ,&@@@@@&             \n" +
                "     .@@@@@@@@@@@@@@@         \n" +
                "    @@@@@@@@@@@@@@@@@@@       \n" +
                "   &@@@@@@@@@@@@@@@@@@@#      \n" +
                "  @@@@@@@@@@@@@@@@@@@@@@@     \n" +
                "   @.      @@@@@      &@      \n" +
                "   @@@@@@        ,@@@@@@      \n" +
                "   @@@@@@@@*   &@@@@@@@@      \n" +
                "   @@@@@@@@*   &@@@@@@@@      \n" +
                "   @@@@@@@@*   &@@@@@@@@      \n" +
                "   &@@@@@@@*   &@@@@@@@%      \n" +
                "        #@@*   &@@/           \n",ConsoleColor.DarkGray);
                Utility.GenerateMenu(title: $"\nChoose Your Starting Items ({3 - player.InventoryRange()})");
                Utility.GenerateMenuActions(selectedIndex, itemArray, menuColor: ConsoleColor.DarkMagenta, previousMenu: false);
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
            Console.Clear();
            player.CheckInventory();
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
                Utility.GenerateMenu(title: "MAIN MENU\nPLaying as " + player!.Name);
                Utility.GenerateMenuActions(selectedIndex, mainOptions);
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
                            Utility.GenerateMenuActions(selectedIndex, yesNo, previousMenu: false);
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
            Console.WriteLine("W I P");
            Console.ReadKey(true);
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
                        if (player!.Equipped[0] is Weapon w)
                        {
#pragma warning disable CA1416 // Suppress: Console.Beep is only supported on Windows
                            player!.TakeDamage(w);
                            Console.Beep(700, 400);
                        }
                        Console.WriteLine(player.Info());
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
                    case CharMenu.Stats: Utility.Prompt(player!.Info()); break;
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
                Utility.GenerateMenuActions(selectedIndex, yesNo, previousMenu: false);
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
        default: break;
    }
}
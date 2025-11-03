using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Game;


Player? player1 = null;
Enemy? goblin1 = null;
List<Item> items = new();
items.Add(new Weapon("Longsword", 2, WeaponType.Sword));
items.Add(new Weapon("Bearded Axe", 2, WeaponType.Axe));
items.Add(new Weapon("Quillon Dagger", 2, WeaponType.Dagger));
items.Add(new Weapon("Flanged Mace", 2, WeaponType.Mace));
items.Add(new Consumable("Health Potion", 2));

Menu currentMenu = Menu.Start;
bool running = true;
bool narration = true;
while (running)
{
    try{Console.Clear();} catch{}
    bool subRunning;
    switch (currentMenu)
    {
        case Menu.Start:
            subRunning = true;
            ConsoleKeyInfo keyInput;
            goblin1 = new Enemy(name: "Goblin Soldier", maxHP: 5.0, mp: 5, dmg: 1, xp: 100, lvl: 1, 3, "Goblin");
            string[] startOptions = ["START", "QUIT"];

            int selectedIndex1 = 0;
            Dictionary<string, Menu> startMenuOptions = new Dictionary<string, Menu>();
            startMenuOptions.Add(startOptions[0], Menu.Creation);
            startMenuOptions.Add(startOptions[1], Menu.Quit);

            while (subRunning)
            {
                try {Console.Clear();} catch{}
                Utility.GenerateMenu(title:"D U N G E O N  C R A W L E R");
                Utility.GenerateMenuActions(selectedIndex1, startOptions);
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex1--;
                        if (selectedIndex1 < 0)
                            selectedIndex1 = startOptions.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex1++;
                        if (selectedIndex1 >= startOptions.Length)
                            selectedIndex1 = 0;
                        break;
                    case ConsoleKey.Enter:
                        Console.Clear();
                        currentMenu = startMenuOptions[startOptions[selectedIndex1]];
                        subRunning = false;
                        break;
                }
            }
            break;
        case Menu.Creation:
            List<Item> tempItems = new();
            foreach (Item item1 in items)
            { tempItems.Add(item1); }

            keyInput = Utility.PromptKey("Skip narration?\n(Y/n)");
            if (keyInput.Key == ConsoleKey.Enter) { break; }
            else if (keyInput.Key == ConsoleKey.Y) { narration = false; }
            
            player1 = new Player(name: "Max", maxHP: 20.0, mp: 10, dmg: 1.0, xp: 100, lvl: 1, inventorySize: 5);
            if (narration)
            {
                Utility.Narrate(text: "The hinges of the door creaks and you enter the room,\nyour torch light slowly " +
                "illuminates an unlocked rusty, matte padlock..\n");
                Utility.Narrate(text: "As you step closer you make out the outlines of an\n" +
                "old oak chest which materializes from the black, seemingly infinite void room. ");
            }
            while (player1.InventoryRange() < 3)
            {
                subRunning = true;
                int selectedItemIndex = 0;
                while (subRunning)
                {
                    List<string> itemList = new();
                    foreach (Item item in tempItems)
                    {
                        itemList.Add(item.Info());
                    }
                    string[] itemArray = itemList.ToArray();
                    try {Console.Clear();} catch {}
                    Utility.GenerateMenu(title: $"\nChoose Your Starting Items ({player1.InventoryRange()})");
                    Utility.GenerateMenuActions(selectedItemIndex, itemArray);
                    player1.CheckInventory();
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.UpArrow:
                            selectedItemIndex--;
                            if (selectedItemIndex < 0)
                                selectedItemIndex = itemArray.Length - 1;
                            break;
                        case ConsoleKey.DownArrow:
                            selectedItemIndex++;
                            if (selectedItemIndex >= itemArray.Length)
                                selectedItemIndex = 0;
                            break;
                        case ConsoleKey.Enter:
                            Console.Clear();
                            subRunning = false;
                            player1.AddItem(tempItems[selectedItemIndex]);
                            tempItems.Remove(tempItems[selectedItemIndex]);
                            break;
                        case ConsoleKey.Escape:
                            subRunning = false;
                            break;
                    }
                }
            }
            if (narration)
            {
                Utility.Narrate("You delve into the depths of the dungeon...");
            }
            currentMenu = Menu.Main;
            break;
        case Menu.Main:
            subRunning = true;
            int selectedIndex = 0;
            string[] mainOptions = ["Attack enemy WIP", "Character"];
            Dictionary<string, Menu> menuOptions = new Dictionary<string, Menu>();
            menuOptions.Add(mainOptions[0], Menu.Battle);
            menuOptions.Add(mainOptions[1], Menu.Character);
            while (subRunning)
            {
                Console.Clear();
                Utility.GenerateMenuActions(selectedIndex, mainOptions);
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex--;
                        if (selectedIndex < 0)
                            selectedIndex = mainOptions.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex++;
                        if (selectedIndex >= mainOptions.Length)
                            selectedIndex = 0;
                        break;
                    case ConsoleKey.Enter:
                        subRunning = false;
                        currentMenu = menuOptions[mainOptions[selectedIndex]];
                        break;
                    case ConsoleKey.Escape:
                        string input = Utility.Prompt("Are you sure?(Y/n)");
                        if (input.ToLower() == "y")  
                        { currentMenu = Menu.Start; subRunning = false; }
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
            CharMenu charMenu = CharMenu.None;
            try{Console.Clear();} catch{}
            Utility.GenerateMenu(title: "Choose an option: ", choices: new[] { "Take Damage DEBUG","Inventory", "Equipped","Stats" });
            keyInput = Utility.PromptKey("",clear:false);
            if (keyInput.Key == ConsoleKey.Enter) { currentMenu = Menu.Main;  break; }
            switch (keyInput.Key)
            {
                case ConsoleKey.D1: charMenu = CharMenu.TakeDamage; break;
                case ConsoleKey.D2: charMenu = CharMenu.Inventory;  break;
                case ConsoleKey.D3: charMenu = CharMenu.Equipped;   break;
                case ConsoleKey.D4: charMenu = CharMenu.Stats;      break;
            }
            switch (charMenu)
            {
                case CharMenu.TakeDamage:
                    try { Console.Clear(); } catch { }
                    if (player1!.Equipped[0] is Weapon w)
                    { player1!.TakeDamage(w); }
                    Console.WriteLine(player1.Info());
                    if (!player1.Alive)
                    {
                        Utility.PrintColor("You died!", ConsoleColor.DarkRed);
                        currentMenu = Menu.Start;
                    }
                    Console.ReadKey(true);
                    break;
                case CharMenu.Inventory: player1!.CheckInventory(equip:true); break;
                case CharMenu.Equipped: player1!.CheckEquipped(); break;
                case CharMenu.Stats: Utility.Prompt(player1!.Info()); break;
                default: break;
            }
            break;
        case Menu.Quit:
            keyInput = Utility.PromptKey("Are you sure?");
            if (keyInput.Key == ConsoleKey.Enter) { break; }else if (keyInput.Key == ConsoleKey.Y){ Environment.Exit(0); }
            break;
        default: break;
    }
}
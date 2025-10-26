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
    string choice = "";
    switch (currentMenu)
    {
        case Menu.Start:
            goblin1 = new Enemy(name: "Goblin Soldier", maxHP: 5.0, mp: 5, dmg: 1, xp: 100, lvl: 1, 3, "Goblin");
            
            Utility.GenerateMenu(title: "D U N G E O N  C R A W L E R", choices: new[] { "START", "QUIT" });
            int.TryParse(Console.ReadLine(), out int input);
            switch (input)
            {
                case 1:
                    choice = Utility.Prompt("Skip narration?\n(Y/n)");
                    if (string.IsNullOrWhiteSpace(choice)) { break; }
                    if (choice == "y") {narration = false;}
                    currentMenu = Menu.Creation;
                    break;
                case 2:
                    currentMenu = Menu.Quit;
                    break;
            }
            break;
        case Menu.Creation:
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
                Utility.GenerateMenu(title: "Choose Your Starting Items");
                for (int i = 0; i < items.Count; i++)
                {
                    if (!player1.Inventory.Contains(items[i]))
                    {
                        Console.WriteLine($"[{i + 1}]");
                        Console.WriteLine(items[i].Info());
                    }
                }
                choice = Utility.Prompt(">", clear: false);
                if (string.IsNullOrWhiteSpace(choice)) { break; }
                int.TryParse(choice, out input);
                if (!player1.InInventoryRange(input))
                { break; }
                player1.AddItem(items[input - 1]);
                try{Console.Clear();} catch{}
            }
            Utility.Narrate("You delve into the depths of the dungeon...");
            currentMenu = Menu.Main;
            break;
        case Menu.Main:
            Utility.GenerateMenu(title: "Choose an option: ", choices: new[] { "Attack enemy WIP", "Character", "Leave" });
            int.TryParse(Console.ReadLine(), out input);
            switch (input)
            {
                case 1: currentMenu = Menu.Battle;    break;
                case 2: currentMenu = Menu.Character; break;
                case 3:
                    choice = Utility.Prompt("Are you sure?(y/n)");
                    if(string.IsNullOrWhiteSpace(choice)) { break; }
                    currentMenu = Menu.Start;      
                    break;
            }
            break;
        case Menu.Battle:
            // BattleSystem battle = new BattleSystem(player1, goblin1);
            currentMenu = Menu.Main;
            break;
        case Menu.Character:
            CharMenu charMenu = CharMenu.None;
            try{Console.Clear();} catch{}
            Utility.GenerateMenu(title: "Choose an option: ", choices: new[] { "Take Damage DEBUG","Inventory", "Equipped","Stats" });
            choice = Utility.Prompt("",clear:false);
            if (string.IsNullOrWhiteSpace(choice)) { currentMenu = Menu.Main;  break; }
            int.TryParse(choice, out input);
            switch (input)
            {
                case 1: charMenu = CharMenu.TakeDamage; break;
                case 2: charMenu = CharMenu.Inventory;  break;
                case 3: charMenu = CharMenu.Equipped;   break;
                case 4: charMenu = CharMenu.Stats;      break;
            }
            switch (charMenu)
            {
                case CharMenu.TakeDamage:
                    try { Console.Clear(); } catch { }
                    if (player1!.Equipped[0] is Weapon w)
                    {
                        player1!.TakeDamage(w);
                    }
                    Console.WriteLine(player1.Info());
                    if (!player1.Alive)
                    {
                        Utility.PrintColor("You died!", ConsoleColor.DarkRed);
                        currentMenu = Menu.Start;
                    }
                    Console.ReadKey(true);
                    break;
                case CharMenu.Inventory:
                    player1!.CheckInventory();
                    break;
                case CharMenu.Equipped:
                    player1!.CheckEquipped();
                    break;
                case CharMenu.Stats:
                    Utility.Prompt(player1!.Info());
                    break;
                default:
                    Utility.Error("Something went wrong in sub-menu input");
                    break;
            }
            break;
        case Menu.Quit:
            choice = Utility.Prompt("Are you sure?");
            if (string.IsNullOrWhiteSpace(choice)) { break; }else { Environment.Exit(0); }

            break;
        default:Utility.Error("Something went wrong in menu input"); break;
    }
}
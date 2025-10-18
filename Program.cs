using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Game;

List<Item> items = new();
items.Add(new Weapon("Longsword", 2, WeaponType.Sword));
items.Add(new Weapon("Bearded Axe", 2, WeaponType.Axe));
items.Add(new Weapon("Quillon Dagger", 2, WeaponType.Dagger));
items.Add(new Weapon("Flanged Mace", 2, WeaponType.Mace));
items.Add(new Consumable("Health Potion", 2));

Player? player1 = null;
Enemy? goblin1 = null;

Consumable? healthPotion = null;

Menu currentMenu = Menu.Start;
bool running = true;
while (running)
{
    Console.Clear();
    string choice = "";
    switch (currentMenu)
    {
        case Menu.Start:
            goblin1 = new Enemy(name: "Goblin Soldier", maxHP: 5, mp: 5, dmg: 1, xp: 100, lvl: 1, 3, "Goblin");
            
            Utility.GenerateMenu(title: "D U N G E O N  C R A W L E R", choices: new[] { "START", "QUIT" });
            int.TryParse(Console.ReadLine(), out int input);
            switch (input)
            {
                case 1:
                    currentMenu = Menu.Creation;
                    break;
                case 2:
                    currentMenu = Menu.Quit;
                    break;
            }
            break;
        case Menu.Creation:
            player1 = new Player(name:"Max", maxHP:20, mp:10, dmg:1, xp:100, lvl:1, inventorySize:5);

            Utility.GenerateMenu(title: "Choose Your Starting Items");
            // bool chooseItems = true;
            // while (chooseItems)
            for(int i = 0; i<items.Count;i++)
            {
                Console.WriteLine($"[{i + 1}]");
                Console.WriteLine(items[i].Info());
            }
            Console.ReadLine();

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
            Console.Clear();
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
                    Console.Clear();
                    player1!.TakeDamage(player1.Dmg);
                    Console.WriteLine(player1.Info());
                    Utility.Error($"Oof.. {player1.Name} hit himself with {player1.Equipped.ElementAtOrDefault(0)?.Name ?? "his fist"}!\nHe took {player1.Dmg} damage..");
                    if (!player1.Alive)
                    {
                        Utility.Error("You died!");
                        currentMenu = Menu.Start;
                    }
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
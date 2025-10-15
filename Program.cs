using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Game;

Item longsword = new Item("Longsword", "weapon");
longsword.DefineItem(2);
Item healthPotion = new Item("Health Potion", "consumable");
healthPotion.DefineItem(3);

Player player1 = new Player(name:"Max", maxHP:20, mp:10, dmg:2, xp:100, level:1, inventorySize:5);
player1.AddItem(longsword);
player1.AddItem(healthPotion);

Enemy goblin1 = new Enemy("Goblin Soldier", 5, 5, 2, 100, 1, 3, "Goblin");

Menu currentMenu = Menu.Start;
bool running = true;
while (running)
{
    Console.Clear();
    string choice = "";
    switch (currentMenu)
    {
        case Menu.Start:
            Utility.GenerateMenu(title: "D U N G E O N  C R A W L E R", choices: new[] { "START", "QUIT" });
            int.TryParse(Console.ReadLine(), out int input);
            switch (input)
            {
                case 1:
                    currentMenu = Menu.Main;
                    break;
                case 2:
                    currentMenu = Menu.Quit;
                    break;
            }
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
            BattleSystem battle = new BattleSystem(player1, goblin1);
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
                    player1.TakeDamage(player1.Dmg);
                    Console.WriteLine(player1.Info());
                    Utility.Error($"Oof.. {player1.Name} hit himself with {player1.Equipped.ElementAtOrDefault(0)?.Name ?? "his fist"}!\nHe took {player1.Dmg} damage..");
                    if (!player1.Alive)
                    {
                        Utility.Error("You died!");
                        currentMenu = Menu.Start;
                    }
                    break;
                case CharMenu.Inventory:
                    choice = Utility.Prompt(player1.CheckInventory());
                    if (string.IsNullOrWhiteSpace(choice)) { break; }
                    int.TryParse(choice, out int nr);

                    Console.Clear();
                    Console.WriteLine(player1.Inventory[nr - 1]!.Info());

                    choice = Utility.Prompt("Equip?(y/n)", clear: false);
                    if (string.IsNullOrWhiteSpace(choice)) { break; }
                    if (choice == "y") { player1.EquipItem(player1.Inventory[nr - 1]!); }
                    else { break; }
                    break;
                case CharMenu.Equipped:
                    choice = Utility.Prompt(player1.CheckEquipped());
                    if (string.IsNullOrWhiteSpace(choice)) { break; }
                    int.TryParse(choice, out nr);
                    Console.Clear();
                    Console.WriteLine(player1.Equipped[nr - 1]!.Info());

                    choice = Utility.Prompt("Unequip?(y/n)", clear:false);
                    if (string.IsNullOrWhiteSpace(choice)) { break; }
                    if (choice == "y") { player1.UnEquipItem(player1.Equipped[nr-1]!); }
                    else { break; }
                    break;
                case CharMenu.Stats:
                    Utility.Prompt(player1.Info());
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
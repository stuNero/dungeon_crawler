using System.Xml.Serialization;
using Game;

Item longsword = new Item("Longsword", "weapon");
longsword.DefineItem(2);
Item healthPotion = new Item("Health Potion", "consumable");
healthPotion.DefineItem(3);

Player player1 = new Player("Max", 20, 10, 2, 100, 1, 5, 20);
player1.AddItem(longsword);
player1.AddItem(healthPotion);

Enemy goblin1 = new Enemy("Goblin Soldier", 5, 5, 2, 100, 1, 3, "Goblin", 5);

bool running = true;
while (running)
{
    string choice = "";
    Console.Clear();
    Utility.GenerateMenu(title: "Choose an option: ", choices: new[] { "Attack enemy WIP","Take Damage", "Character", "Leave" });
    int.TryParse(Console.ReadLine(), out int input);
    switch (input)
    {
        case 1:
            BattleSystem battle = new BattleSystem(player1, goblin1);
            break;
        case 2:
            Console.Clear();
            bool check = false;
            foreach (Item item in player1.Equipped)
            {
                if (item != null)
                {
                    if (item.Type == "weapon")
                    {
                        player1.Hp -= item.Dmg;
                        check = true;
                        Utility.Error($"Oof.. {player1.Name} stabbed himself with {item.Name}!\nHe took {item.Dmg} damage..");
                        break;
                    }
                }
            }
            if (!check)
            {
                Utility.Error("No weapon selected");
                break;
            }
            Utility.Prompt("");
            break;
        case 3:
            Console.Clear();
            Utility.GenerateMenu(title: "Choose an option: ", choices: new[] { "Equip Item", "Unequip item","Inventory", "Equipped","Check stats" });
            choice = Utility.Prompt("",clear:false);
            if (string.IsNullOrWhiteSpace(choice)) { break; }
            int.TryParse(choice, out input);
            switch (input)
            {
                case 1:
                    choice = Utility.Prompt(player1.CheckInventory());
                    if (string.IsNullOrWhiteSpace(choice)) { break; }
                    int.TryParse(choice,out input);
                    player1.EquipItem(player1.Inventory[input-1]);
                    break;
                case 2:
                    choice = Utility.Prompt(player1.CheckEquipped());
                    if (string.IsNullOrWhiteSpace(choice)) { break; }
                    int.TryParse(choice,out input);
                    player1.UnEquipItem(player1.Equipped[input-1]);
                    break;
                case 3:
                    choice = Utility.Prompt(player1.CheckInventory());
                    if (string.IsNullOrWhiteSpace(choice)) { break; }
                    int.TryParse(choice, out int nr);
                    Console.WriteLine(player1.Inventory[nr - 1].Info());
                    Console.ReadLine();
                    break;
                case 4:
                    Utility.Prompt(player1.CheckEquipped());
                    break;
                case 5:
                    Utility.Prompt(player1.Info());
                    break;
                default:
                    Utility.Error("Something went wrong in sub-menu input");
                    break;
            }
            break;
        case 4:
            choice = Utility.Prompt("Are you sure?");
            if (string.IsNullOrWhiteSpace(choice)) { break; }
            break;
        default:
            Utility.Error("Something went wrong in menu input");
            break;
    }
}
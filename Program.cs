using Game;
Console.Clear();

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
    Utility.GenerateMenu(title: "Choose an option: ", choices: new[] { "Attack enemy", "Character", "Leave" });
    int.TryParse(Console.ReadLine(), out int input);
    switch (input)
    {
        case 1:
            BattleSystem battle = new BattleSystem(player1, goblin1, true);
            break;
        case 2:
            Console.Clear();
            Utility.GenerateMenu(title: "Choose an option: ", choices: new[] { "[Equip Item]", "[Unequip item]","[Inventory]", "[Equipped]","[Check stats]" });
            int.TryParse(Console.ReadLine(), out input);
            switch (input)
            {
                case 1:
                    Console.WriteLine(player1.CheckInventory());
                    int.TryParse(Console.ReadLine(),out input);
                    player1.EquipItem(player1.Inventory[input-1]);
                    break;
                case 2:
                    Console.WriteLine(player1.CheckEquipped());
                    int.TryParse(Console.ReadLine(),out input);
                    player1.UnEquipItem(player1.Equipped[input-1]);
                    break;
                case 3:
                    Console.WriteLine(player1.CheckInventory());
                    Console.ReadLine();
                    break;
                case 4:
                    Console.WriteLine(player1.CheckEquipped());
                    Console.ReadLine();
                    break;
                case 5:
                    Console.WriteLine(player1.Info());
                    Console.ReadLine();
                    break;
                default:
                    Utility.Error("Something went wrong in sub-menu input");
                    break;
            }
            break;
        case 3:
            Utility.Prompt("Are you sure?");
            string quitInput = Console.ReadLine();
            if (quitInput.ToLower() != "exit")
            {
                running = false;
            }

            break;
        default:
            Utility.Error("Something went wrong in menu input");
            break;
    }
}
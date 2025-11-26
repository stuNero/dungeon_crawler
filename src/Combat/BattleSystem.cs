namespace Game;

class BattleSystem
{
    Entity Attacker;
    Entity Defender;
    public int turns;
    bool BattleActive;
    public BattleSystem(Entity attacker, Entity defender)
    {
        Attacker = attacker;
        Defender = defender;
        BattleActive = true;
        if (!defender.Alive || !attacker.Alive) {return;}
        if (Attacker is Enemy)
        {
            Console.WriteLine($"A wild {Attacker.Name} appears!");
        }
        else if (Defender is Enemy)
        {
            Utility.PrintColor($"A wild {Defender.Name} appears!", ConsoleColor.DarkRed);
        }
        Console.ReadKey(true);
    }
    public Menu BattleLoop()
    {
        Menu currentMenu = Menu.Main;
        while (BattleActive)
        {
            if (!Attacker.Alive) { currentMenu = EndBattle(Defender, Attacker); return currentMenu; }
            if (!Defender.Alive) { currentMenu = EndBattle(Attacker, Defender); return currentMenu; }

            turns += 1;

            if (turns % 2 != 0) { Attacker.TakeTurn(Defender); }
            else { Defender.TakeTurn(Attacker); }
        }
        return currentMenu;
    }
    Menu EndBattle(Entity winner, Entity killed)
    {
        BattleActive = false;
        if (winner is Player)
        {
            int selectedIndex = 0;
            bool subRunning = true;
            string[] yesNo = ["Loot", "Leave"];
            while (subRunning)
            {
                Console.Clear();
                Utility.GenerateMenu(killed.Name + "'s body is lifeless on the floor.");
                Utility.GenerateMenuActions(selectedIndex, yesNo);
                switch (Console.ReadKey(true).Key)
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
                        if (yesNo[selectedIndex] == "Loot")
                        {
                            winner.Loot(killed);
                            subRunning = false;
                        }
                        else if (yesNo[selectedIndex] == "Leave")
                        { subRunning = false; }
                        break;
                }
            }
            return Menu.Main;
        }
        else
        {
            return Menu.GameOver;
        }
    }
}
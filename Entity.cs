namespace Game;

using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;

abstract class Entity
{
    public string Name;
    public bool Alive;
    public double Hp;
    public double MaxHP;
    public int InventorySize;
    public Item?[] Inventory;
    public Entity(string name, double maxHP, int inventorySize)
    {
        Name = name;
        Hp = maxHP;
        MaxHP = maxHP;
        Alive = true;
        InventorySize = inventorySize;
        Inventory = new Item[InventorySize];
    }
    public void SortInventory()
    {
        // Puts all items to beginning of array
        Item[] temp = new Item[InventorySize];
        foreach (Item? item in Inventory)
        {
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] == null)
                {
                    if (item != null)
                    { temp[i] = item; }
                    break;
                }
            }
        }
        Array.Clear(Inventory, 0, Inventory.Length);
        Inventory = temp;
    }
    public virtual void CheckInventory(bool equip = false)
    {
        string txt = "--Your Inventory--\n";

        // Checking inventory: 
        for (int i = 0; i < InventorySize; i++)
        {
            txt += $"[{i + 1}] [{Inventory.ElementAtOrDefault(i)?.Name ?? "Empty Slot"}]\n";
        }
        Console.WriteLine(txt);
    }
    public virtual string Info()
    {
        string txt = "___________________\n";

        txt += $"Name: [{Name}]\n"
             + $"HP:   [{Hp}]\n";
        return txt;
    }
    public int InventoryRange()
    {
        int amount = 0;
        for(int i = 0; i<Inventory.Length;++i)
        {
            if (Inventory[i] != null)
            {
                amount += 1;
            }
        }
        return amount;
    }
    public bool InInventoryRange(int input)
    {
        return input > 0 && input <= InventorySize;
    }
    public void AddItem(Item item)
    {
        for (int i = 0; i < InventorySize; i++)
        {
            if (Inventory[i] == null)
            {
                Inventory[i] = item;
                Debug.Assert(Inventory[i] != null);
                break;
            }
        }
    }
    public virtual void Loot(Entity victim) {}
    public void TakeDamage(Entity enemy)
    {
        double dmg = 0;
        Item? weapon = null;
        if (enemy is Player p)
        {
            dmg = p.Dmg;
            weapon = p.Equipped[0];
        }
        else if (enemy is Enemy e)
        {
            dmg = e.Dmg;
            weapon = e.Inventory[0];
        }
        if (weapon != null)
        {
            if (weapon is Weapon w)
            {
                dmg = w.CritCheck(dmg);
            }
        }
        if (this is Player) { Utility.PrintColor("\nYou took " + dmg + " damage!", ConsoleColor.DarkRed); }
        else { Utility.PrintColor("\n" + this.Name + " took " + dmg + " damage!", ConsoleColor.DarkGreen); }
        this.Hp -= dmg;
        if (this.Hp <= 0) { this.Alive = false; }
        Console.ReadKey(true);
    }
    public virtual void TakeTurn(Entity opponent)
    {
        bool subRunning = true;
        int selectedIndex = 0;
        while (subRunning)
        {
            Console.Clear();
            Utility.GenerateMenu("BATTLE\n" + this.Name + "'s turn...");
            if (this is Player)
            {
                string[] battleOptions = ["Attack", "Pass"];
                Utility.PrintColor(opponent.Info(), ConsoleColor.DarkRed);
                Utility.PrintColor(this.Info(), ConsoleColor.DarkGreen);
                Utility.GenerateMenuActions(selectedIndex, battleOptions);

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex--;
                        if (selectedIndex < 0)
                            selectedIndex = battleOptions.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex++;
                        if (selectedIndex > battleOptions.Length - 1)
                            selectedIndex = 0;
                        break;
                    case ConsoleKey.Enter:
                        if (battleOptions[selectedIndex] == "Attack")
                        {
                            opponent.TakeDamage(this);
                            subRunning = false;
                        }
                        else if (battleOptions[selectedIndex] == "Pass") 
                        {subRunning = false;}
                        break;
                }
            }
            else
            {
                Console.WriteLine(this.Name + " swings at you!");
                opponent.TakeDamage(this);
                subRunning = false;
            }
        }
    }
}
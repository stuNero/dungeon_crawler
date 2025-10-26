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
    public void ReceiveItem(List<Item> items)
    {
        string txt = "Loot:\n";
        txt += "________________\n";
        for (int i = 0; i < items.Count; i++)
        {
            txt += $"[{i}] '{items[i]}'\n";
        }
        txt += "________________ ";
        string? input = "";
        while (input!.ToLower() != "exit")
        {
            try{Console.Clear();} catch{}
            Utility.Prompt(txt);
            input = Console.ReadLine();
            int.TryParse(input, out int output);
            AddItem(items[output]);
        }
    }
    public List<Item> TransferItem(Item item)
    {
        List<Item> items = new List<Item>();
        for (int i = 0; i < InventorySize; i++)
        {
            if (Inventory[i] != null)
            {
                items.Add(Inventory[i]!);
                Inventory[i] = null;
            }
        }
        return items;
    }
    public void DiscardItem(Item item)
    {
        for (int i = 0; i < InventorySize; i++)
        {
            if (Inventory[i] != null)
            {
                if (item == Inventory[i])
                {
                    Inventory[i] = null;
                    break;
                }
            }
        }
    }
    public virtual void TakeDamage(Weapon weapon)
    {
        bool crit = false;
        double dmg = weapon.Value;
        crit = weapon.CritCheck();
        double critDamage = 0;
        if (crit)
        {
            critDamage = weapon.Crit();
            Utility.PrintColor(weapon.Name + " crit for " + (critDamage - dmg) + " damage!", ConsoleColor.DarkRed);
            dmg = critDamage;
        }
        if (this is Player) {Utility.PrintColor("You took " + dmg + " damage!", ConsoleColor.DarkRed);}
        else { Utility.PrintColor(this.Name + " took " + dmg + " damage!", ConsoleColor.DarkGreen);}
        this.Hp -= dmg;
        if (this.Hp <= 0) {this.Alive = false;}
    }
    public virtual void TakeTurn(Entity opponent)
    {
        
    }
}
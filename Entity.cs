namespace Game;

using System.Net;
using System.Security.Cryptography;
using MyUtilities;

abstract class Entity
{
    public string Name;
    public bool Alive;
    public int Hp;
    public int InventorySize;
    public Item[] Inventory;
    public Entity(string name, int hp, int inventorySize)
    {
        Name = name;
        Hp = hp;

        InventorySize = inventorySize;
        Inventory = new Item[InventorySize];

        if (Hp > 0)
        { Alive = true; }
        else
        { Alive = false; }
    }
    public void AddItem(Item item)
    {
        for (int i = 0; i < InventorySize; i++)
        {
            if (Inventory[i] == null)
            {
                Inventory[i] = item;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(item.Name + " put into inventory!");
                Console.ResetColor();
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
        while (input.ToLower() != "exit")
        {
            Console.Clear();
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
                items.Add(Inventory[i]);
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
    public virtual void TakeTurn(Entity opponent)
    {
        
    }
}
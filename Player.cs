namespace Game;

class Player : Actor
{
    public Item[] Equipped = new Item[3];
    public Player(string name, int hp, int mp, int dmg, int xp, int level, int inventorySize, int maxHP)
                : base(name, hp, mp, dmg, xp, level, inventorySize, maxHP)
    {

    }
    public string CheckInventory()
    {
        string txt = "";

        for (int i = 0; i < InventorySize; i++)
        {
            if (Inventory[i] != null)
            {
                txt += $"[{i + 1}] [{Inventory[i].Name}]\n";
            }
        }
        if (txt != "" || txt != null)
        {
            return txt;
        }
        else
        {
            return "Inventory is empty...";
        }
    }
    public string CheckEquipped()
    {
        string txt = "";

        for (int i = 0; i < Equipped.Length; i++)
        {
            if (Equipped[i] != null)
            {
                txt += $"[{i + 1}] [{Equipped[i].Name}]\n";
            }
        }
        if (txt != "" || txt != null)
        {
            return txt;
        }
        else
        {
            return "Inventory is empty...";
        }
    }
    public void UnEquipItem(Item item)
    {
        try
        {
            for (int i = 0; i < Equipped.Length; i++)
            {
                if (Equipped[i] == item)
                {
                    AddItem(Equipped[i]);
                    Equipped[i] = null;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(item.Name + "unequipped!");
                    Console.ResetColor();
                    Thread.Sleep(2000);
                    break;
                }
            }
        }
        catch
        { Utility.Error("Something went wrong with unequipping item"); }
    }
    public void EquipItem(Item item)
    {
        void Equip(Item item)
        {
            for (int i = 0; i < Equipped.Length; i++)
            {
                if (Equipped[i] != null)
                {
                    AddItem(Equipped[i]);
                    Equipped[i] = item;
                    break;
                }
                else
                {
                    Equipped[i] = item;
                    break;
                }
            }
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(item.Name + " equipped!");
            Console.ResetColor();
            Thread.Sleep(2000);
        }
        try
        {
            switch (item.Type)
            {
                case "weapon":
                    if (!item.Equipped)
                    {
                        Equip(item);
                        item.Equipped = true;
                        this.Dmg += item.Dmg;
                    }
                    break;
                case "consumable":
                    this.Hp += item.Value;
                    if (this.Hp > this.MaxHP)
                    {
                        this.Hp = this.MaxHP;
                    }
                    Equip(item);
                    break;
            }
        }
        catch
        { Utility.Error("Something went wrong with equipping item"); }
    }
    public override void TakeTurn(Entity opponent)
    {
        base.TakeTurn(opponent);
        Console.WriteLine(this.Name + "'s turn!");

    }
}
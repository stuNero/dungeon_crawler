namespace Game;

class Player : Actor
{
    public Item[] Equipped = new Item[3];
    public Player(string name, int maxHP, int mp, int dmg, int xp, int level, int inventorySize)
                : base(name, maxHP, mp, dmg, xp, level, inventorySize)
    { }
    public string CheckInventory()
    {
        // Puts all items to beginning of array
        Item[] temp = new Item[InventorySize];
        foreach (Item item in Inventory)
        {
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] == null)
                {
                    temp[i] = item;
                    break;
                }
            }
        }
        Array.Clear(Inventory, 0, Inventory.Length);
        Inventory = temp;

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
        for (int i = 0; i < Equipped.Length; i++)
        {
            if (Equipped[i] == item)
            {
                AddItem(Equipped[i]);
                Equipped[i] = null;
                Utility.Success(item.Name + "unequipped!");
                for (int j = 0; j < Inventory.Length; j++)
                {
                    if (Inventory[i] == null)
                    {
                        Inventory[i] = item;
                    }
                }
                break;
            }
        }
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
            for (int i = 0; i<Inventory.Count(); i++)
            {
                if (Inventory[i] == item)
                {
                    Inventory[i] = null;
                }
            }
            Utility.Success(item.Name + " equipped!");
        }
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
    public override void TakeTurn(Entity opponent)
    {
        base.TakeTurn(opponent);
        Console.WriteLine(this.Name + "'s turn!");

    }
}
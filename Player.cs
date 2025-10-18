namespace Game;
using System.Diagnostics;
class Player : Actor
{
    public Item?[] Equipped = new Item?[3];
    public Player(string name, int maxHP, int mp, int dmg, int xp, int lvl, int inventorySize)
                : base(name, maxHP, mp, dmg, xp, lvl, inventorySize)
    { }
    public void CheckInventory()
    {
        string Display()
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

            string txt = "";

            // Checking inventory: 
            for (int i = 0; i < InventorySize; i++)
            {
                txt += $"[{i + 1}] [{Inventory.ElementAtOrDefault(i)?.Name ?? "Empty Slot"}]\n";
            }
            return txt;
        }
        string choice = Utility.Prompt(Display());
        if (string.IsNullOrWhiteSpace(choice)) { return; }
        int.TryParse(choice, out int nr);
        if (nr-1 > Inventory.Length) {Utility.Error("No item selected!"); return; }
        if (this.Inventory[nr - 1] != null)
        {
            Console.Clear();
            Console.WriteLine(this.Inventory[nr - 1]!.Info());

            choice = Utility.Prompt("Equip?(y/n)", clear: false);
            if (string.IsNullOrWhiteSpace(choice)) { return; }
            if (choice == "y") { this.EquipItem(this.Inventory[nr - 1]!); }
            else { return; }
        }
        else
        {
            Utility.Error("No item selected!");
        }
                    
    }
    public void CheckEquipped()
    {
        string Display()
        {
            string txt = "";

            for (int i = 0; i < Equipped.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        txt += $"[{i + 1}] [{Equipped.ElementAtOrDefault(i)?.Name ?? "Primary Weapon - Empty Slot"}]\n";
                        break;
                    case 1:
                        txt += $"[{i + 1}] [{Equipped.ElementAtOrDefault(i)?.Name ?? "Off Hand       - Empty Slot"}]\n";
                        break;
                    case 2:
                        txt += $"[{i + 1}] [{Equipped.ElementAtOrDefault(i)?.Name ?? "Consumable     - Empty Slot"}]\n";
                        break;
                }
            }
            return txt;
        }
        string choice = Utility.Prompt(Display());
        if (string.IsNullOrWhiteSpace(choice)) { return; }
        int.TryParse(choice, out int nr);
        if (nr-1 > Equipped.Length) {Utility.Error("No item selected!"); return; }
        if (this.Equipped[nr - 1] != null)
        {
            Console.Clear();
            Console.WriteLine(this.Equipped[nr - 1]!.Info());

            choice = Utility.Prompt("Unequip?(y/n)", clear: false);
            if (string.IsNullOrWhiteSpace(choice)) { return; }
            if (choice == "y") { this.UnEquipItem(this.Equipped[nr - 1]!); }
            else { return; }
        }
        else
        {
            Utility.Error("No item selected!");
        } 
    }
    public void UnEquipItem(Item item)
    {
        void UnEquip(Item item)
        {
            for (int i = 0; i < Equipped.Length; i++)
            {
                if (Equipped[i] == item)
                {
                    Debug.Assert(Equipped[i] != null);
                    AddItem(Equipped[i]!);
                    Equipped[i] = null;
                    Utility.Success(item.Name + " unequipped!");
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
        if(item.Type == "weapon")
        {
            UnEquip(item);
            this.Dmg -= item.Dmg;
        }
    }
    public void EquipItem(Item item)
    {
        void Equip(Item item)
        {
            for (int i = 0; i < Equipped.Length; i++)
            {
                if (item.Type != "consumable")
                {
                    if (Equipped[i] != null)
                    {
                        AddItem(Equipped[i]!);
                        Equipped[i] = item;
                        break;
                    }
                    else if (Equipped[i] == null)
                    {
                        Equipped[i] = item;
                        break;
                    }
                }
            }
            for (int i = 0; i < Inventory.Count(); i++)
            {
                if (Inventory[i] == item)
                {
                    Inventory[i] = null;
                }
            }
        }
        switch (item.Type)
        {
            case "weapon":
                Equip(item);
                this.Dmg += item.Dmg;
                Utility.Success(item.Name + " equipped!");
                break;
            case "consumable":
                int restoredHP = item.Value;
                int leftOverHP = 0;
                this.Hp += item.Value;
                if (this.Hp > this.MaxHP)
                {
                    leftOverHP = this.Hp - MaxHP;
                    this.Hp = this.MaxHP;
                }
                restoredHP = restoredHP - leftOverHP;
                Equip(item);
                Utility.Success($"{this.Name} restored {restoredHP}!");
                break;
        }
    }
    public override void TakeTurn(Entity opponent)
    {
        base.TakeTurn(opponent);
        Console.WriteLine(this.Name + "'s turn!");

    }
}
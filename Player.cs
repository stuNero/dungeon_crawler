namespace Game;
using System.Diagnostics;
/// <summary>
/// Represents the player-controlled <see cref="Actor"/>.
/// </summary>
/// <remarks>
/// The <see cref="Player"/> manages an inventory and equipped items and
/// provides helper methods to inspect, equip, and unequip items. Equipment
/// affects player stats (for example, equipping a <see cref="Weapon"/>
/// modifies <see cref="Actor.Dmg"/>).
/// </remarks>
class Player : Actor
{
    /// <summary>
    /// The currently equipped items. Index mapping:
    /// [0] Primary weapon, [1] Off-hand, [2] Consumable.
    /// Elements may be <c>null</c> when the slot is empty.
    /// </summary>
    public Item?[] Equipped = new Item?[3];
    /// <summary>
    /// Initializes a new instance of the <see cref="Player"/> class.
    /// </summary>
    /// <param name="name">Player's display name.</param>
    /// <param name="maxHP">Maximum health points for the player.</param>
    /// <param name="mp">Mana/energy points for the player.</param>
    /// <param name="dmg">Base damage value for the player.</param>
    /// <param name="xp">Starting experience points.</param>
    /// <param name="lvl">Starting level.</param>
    /// <param name="inventorySize">Size of the player's inventory (number of slots).</param>
    public Player(string name, double maxHP, int mp, double dmg, int xp, int lvl, int inventorySize)
                : base(name, maxHP, mp, dmg, xp, lvl, inventorySize)
    { }
    /// <summary>
    /// Shows the player's inventory to the console. When <paramref name="equip"/>
    /// is <c>true</c>, the method will prompt the user to select an item and
    /// attempt to equip it.
    /// </summary>
    /// <param name="equip">If <c>true</c>, allow selecting and equipping an item; otherwise only display inventory.</param>
    public override string Info()
    {
        string txt = 
        $"\nInventory Slots: [{InventorySize}]\n___________________";

        return base.Info() + txt;
    }
    public override void CheckInventory(bool equip = false)
    {
        base.SortInventory();
        if (equip)
        {
            bool running = true;
            int selectedItemIndex = 0;
            
            while (running)
            {
                List<string> invOptions = new();
                foreach (Item? item in Inventory)
                {
                    if (item != null)
                    { invOptions.Add(item.Name);}
                }
                if (invOptions.Count == 0) { return;}
                string[] invOptionsArr = invOptions.ToArray();

                Dictionary<string, Item> invDict = new();
                for (int i = 0; i < invOptionsArr.Length; ++i)
                {
                    if (Inventory[i] != null)
                    {
                        invDict.Add(Inventory[i]!.Name, Inventory[i]!);
                    }
                }
                Console.Clear();
                Utility.GenerateMenu("INVENTORY:");
                Utility.GenerateMenuActions(selectedItemIndex, invOptionsArr);
                Utility.PrintColor("[ESC] - Back to previous menu", ConsoleColor.DarkGray);
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedItemIndex--;
                        if (selectedItemIndex < 0)
                            selectedItemIndex = invOptionsArr.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedItemIndex++;
                        if (selectedItemIndex > invOptionsArr.Length-1)
                            selectedItemIndex = 0;
                        break;
                    case ConsoleKey.Enter:
                        Debug.Assert(Inventory[selectedItemIndex] != null); 
                        try { Console.Clear(); } catch { }
                        Console.WriteLine(this.Inventory[selectedItemIndex]!.Info());

                        bool subRunning = true;
                        int selectedIndex = 0;
                        string[] yesNo= ["Yes", "No"];
                        while (subRunning)
                        {
                            Console.Clear();
                            Utility.GenerateMenu("Equip " + this.Inventory[selectedItemIndex]!.Name + "?");
                            Utility.PrintColor(Inventory[selectedItemIndex]!.Info() + "\n",ConsoleColor.DarkGreen);
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
                                    if (selectedIndex >= yesNo.Length)
                                        selectedIndex = 0;
                                    break;
                                case ConsoleKey.Enter:
                                    if (yesNo[selectedIndex] == "Yes")
                                    {
                                        this.EquipItem(this.Inventory[selectedItemIndex]!);
                                        subRunning = false;
                                    }
                                    else if (yesNo[selectedIndex] == "No") { subRunning = false; }
                                    break;
                            }
                        }
                        running = false;
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
        else
        {
            base.CheckInventory();
        }
    }
    public void CheckEquipped(bool unequip = false)
    {
        /// <summary>
        /// Displays the currently equipped items and allows the user to
        /// select a slot to view details and optionally unequip the item.
        /// </summary>

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
        if (unequip)
        {
            bool running = true;
            int selectedItemIndex = 0;

            while (running)
            {
                List<string> invOptions = new();
                foreach (Item? item in Equipped)
                {
                    if (item != null)
                    { invOptions.Add(item.Name); }
                }
                if (invOptions.Count == 0) { return; }
                string[] invOptionsArr = invOptions.ToArray();

                Dictionary<string, Item> invDict = new();
                for (int i = 0; i < invOptionsArr.Length; ++i)
                {
                    if (Equipped[i] != null)
                    {
                        invDict.Add(Equipped[i]!.Name, Equipped[i]!);
                    }
                }
                Console.Clear();
                Utility.GenerateMenu("EQUIPPED:");
                Utility.GenerateMenuActions(selectedItemIndex, invOptionsArr);
                Utility.PrintColor("[ESC] - Back to previous menu", ConsoleColor.DarkGray);
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedItemIndex--;
                        if (selectedItemIndex < 0)
                            selectedItemIndex = invOptionsArr.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedItemIndex++;
                        if (selectedItemIndex >= invOptionsArr.Length)
                            selectedItemIndex = 0;
                        break;
                    case ConsoleKey.Enter:
                        Debug.Assert(Equipped[selectedItemIndex] != null);
                        try { Console.Clear(); } catch { }
                        Console.WriteLine(this.Equipped[selectedItemIndex]!.Info());

                        bool subRunning = true;
                        int selectedIndex = 0;
                        string[] yesNo = ["Yes", "No"];
                        while (subRunning)
                        {
                            Console.Clear();
                            Utility.GenerateMenu("Unequip " + this.Equipped[selectedItemIndex]!.Name + "?");
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
                                    if (selectedIndex >= yesNo.Length)
                                        selectedIndex = 0;
                                    break;
                                case ConsoleKey.Enter:
                                    if (yesNo[selectedIndex] == "Yes")
                                    {
                                        this.UnEquipItem(this.Equipped[selectedItemIndex]!);
                                        subRunning = false;
                                    }
                                    else if (yesNo[selectedIndex] == "No") { subRunning = false; }
                                    break;
                            }
                        }
                        running = false;
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
        else
        {
            Console.WriteLine(Display());
            Utility.PromptKey(clear: false);
        }
    }
    public override void Loot(Entity victim)
    {
        List<Item> tempItems = new();
        foreach (Item? item in victim.Inventory) { tempItems.Add(item!); }
        int selectedItemIndex = 0;
        bool subRunning = true;
        while (victim.InventoryRange() > 0 && subRunning)
        {
            Console.Clear();
            List<string> itemList = new();
            foreach (Item? item in tempItems)
            { if (item != null) { itemList.Add("\n" + item!.Name); } }

            string[] itemArray = itemList.ToArray();
            Utility.GenerateMenu(victim.Name + "s Inventory:");
            Utility.GenerateMenuActions(selectedItemIndex, itemArray);
            Utility.PrintColor("[ESC] - Stop looting", ConsoleColor.DarkGray);
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.UpArrow:
                    selectedItemIndex--;
                    if (selectedItemIndex < 0)
                        selectedItemIndex = itemArray.Length - 1;
                    break;
                case ConsoleKey.DownArrow:
                    selectedItemIndex++;
                    if (selectedItemIndex >= itemArray.Length)
                        selectedItemIndex = 0;
                    break;
                case ConsoleKey.Enter:
                    this.AddItem(tempItems[selectedItemIndex]);
                    victim.DiscardItem(tempItems[selectedItemIndex]);
                    tempItems.Remove(tempItems[selectedItemIndex]);
                    selectedItemIndex = 0;
                    break;
                case ConsoleKey.Escape:
                    subRunning = false;
                    return;
            }
        }
        return;
    }
    /// <summary>
    /// Unequips <paramref name="item"/> if it is currently equipped.
    /// </summary>
    /// <param name="item">The item to unequip. If the item is a <see cref="Weapon"/>, the player's damage will be adjusted.</param>
    /// <remarks>
    /// The method will attempt to move the unequipped item back into the inventory
    /// and update related stats (for example, decrease damage when a weapon is removed).
    /// </remarks>
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
        if(item is Weapon)
        {
            UnEquip(item);
            this.Dmg -= item.Value;
        }
    }
    /// <summary>
    /// Equips the provided <paramref name="item"/> into the appropriate slot.
    /// </summary>
    /// <param name="item">Item to equip. If the item is a <see cref="Consumable"/>, it is consumed and its effect applied immediately.</param>
    /// <remarks>
    /// For <see cref="Weapon"/> items this method updates the player's damage.
    /// When equipping, any currently-equipped item in the target slot will be
    /// unequipped first.
    /// </remarks>
    public void EquipItem(Item item)
    {
        void Equip(Item item)
        {
            for (int i = 0; i < Equipped.Length; i++)
            {
                if (item is not Consumable)
                {
                    if (Equipped[i] != null)
                    {
                        UnEquipItem(Equipped[i]!);
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
        switch (item)
        {
            case Weapon:
                Equip(item);
                this.Dmg += item.Value;
                Utility.Success(item.Name + " equipped!");
                break;
            case Consumable:
                double restoredHP = item.Value;
                double leftOverHP = 0;
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
    /// <summary>
    /// Called when it is the player's turn in combat. This override currently
    /// prints a turn announcement and defers to <see cref="Actor.TakeTurn"/> for shared behavior.
    /// </summary>
    /// <param name="opponent">The current opponent <see cref="Entity"/> in the encounter.</param>
    public override void TakeTurn(Entity opponent)
    {
        base.TakeTurn(opponent);
        Console.WriteLine(this.Name + "'s turn!");

    }
}
namespace Game;

using System.Data.Entity.Core.Mapping;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using Game;
using Microsoft.Data.Sqlite;

static class DataManager
{
    static string DbDir = Path.Combine(AppContext.BaseDirectory, "data");
    static string DbPath = Path.Combine(DbDir, "data.db");
    static string connString = $"Data Source={DbPath};Mode=ReadWriteCreate";
    static DataManager()
    {
        Directory.CreateDirectory(DbDir);
    }
    public static bool CheckSaveSlot(int newSlot)
    {
        int dbSlot = 0;
        using (var conn = new SqliteConnection(connString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText =
            """
            SELECT saveSlotNr FROM SaveSlots WHERE saveSlotNr = @newSlot;
            """;
            cmd.Parameters.AddWithValue("@newSlot", newSlot);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    dbSlot = reader.GetInt32(0);
                }
            }


            if (dbSlot == 0)
            {
                cmd = conn.CreateCommand();
                cmd.CommandText =
                """
                INSERT INTO SaveSlots (saveSlotNr, created_at)
                VALUES (@newSlot, CURRENT_TIMESTAMP);
                """;
                cmd.Parameters.AddWithValue("@newSlot", newSlot);

                cmd.ExecuteNonQuery();
                Utility.Success("Creating new Save Slot, please wait...");
                Thread.Sleep(1000);
                return false;
            }
            else
            {
                Utility.Success("Slot exists, loading...");
                Thread.Sleep(1000);
                return true;
            }
        }
        
    }
    public static List<Player> LoadGlobalClasses()
    {
        List<Player> classes = new List<Player>();
        using (var conn = new SqliteConnection(connString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText =
            """
            Select * FROM PlayerClasses;
            """;
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read()) 
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    bool alive = reader.GetBoolean(2);
                    double hp = reader.GetDouble(3);
                    double maxHp = reader.GetDouble(4);
                    int mp = reader.GetInt32(5);
                    double dmg = reader.GetDouble(6);
                    int xp = reader.GetInt32(7);
                    int xp_drop = reader.GetInt32(8);
                    int lvl = reader.GetInt32(9);
                    int inventorySize = reader.GetInt32(10);
                    Player newClass = new Player(id, name, maxHp, mp, dmg, xp, lvl, inventorySize);
                    classes.Add(newClass);
                }
            }
            return classes;
        }
    }
    public static List<Item> LoadGlobalItems()
    {
        List<Item> items = new List<Item>();
        using (var conn = new SqliteConnection(connString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText =
            """
            SELECT * FROM Items;
            """;
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string itemType = reader.GetString(1);
                    string name = reader.GetString(2);
                    double effectAmount = reader.GetDouble(3);
                    if (itemType == "weapon")
                    {
                        string weaponTypeStr = reader.GetString(4);
                        double critChance = reader.GetDouble(5);
                        double critDamage = reader.GetDouble(6);
                        if (Enum.TryParse<WeaponType>(weaponTypeStr, out var weaponType))
                        {
                            var weapon = new Weapon(name, effectAmount, weaponType) { Id = id };
                            weapon.CritChance = critChance;
                            weapon.CritDamage = critDamage;
                            items.Add(weapon);
                        }
                    }
                    else if (itemType == "consumable")
                    {
                        var consumable = new Consumable(name, effectAmount) { Id = id };
                        items.Add(consumable);
                    }
                }
            }
        }
        return items;
    }
    public static void SavePlayer(int saveslot, Player player)
    {
        using (var conn = new SqliteConnection(connString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText =
            """
            INSERT OR IGNORE INTO Entities 
            (save_slot, entity_type, name, alive, hp, max_hp,dmg,xp,xp_drop,lvl,inventory_size)
            VALUES
            (@save_slot, @entity_type, @name, @alive, @hp, @max_hp,@dmg,@xp,@xp_drop,@lvl,@inventory_size)
            """;

            cmd.Parameters.AddWithValue("@save_slot", saveslot);
            cmd.Parameters.AddWithValue("@entity_type", "player");
            cmd.Parameters.AddWithValue("@name", player.Name);
            cmd.Parameters.AddWithValue("@alive", player.Alive);
            cmd.Parameters.AddWithValue("@hp", player.Hp);
            cmd.Parameters.AddWithValue("@max_hp", player.MaxHP);
            cmd.Parameters.AddWithValue("@dmg", player.Dmg);
            cmd.Parameters.AddWithValue("@xp", player.Xp);
            cmd.Parameters.AddWithValue("@xp_drop", player.XpDrop);
            cmd.Parameters.AddWithValue("@lvl", player.Lvl);
            cmd.Parameters.AddWithValue("@inventory_size", player.InventorySize);
            cmd.ExecuteNonQuery();
        }
    }
    public static void SavePlayerInventory(Player player)
    {
        using (var conn = new SqliteConnection(connString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText =
            """
            INSERT OR IGNORE INTO Inventories 
            (entity_id, item_id, quantity)
            VALUES
            (@entity_id, @item_id, @quantity)
            """;
            foreach (Item item in player.Inventory)
            {
                Console.WriteLine("Saving started");
                Console.WriteLine("Item to save: ", item.Id);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@entity_id", player.Id);
                Console.WriteLine("Saving ", player.Id);
                cmd.Parameters.AddWithValue("@item_id", item.Id);
                Console.WriteLine("Saving ", item.Id);
                cmd.Parameters.AddWithValue("@quantity", item.quantity);
                Console.WriteLine("Saving ended");
                cmd.ExecuteNonQuery();
            }
        }
    }
    public static void SaveItem(Item item)
    {
        int itemId = -1;
        using (var conn = new SqliteConnection(connString))
        {
            if (item is Weapon)
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText =
                """
                INSERT OR IGNORE INTO Items (item_type, name, effect_amount, weapon_type, crit_chance, crit_damage)
                VALUES (@item_type, @name, @effect_amount, @weapon_type, @crit_chance, @crit_damage);
                SELECT last_insert_rowid();
                """;
                cmd.Parameters.AddWithValue("@name", item.Name);
                cmd.Parameters.AddWithValue("@effect_amount", item.EffectAmount);
                cmd.Parameters.AddWithValue("@item_type", "weapon");
                cmd.Parameters.AddWithValue("@weapon_type", ((Weapon)item).Type.ToString());
                cmd.Parameters.AddWithValue("@crit_chance", ((Weapon)item).CritChance);
                cmd.Parameters.AddWithValue("@crit_damage", ((Weapon)item).CritDamage);
                var rows = cmd.ExecuteNonQuery();

                if (rows == 1)
                {
                    cmd.CommandText = "SELECT last_insert_rowid();";
                    item.Id = Convert.ToInt32(cmd.ExecuteScalar());
                }
                //else
                //{
                //    // insert failed or was ignored
                //    item.Id = -1; // or handle explicitly
                //}
                conn.Close();
            }
            else if (item is Consumable)
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText =
                """
                INSERT OR IGNORE INTO Items (item_type, name, effect_amount)
                VALUES (@item_type, @name, @effect_amount);
                SELECT last_insert_rowid();
                """;
                cmd.Parameters.AddWithValue("@name", item.Name);
                cmd.Parameters.AddWithValue("@effect_amount", item.EffectAmount);
                cmd.Parameters.AddWithValue("@item_type", "consumable");
                var rows = cmd.ExecuteNonQuery();

                if (rows == 1)
                {
                    cmd.CommandText = "SELECT last_insert_rowid();";
                    item.Id = Convert.ToInt32(cmd.ExecuteScalar());
                }
                conn.Close();
            }
            item.Id = itemId;
        }
    }
    public static Player LoadPlayer(int saveslot, Player player)
    {
        using (var conn = new SqliteConnection(connString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText =
            """
            SELECT name, alive, hp, max_hp, dmg, xp, xp_drop, lvl, inventory_size
            FROM Entities
            WHERE save_slot = @save_slot AND entity_type = 'player'
            """;
            cmd.Parameters.AddWithValue("@save_slot", saveslot);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    player.Name = reader.GetString(0);
                    player.Alive = reader.GetBoolean(1);
                    player.Hp = reader.GetInt32(2);
                    player.MaxHP = reader.GetInt32(3);
                    player.Dmg = reader.GetInt32(4);
                    player.Xp = reader.GetInt32(5);
                    player.XpDrop = reader.GetInt32(6);
                    player.Lvl = reader.GetInt32(7);
                    player.InventorySize = reader.GetInt32(8);
                }
            }
        }
        return player;
    }
}
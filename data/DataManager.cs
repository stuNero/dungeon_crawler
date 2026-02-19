namespace Game;

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
    public static void MakeSaveSlot()
    {
        using (var conn = new SqliteConnection(connString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText =
            """
            INSERT INTO Save_Slots (created_at)
            VALUES (CURRENT_TIMESTAMP);
            """;
            cmd.ExecuteNonQuery();
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
                    int effectAmount = reader.GetInt32(3);
                    if (itemType == "weapon")
                    {
                        string weaponTypeStr = reader.GetString(4);
                        int critChance = reader.GetInt32(5);
                        int critDamage = reader.GetInt32(6);
                        if (Enum.TryParse<WeaponType>(weaponTypeStr, out var weaponType))
                        {
                            var weapon = new Weapon(name, effectAmount, weaponType) { Id = id };
                            weapon.CritChance = critChance;
                            weapon.CritDamage = critDamage;
                            Utility.PromptKey(weapon.Info());
                            items.Add(weapon);
                        }
                    }
                    else if (itemType == "consumable")
                    {
                        var consumable = new Consumable(name, effectAmount) { Id = id };
                        Utility.PromptKey(consumable.Info());
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
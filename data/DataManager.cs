namespace Game;

using System.CodeDom.Compiler;
using System.Data.Entity.Core.Mapping;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using Game;
using Microsoft.Data.Sqlite;

static class DataManager
{
    static readonly string DbDir = Path.Combine(AppContext.BaseDirectory, "data");
    static readonly string DbPath = Path.Combine(DbDir, "data.db");
    static readonly string connString = $"Data Source={DbPath};Mode=ReadWriteCreate";
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
    public static void Save_SaveEntities(int saveSlot, List<Entity> entities)
    {
        using (var conn = new SqliteConnection(connString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            foreach (Entity entity in entities)
            {
                cmd.CommandText =
                """
                SELECT id FROM Entities WHERE id = @id
                """;
                cmd.Parameters.AddWithValue("@id", entity.id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        if (entity is Player player)
                        {
                            SavePlayer(exists: false, player);
                        }
                        else if (entity is Enemy enemy)
                        {
                            SaveEnemy(exists: false, enemy);
                        }
                    }
                    else
                    {
                        if (entity is Player player)
                        {
                            SavePlayer(exists: true, player);
                        }
                        else if (entity is Enemy enemy)
                        {
                            SaveEnemy(exists: true, enemy);
                        }
                    }
                }
            }
        }
    }
    public static List<Entity> LoadSaveEntities(int saveSlot)
    {
        List<Entity> saveEntities = [];
        using (var conn = new SqliteConnection(connString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText =
            """
            SELECT * FROM Entities WHERE id = 
            SELECT entity FROM EntitiesPerSave WHERE saveSlot = @saveSlot;  
            """;
            cmd.Parameters.AddWithValue("@saveSlot", saveSlot);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string entity_type = reader.GetString(1);
                    string name = reader.GetString(2);
                    bool alive = reader.GetBoolean(3);
                    double hp = reader.GetDouble(4);
                    double maxHp = reader.GetDouble(5);
                    int mp = reader.GetInt32(6);
                    double dmg = reader.GetDouble(7);
                    int xp = reader.GetInt32(8);
                    int lvl = reader.GetInt32(10);
                    int inventorySize = reader.GetInt32(12);
                    switch (entity_type)
                    {
                        case "player":
                            Player player = new(name, maxHp, mp, dmg, xp, lvl, inventorySize)
                            {
                                id = id,
                                Hp = hp,
                                Alive = alive
                            };
                            saveEntities.Add(player);
                            break;
                        case "enemy":
                            string enemyType = reader.GetString(11);
                            Enemy enemy = new(name, maxHp, mp, dmg, xp, lvl, inventorySize, enemyType)
                            {
                                id = id,
                                Hp = hp,
                                Alive = alive
                            };
                            saveEntities.Add(enemy);
                            break;
                    }
                }
            }
        }
        return saveEntities;
    }
    public static List<Player> LoadGlobalClasses()
    {
        List<Player> classes = [];
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
                    Player newClass = new(name, maxHp, mp, dmg, xp, lvl, inventorySize);
                    classes.Add(newClass);
                }
            }
            return classes;
        }
    }
    public static List<Item> LoadGlobalItems()
    {
        List<Item> items = [];
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
                            var weapon = new Weapon(name, effectAmount, weaponType)
                            {
                                Id = id,
                                CritChance = critChance,
                                CritDamage = critDamage
                            };
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
    public static void SavePlayer(bool exists, Player player)
    {
        using (var conn = new SqliteConnection(connString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            if (exists)
            {
                cmd.CommandText =
                """
                UPDATE Entities
                SET
                entity_type = @entity_type,
                name = @name,
                alive = @alive,
                hp = @hp,
                max_hp = @max_hp,
                dmg = @dmg,
                xp = @xp,
                lvl = @lvl,
                inventory_size = @inventory_size
                WHERE id = @id;
                """;
            }
            else
            {
                cmd.CommandText =
                """
                INSERT INTO Entities 
                (entity_type, name, alive, hp, max_hp, dmg, xp, lvl, inventory_size)
                VALUES
                (@entity_type, @name, @alive, @hp, @max_hp, @dmg, @xp, @lvl, @inventory_size)
                """;
            }
            cmd.Parameters.AddWithValue("@id", player.id);
            cmd.Parameters.AddWithValue("@entity_type", "player");
            cmd.Parameters.AddWithValue("@name", player.Name);
            cmd.Parameters.AddWithValue("@alive", player.Alive);
            cmd.Parameters.AddWithValue("@hp", player.Hp);
            cmd.Parameters.AddWithValue("@max_hp", player.MaxHP);
            cmd.Parameters.AddWithValue("@dmg", player.Dmg);
            cmd.Parameters.AddWithValue("@xp", player.Xp);
            cmd.Parameters.AddWithValue("@lvl", player.Lvl);
            cmd.Parameters.AddWithValue("@inventory_size", player.InventorySize);
            cmd.ExecuteNonQuery();
        }
        SaveEntityInventory(player);
    }
    public static void SaveEnemy(bool exists, Enemy enemy)
    {
        using (var conn = new SqliteConnection(connString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            if (exists)
            {
                cmd.CommandText =
                """
                UPDATE Entities
                SET
                entity_type = @entity_type,
                name = @name,
                alive = @alive,
                hp = @hp,
                max_hp = @max_hp,
                dmg = @dmg,
                xp = @xp,
                lvl = @lvl,
                enemy_type = @enemy_type,
                inventory_size = @inventory_size
                WHERE id = @id;
                """;
            }
            else
            {
                cmd.CommandText =
                """
                INSERT INTO Entities 
                (entity_type, name, alive, hp, max_hp,dmg,xp,lvl,enemy_type,inventory_size)
                VALUES
                (@entity_type, @name, @alive, @hp, @max_hp, @dmg, @xp, @lvl, @enemy_type, @inventory_size)
                """;
            }
            cmd.Parameters.AddWithValue("@id", enemy.id);
            cmd.Parameters.AddWithValue("@entity_type", "enemy");
            cmd.Parameters.AddWithValue("@name", enemy.Name);
            cmd.Parameters.AddWithValue("@alive", enemy.Alive);
            cmd.Parameters.AddWithValue("@hp", enemy.Hp);
            cmd.Parameters.AddWithValue("@max_hp", enemy.MaxHP);
            cmd.Parameters.AddWithValue("@dmg", enemy.Dmg);
            cmd.Parameters.AddWithValue("@xp", enemy.Xp);
            cmd.Parameters.AddWithValue("@lvl", enemy.Lvl);
            cmd.Parameters.AddWithValue("@enemy_type", enemy.Type);
            cmd.Parameters.AddWithValue("@inventory_size", enemy.InventorySize);
            cmd.ExecuteNonQuery();
        }
        SaveEntityInventory(enemy);
    }
    public static void SaveEntityInventory(Entity entity)
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
            foreach (Item item in entity.Inventory)
            {
                if (item == null) { continue; }
                Console.WriteLine(entity.id);
                Console.WriteLine(item.Id);
                Console.ReadKey();
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@entity_id", entity.id);
                cmd.Parameters.AddWithValue("@item_id", item.Id);
                cmd.Parameters.AddWithValue("@quantity", item.quantity);
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
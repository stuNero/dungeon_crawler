namespace Game;

using System.Runtime.InteropServices;
using Game;
using Microsoft.Data.Sqlite;


static class DBManager
{
    static string connString = @"Data Source=C:\Users\maxve\repos\dungeon_crawler\data\schema.db";

    public static void SavePlayer(int saveslot, Player player)
    {
        using (var conn = new SqliteConnection(connString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = 
            """
            INSERT INTO Entities 
            (save_slot, entity_type, name, alive, hp, max_hp,dmg,xp,xp_drop,lvl,inventory_size)
            VALUES
            (@save_slot, @entity_type, @name, @alive, @hp, @max_hp,@dmg,@xp,@xp_drop,@lvl,@inventory_size)
            """;

            cmd.Parameters.AddWithValue("@save_slot", saveslot);
            cmd.Parameters.AddWithValue("@entity_type", "player");
            cmd.Parameters.AddWithValue("@name", player.Name);
            cmd.Parameters.AddWithValue("@alive", player.Alive);
            cmd.Parameters.AddWithValue("@hp",player.Hp);
            cmd.Parameters.AddWithValue("@max_hp",player.MaxHP);
            cmd.Parameters.AddWithValue("@dmg",player.Dmg);
            cmd.Parameters.AddWithValue("@xp",player.Xp);
            cmd.Parameters.AddWithValue("@xp_drop",player.XpDrop);
            cmd.Parameters.AddWithValue("@lvl",player.Lvl);
            cmd.Parameters.AddWithValue("@inventory_size",player.InventorySize);
            cmd.ExecuteNonQuery();
        }
    }
}
namespace Game;

class Enemy(int id, string name, double maxHP, int mp, double dmg, int xp, int lvl, int inventorySize, string type) : Actor(id, name, maxHP, mp, dmg, xp, lvl, inventorySize)
{
    public string Type = type;
}
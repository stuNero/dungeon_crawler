namespace Game;

class Enemy(string name, double maxHP, int mp, double dmg, int xp, int lvl, int inventorySize, string type) : Actor(name, maxHP, mp, dmg, xp, lvl, inventorySize)
{
    public int id;
    public string Type = type;
}
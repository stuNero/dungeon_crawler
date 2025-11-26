namespace Game;

class Enemy : Actor
{
    public string Type;
    public Enemy(string name, double maxHP, int mp, double dmg, int xp, int lvl, int inventorySize, string type)
            : base(name, maxHP, mp, dmg, xp, lvl, inventorySize)
    {
        Type = type;
    }
}
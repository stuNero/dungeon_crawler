namespace Game;

class Enemy : Actor
{
    public string Type;
    public Enemy(int id, string name, double maxHP, int mp, double dmg, int xp, int lvl, int inventorySize, string type)
            : base(id, name, maxHP, mp, dmg, xp, lvl, inventorySize)
    {
        Type = type;
    }
}
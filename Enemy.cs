namespace Game;

class Enemy : Actor
{
    public string Type;
    public Enemy(string name, int maxHP, int mp, int dmg, int xp, int lvl, int inventorySize, string type)
            : base(name, maxHP, mp, dmg, xp, lvl, inventorySize)
    {
        Type = type;
    }
}
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Game;

abstract class Actor : Entity
{
    public int Mp;
    public int Dmg;
    public int Xp;
    public int XpDrop;
    public int Lvl;

    public Actor(string name, int maxHP, int mp, int dmg, int xp, int lvl, int inventorySize)
            : base(name, maxHP, inventorySize)
    {
        Mp = mp;
        Dmg = dmg;
        Xp = xp;
        XpDrop = Xp / 100;
        Lvl = lvl;
    }
    public string Info()
    {
        string txt = "___________________\n";

        txt += $"Name: [{Name}]\n"
             + $"LVL:  [{Lvl}]\n"
             + $"HP:   [{Hp}]\n"
             + $"MP:   [{Mp}]\n"
             + $"DMG:  [{Dmg}]\n";
        txt += "___________________";
        return txt;
    }
}
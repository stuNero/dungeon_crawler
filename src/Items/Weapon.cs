namespace Game;

class Weapon : Item
{
    public double CritChance = 0.05;
    public double CritDamage = 1.5;
    public WeaponType Type;
    public Weapon(string name, double damage, WeaponType type)
    : base(name, damage)
    {
        Type = type;
        switch (type)
        {
            case WeaponType.Axe:
                this.Value += 3;
                CritChance += 0.1;
                CritDamage += 0.3;
                break;
            case WeaponType.Sword:
                this.Value += 2;
                CritChance += 0.05;
                CritDamage += 0.1;
                break;
            case WeaponType.Dagger:
                this.Value += 0;
                CritChance += 0.2;
                CritDamage += 1.5;
                break;
            case WeaponType.Mace:
                this.Value += 3;
                CritChance += 0;
                CritDamage += 1;
                break;
        }
        CritChance = Math.Round(CritChance, 2);
        CritDamage = Math.Round(CritDamage, 2);
    }
    public override string Info()
    {
        return base.Info() + $"\nCrit Chance: [{CritChance*100} %] | Crit Damage: [x{CritDamage}]\nType:[{Type}]";
    }
    public double CritCheck(double damage)
    {
        Random rnd = new Random();
        double chance = rnd.NextDouble();
        if (chance <= CritChance)
        {
            damage = Crit();
            Utility.PrintColor(Name + " crit for " + (damage - this.Value) + " damage!", ConsoleColor.DarkRed);
        }
        return damage;
    }
    public double Crit()
    {
        double damage = this.Value;
        damage *= CritDamage;
        return damage;
    }
}

enum WeaponType
{
    Axe,
    Sword,
    Dagger,
    Mace,
}
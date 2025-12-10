namespace Game;

class Consumable : Item
{
    public Consumable(string name, double EffectAmount):base(name, EffectAmount)
    {}
    public override string Info()
    {
        return  $"Name:   [{Name}]\n" +
                $"Points: [{EffectAmount}]";
    }
}
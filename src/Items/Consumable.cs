namespace Game;

class Consumable(string name, double EffectAmount) : Item(name, EffectAmount)
{
    public override string Info()
    {
        return  $"Name:   [{Name}]\n" +
                $"Points: [{EffectAmount}]";
    }
}
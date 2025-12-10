namespace Game;

class Item
{
    public string Name;
    public double EffectAmount;

    public Item(string name, double effectAmount)
    {
        Name = name;
        EffectAmount = effectAmount;

    }
    public virtual string Info()
    {
        return  $"Name:   [{Name}]\n" +
                $"Damage: [{EffectAmount}]";
    }
}
namespace Game;

class Item
{
    public int Id { get; set; }
    public string Name;
    public double EffectAmount;
    public int quantity;
    public Item(string name, double effectAmount)
    {
        Name = name;
        EffectAmount = effectAmount;
        quantity = 1;
    }
    public virtual string Info()
    {
        return  $"Name:   [{Name}]\n" +
                $"Damage: [{EffectAmount}]";
    }
}
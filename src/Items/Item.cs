namespace Game;

class Item(string name, double effectAmount)
{
    public int Id { get; set; }
    public string Name = name;
    public double EffectAmount = effectAmount;
    public int quantity = 1;

    public virtual string Info()
    {
        return  $"Name:   [{Name}]\n" +
                $"Damage: [{EffectAmount}]";
    }
}
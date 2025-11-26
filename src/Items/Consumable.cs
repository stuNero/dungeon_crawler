namespace Game;

class Consumable : Item
{
    public Consumable(string name, double value):base(name, value)
    {}
    public override string Info()
    {
        return  $"Name:   [{Name}]\n" +
                $"Points: [{Value}]";
    }
}
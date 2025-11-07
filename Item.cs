namespace Game;

class Item
{
    public string Name;
    public double Value;

    public Item(string name, double value)
    {
        Name = name;
        Value = value;

    }
    public virtual string Info()
    {
        return  "_______\n" +
                $"Name:   [{Name}]\n" +
                $"Damage: [{Value}]";
    }
}
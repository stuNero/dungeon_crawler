namespace Game;

class Item
{
    public string Name;
    public string Type; 
    public int Dmg;
    public int Value;

    public Item(string name, string type)
    {
        /// <summary>
        /// Choose -type- between "weapon", "consumable"
        /// </summary>
        Name = name;
        Type = type;
    }
    public string Info()
    {
        if (Type == "weapon")
        {
            return  $"Name:     [{Name}]\n"+
                    $"Damage:   [{Dmg}]";
        }
        else
        {
            return  $"Name:     [{Name}]\n"+
                    $"HP:       [{Value}]";
        }
    }
    public void DefineItem(int value)
    {
        switch (Type)
        {
            case "weapon":
                Dmg = value;
                break;
            case "consumable":
                Value = value;
                break;
            default:
                Utility.Error("Something went wrong when defining item..");
                break;
        }
    }
}
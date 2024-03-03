using Godot;

namespace ProjectReaper.Items;

public partial struct ItemRarity 
{
    public int Value;
    public float Weight;
    public string Name;
    public Color Color;
    public ItemRarity(int value, float weight, string name, Color color)
    {
        Value = value;
        Weight = weight;
        Name = name;
        Color = color;
    }

    public static ItemRarity Common => new ItemRarity(100, .6f, "r_common", new Color(1, 1, 1));
    public static ItemRarity Uncommon => new ItemRarity(200, 0.2f, "r_uncommon", new Color(0, 1, 0));
    public static ItemRarity Rare => new ItemRarity(300,0.05f, "r_rare", new Color(0, 0, 1));
}
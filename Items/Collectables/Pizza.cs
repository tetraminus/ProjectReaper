using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Items.Collectables;

public partial class Pizza : AbstractItem
{
    public override string Id => "pizza";
    public override ItemRarity Rarity => ItemRarity.Common;
    
    private const float Speed = 0.10f;
    private const float Damage = 0.03f;
    


    public override void OnInitalPickup()
    {
        Callbacks.Instance.CalculateStat += CalculateStat;
        
    }
    
    public float CalculateStat(float stat, string statname, AbstractCreature creature)
    {
        if (statname == "Speed")
        {
            return stat + stat * Speed * Stacks;
        }
        if (statname == "Damage")
        {
            return stat + stat * Damage * Stacks;
        }

        return stat;
    }

    public override void Cleanup()
    {
        Callbacks.Instance.CalculateStat -= CalculateStat;
    }
}
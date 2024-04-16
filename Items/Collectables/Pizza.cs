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
    
    public void CalculateStat(ref float stat, string statname, AbstractCreature creature)
    {
        if (statname == "Speed" && creature == GetHolder())
        {
            stat += stat * Speed * Stacks;
        }
        
        if (statname == "Damage" && creature == GetHolder())
        {
            stat += stat * Damage * Stacks;
        }

        
    }

    public override void Cleanup()
    {
        Callbacks.Instance.CalculateStat -= CalculateStat;
    }
}
using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Items.Collectables;

public partial class CritGlasses : AbstractItem
{
    
    public override string Id => "critglasses";
    public override ItemRarity Rarity => ItemRarity.Common;
    public const float CritChance = 0.15f;
    
    public override void OnInitalPickup()
    {
        Callbacks.Instance.CalculateStat += CalculateStat;
        
    }

    private float CalculateStat(float stat, string statname, AbstractCreature creature)
    {
        if (statname == "CritChance" && creature == GetHolder())
        {
            return stat + (stat * CritChance * Stacks);
        }
        return stat;
    }
    
    public override void Cleanup()
    {
        Callbacks.Instance.CalculateStat -= CalculateStat;
    }
}
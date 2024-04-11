using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Items.Collectables;

public partial class ConfidenceBooster : AbstractItem
{
    
    public override string Id => "confidence_booster";
    public override ItemRarity Rarity => ItemRarity.Common;
    public const float DamageIncreaseRatio = 0.001f;
    
    public override void OnInitalPickup()
    {
        Callbacks.Instance.CalculateStat += CalculateStat;
        GetHolder().Stats.MakeUncacheable("Damage");
    }

    private float CalculateStat(float stat, string statname, AbstractCreature creature)
    {
        if (statname == "Damage" && creature == GetHolder())
        {
            
            // increase damage by 0.1% per velocity unit per stack
            return stat + DamageIncreaseRatio * creature.Velocity.Length() * Stacks;
            
            
        }
        return stat;
    }
    
    public override void Cleanup()
    {
        Callbacks.Instance.CalculateStat -= CalculateStat;
        GetHolder().Stats.MakeCacheable("Damage");
    }
    
}
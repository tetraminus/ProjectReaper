using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Items.Collectables;

public partial class ConfidenceBooster : AbstractItem
{
    
    public override string Id => "confidence_booster";
    public override ItemRarity Rarity => ItemRarity.Rare;
    public const float DamageIncreaseRatio = 0.0007f;
    
    public override void OnInitalPickup()
    {
        Callbacks.Instance.CalculateStat += CalculateStat;
        GetHolder().Stats.MakeUncacheable("Damage");
    }

    private void CalculateStat(ref float stat, string statname, AbstractCreature creature)
    {
        if (statname == "Damage" && creature == GetHolder())
        {
            
            // increase damage by 0.1% per velocity unit per stack
            stat += creature.Velocity.Length() * DamageIncreaseRatio * Stacks;
            
        }
      
    }
    
    public override void Cleanup()
    {
        Callbacks.Instance.CalculateStat -= CalculateStat;
        GetHolder().Stats.MakeCacheable("Damage");
    }
    
}
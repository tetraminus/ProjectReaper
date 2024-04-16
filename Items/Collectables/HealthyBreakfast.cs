using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Items.Collectables;

public partial class HealthyBreakfast : AbstractItem
{
    public override string Id => "healthy_breakfast";
    public override ItemRarity Rarity => ItemRarity.Common;
    
    private const float HP = 0.20f;
    private const float Damage = 0.05f;
    

    public override void OnInitalPickup()
    {
        Callbacks.Instance.CalculateStat += CalculateStat;
        Callbacks.Instance.EmitSignal(Callbacks.SignalName.RecalculateStats);
        
        GetHolder().Heal(GetHolder().Stats.MaxHealth * HP);
        
    }

    public override void OnStack(int newstacks)
    {
        base.OnStack(newstacks);
        GetHolder().Heal(GetHolder().Stats.MaxHealth * HP * newstacks);
    }

    public void CalculateStat(ref float stat, string statname, AbstractCreature creature)
    {
        if (statname == "MaxHealth" && creature == GetHolder())
        {
            stat += stat * HP * Stacks;
        }
    }

    public override void Cleanup()
    {
        Callbacks.Instance.CalculateStat -= CalculateStat;
    }
    
}
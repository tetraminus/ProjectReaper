using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Powers;

public partial class StatBoostPower : AbstractPower
{
    public override string Id => "speed_boost";
    public override float DefaultDuration => 1;

    public float Multiplier = 0;
    public string StatName = "";
    public string Source = "";
    

    //10% speed increase per stack
    public override void OnApply()
    {
        StartTimer();
        Callbacks.Instance.CalculateStat += OnCalculateStat;
        Callbacks.Instance.EmitSignal(Callbacks.SignalName.RecalculateStats);
    }
    
    private void OnCalculateStat(ref float stat, string statname, AbstractCreature creature)
    {
       
        if (statname == StatName && creature == Creature)
        {
            stat += stat * Multiplier * Stacks;
        }
        
    
    }

    public override void OnStack(int newStacks)
    {
        Callbacks.Instance.EmitSignal(Callbacks.SignalName.RecalculateStats);
        
    }


    public override void OnRemove()
    {
        
        Callbacks.Instance.CalculateStat -= OnCalculateStat;
        Callbacks.Instance.EmitSignal(Callbacks.SignalName.RecalculateStats);
    }
}
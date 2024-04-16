using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Powers;

public partial class Speedboost : AbstractPower
{
    private const float Multiplier = 0.2f;
   
    public override string Id => "speed_boost";
    public override float DefaultDuration => 1;


    //10% speed increase per stack
    public override void OnApply()
    {
        StartTimer();
        Callbacks.Instance.CalculateStat += OnCalculateStat;
        Callbacks.Instance.EmitSignal(Callbacks.SignalName.RecalculateStats);
    }
    
    private void OnCalculateStat(ref float stat, string statname, AbstractCreature creature)
    {
       
        if (statname == "Speed" && creature == Creature)
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
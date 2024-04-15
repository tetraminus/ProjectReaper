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
    
    private float OnCalculateStat(float stat, string statname, AbstractCreature creature)
    {
       
        if (statname == "Speed" && creature == Creature)
        {
            return stat + stat * Multiplier * Stacks;
        }
        return stat;
    
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
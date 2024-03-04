namespace ProjectReaper.Powers;

public partial class Speedboost : AbstractPower
{
    private const float Multiplier = 0.2f;
    public override string Id => "speed_boost";
    public override float Duration => 1;
    float _addedSpeed;
    
    
    

    //10% speed increase per stack
    public override void OnApply()
    {
        StartTimer();
        
        
        _addedSpeed = Creature.Stats.Speed * Multiplier * Stacks;
        Creature.Stats.Speed += _addedSpeed;
    }
    
    public override void OnStack(int newStacks)
    {
        Creature.Stats.Speed -= _addedSpeed;
        _addedSpeed = Creature.Stats.Speed * Multiplier * Stacks;
        Creature.Stats.Speed += _addedSpeed;
    }
    
    
    public override void OnRemove()
    {
        Creature.Stats.Speed -= _addedSpeed;
    }
    
}
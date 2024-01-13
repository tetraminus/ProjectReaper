using Godot;

namespace ProjectReaper.Abilities;

public abstract partial class  AbstractAbility: Node
{
    
    public abstract void Use();
    public abstract float Cooldown { get; set; }
    
    public int Charges { get; set; } = 1;
    
    
}
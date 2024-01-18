using Godot;

namespace ProjectReaper.Enemies;

public abstract partial class AbstractEnemy: CharacterBody2D
{
    public abstract float Speed { get; set; }
    
    public abstract float Damage { get; set; }
    
    public abstract float Health { get; set; }
    
    public abstract float MaxHealth { get; set; }
    
    public abstract void OnHit();
    
    public abstract void OnDeath();
    
 
}
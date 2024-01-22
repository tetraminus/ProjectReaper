using Godot;
using ProjectReaper.Util;

namespace ProjectReaper.Enemies;

public abstract partial class AbstractEnemy : CharacterBody2D {

    public Stats Stats { get; set; } = new Stats();
    
    public Area2D Hurtbox { get; set; }

    public override void _Ready() {
        Stats.Init();
        Hurtbox = FindChild("Hurtbox") as Area2D;
        
    }

	public abstract void OnHit();
	
	public abstract void OnDeath();
	
 
}

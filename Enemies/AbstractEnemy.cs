using Godot;
using ProjectReaper.Util;

namespace ProjectReaper.Enemies;

public abstract partial class AbstractEnemy : CharacterBody2D {

    public Stats Stats { get; set; } = new Stats();
    
    public Area2D Hurtbox { get; set; }
    
    public NavigationAgent2D NavigationAgent { get; set; }

    public override void _Ready() {
        Stats.Init();
        Hurtbox = FindChild("Hurtbox") as Area2D;
        NavigationAgent = FindChild("NavigationAgent") as NavigationAgent2D;
        
    }
    
    
    public override void _PhysicsProcess(double delta) {
        NavigationAgent.TargetPosition = GlobalPosition;
        Velocity = NavigationAgent.Velocity;
        MoveAndSlide();

    }
    

    public abstract void OnHit();
    
    public abstract void OnDeath();
    
 
}
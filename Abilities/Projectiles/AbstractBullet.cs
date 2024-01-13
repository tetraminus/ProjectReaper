using Godot;

namespace ProjectReaper.Abilities.Projectiles;

public abstract partial class AbstractBullet : Area2D
{
    
    private Timer _timer;
    
    public override void _Ready()
    {
        _timer = new Timer();
        AddChild(_timer);
        _timer.Timeout += () => QueueFree();
    }
    public void OnShoot() {}
    
    public void OnHit() {}
    
    public abstract float Speed { get; set; }
    
    public abstract float Damage { get; set; }
    
    public abstract float Duration { get; set; }
    
}
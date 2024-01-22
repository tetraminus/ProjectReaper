using Godot;
using ProjectReaper.Globals;

namespace ProjectReaper.Abilities.Projectiles;

public abstract partial class AbstractDamageArea : Area2D
{
    
    private Timer _timer;
    
    public override void _Ready()
    {
        _timer = new Timer();
        AddChild(_timer);
        _timer.Timeout += () => QueueFree();
    }
    public void OnShoot() {}

    public void OnHit()
    {
        Callbacks.Instance.EmitSignal(Callbacks.SignalName.BulletHit, this);
    }
    
    public abstract float Speed { get; set; }
    
    public abstract float Damage { get; set; }
    
    public abstract float Duration { get; set; }
    
}
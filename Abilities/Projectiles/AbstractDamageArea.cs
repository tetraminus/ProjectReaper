using Godot;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Util;

namespace ProjectReaper.Abilities.Projectiles;

public abstract partial class AbstractDamageArea : Area2D
{
    public bool IsEnemy = false;

    protected Timer Timer;

    public AbstractCreature Source { get; set; }
    
    public AbstractCreature.Teams Team { get; set; } = AbstractCreature.Teams.Enemy;

    public abstract float Speed { get; set; }

    public abstract float Damage { get; set; }

    public abstract float Duration { get; set; }
    public bool DestroyOnHit { get; set; } = true;

    public override void _Ready()
    {
        Monitoring = true;

        AreaEntered += OnAreaEntered;
        AreaExited += OnAreaExited;
        
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
        
    }
    
    public virtual void OnAreaEntered(Area2D area)
    {
        // enemy check
        if (area is not HurtBox hurtBox) return;
        var creature = hurtBox.GetParentCreature();
        if (creature == null) return;
        if (creature.Team == Team) return;
        creature.Damage(new DamageReport(Damage, Source, creature, Source.Stats, creature.Stats));
        if (DestroyOnHit) QueueFree();
    }
    
    public virtual void OnAreaExited(Area2D area)
    {
    }
    
    public virtual void OnBodyEntered(Node body)
    {
        // terrain check
        if (body is not TileMap) return;
        // check if collison layer is terrain
         QueueFree();
        
    }
    
    public virtual void OnBodyExited(Node body)
    {
    }

    public void OnShoot()
    {
    }


    public void OnHit()
    {
        Callbacks.Instance.BulletHitEvent?.Invoke(this);
    }
    
    
}
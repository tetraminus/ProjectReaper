using Godot;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Util;

namespace ProjectReaper.Abilities.Projectiles;

public abstract partial class AbstractDamageArea : Area2D
{
    

    protected Timer Timer;

    public AbstractCreature Source { get; set; }
    
    public AbstractCreature.Teams Team { get; set; } = AbstractCreature.Teams.Enemy;

    public abstract float Speed { get; set; }

    public abstract float Damage { get; set; }

    public abstract float Duration { get; set; }
    public float Range { get; set; } = -1f;
    public bool DestroyOnHit { get; set; } = true;
    
    protected Vector2 startPosition;

    public override void _Ready()
    {
        Monitoring = true;
        
        

        AreaEntered += OnAreaEntered;
        AreaExited += OnAreaExited;
        
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
        
    }
    
    
    /// <summary>
    /// Initialize the projectile with required parameters
    /// </summary>
    /// <param name="source">source of the projectile</param>
    /// <param name="team">team of the source</param>
    /// <param name="position">global position</param>
    /// <param name="rotation"> rotation in radians</param>
    public void Init(AbstractCreature source, AbstractCreature.Teams team, Vector2 position, float rotation)
    {
        GlobalPosition = position;
        startPosition = position;
        GlobalRotation = rotation;
        Source = source;
        Team = team;
    }
    
    
    public virtual void OnAreaEntered(Area2D area)
    {
        // enemy check
        if (area is not HurtBox hurtBox) return;
        var blocker = hurtBox.GetParentBlocker();
        if (blocker == null) return;
        if (!blocker.CanBlockProjectile(this)) return;
        if (blocker.Team == Team) return;
        blocker.OnProjectileBlocked(this);
        
        
         if (blocker is AbstractCreature creature)
        {
           
            creature.Damage(new DamageReport(Damage, Source, creature, Source.Stats, creature.Stats));
        }
        if (DestroyOnHit) QueueFree();
    }
    
    public virtual void OnAreaExited(Area2D area)
    {
    }

    public virtual void OnBodyEntered(Node body)
    {
        if (body is TileMap tileMap)
        {
            QueueFree();
            return;
        }
        
        
        if (body is not PhysicsBody2D physBody ) return;
        // objects check
        if (!physBody.GetCollisionLayerValue(1)) return;
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

    public override void _Process(double delta) {
        base._Process(delta);
        if (Range > 0 && startPosition.DistanceTo(GlobalPosition) > Range)
        {
            QueueFree();
        }
    }
}
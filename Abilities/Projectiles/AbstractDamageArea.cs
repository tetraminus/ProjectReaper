using System;
using System.Collections.Generic;
using Godot;
using Mono.Collections.Generic;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Objects;
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
    public virtual float Knockback { get; set; } = 0f;
    public bool DestroyOnHit { get; set; } = true;
    public bool DestroyOnWall { get; set; } = true;
    public float ProcCoef { get; set; } = 1f;
    public List<IProjectileBlocker> ExclusionList = new();

    protected Vector2 startPosition;

    [Export] public bool overrideLayer = false;
   
    Vector2 _lastPosition;

    public Dictionary<string, Dictionary<string, Variant>> Flags = new();
    

    public override void _Ready()
    {
        Monitoring = true;
        
        

        AreaEntered += OnAreaEntered;
        AreaExited += OnAreaExited;
        
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
        if (!overrideLayer)
        {
            CollisionLayer = 0;
            CollisionMask = 0;
            SetCollisionLayerValue(2, true);
            SetCollisionMaskValue(2, true);
            SetCollisionMaskValue(1, true);
        }
        
        _lastPosition = GlobalPosition;
        
        Callbacks.Instance.EmitSignal(Callbacks.SignalName.BulletCreated, this);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        // continous collision check
        if (Monitoring && GlobalPosition.DistanceTo(_lastPosition) > 2f)
        {
            
            var spaceState = GetWorld2D().DirectSpaceState;
            var query = PhysicsRayQueryParameters2D.Create(_lastPosition, GlobalPosition, CollisionMask);
            //GD.Print( GlobalPosition - _lastPosition + " " );
            var result = spaceState.IntersectRay(query);
            if (result.Count > 0) // if we hit something
            {
                
                if (result["collider"].Obj is Area2D area)
                {
                    
                    OnAreaEntered(area);
                }
                else if (result["collider"].Obj is PhysicsBody2D body)
                {
                    
                    OnBodyEntered(body);
                }
                
            }
        }
        _lastPosition = GlobalPosition;
        
    }

    public void Disable()
    {
        Monitoring = false;
    }
    
    public void Enable()
    {
        Monitoring = true;
    }
    
    
    /// <summary>
    /// Initialize the projectile with required parameters
    /// </summary>
    /// <param name="source">source of the projectile</param>
    /// <param name="team">team of the source</param>
    /// <param name="position">global position</param>
    /// <param name="rotation"> rotation in radians</param>
    public void Init(AbstractCreature source, AbstractCreature.Teams team, Vector2 position, float rotation = 0f)
    {
        GlobalPosition = position;
        startPosition = position;
        GlobalRotation = rotation;
        Source = source;
        Team = team;
        _lastPosition = GlobalPosition;
    }
    
    public void Init(AbstractCreature source, AbstractCreature.Teams team, float rotation = 0f)
    {
        GlobalRotation = rotation;
        Source = source;
        Team = team;
        startPosition = Position;
        _lastPosition = Position;
    }
    
    
    public virtual void OnAreaEntered(Area2D area)
    {
        // enemy check
        if (area is not HurtBox hurtBox) return;
        var blocker = hurtBox.GetParentBlocker();
        if (blocker == null) return;
        if (!blocker.CanBlockProjectile(this)) return;
        if (blocker.Team == Team || ExclusionList.Contains(blocker)) return;
        blocker.OnProjectileBlocked(this);
        
        if (DestroyOnHit) QueueFree();
         if (blocker is AbstractCreature creature)
        {
           
            creature.Damage(new DamageReport(Damage, Source, creature, Source.Stats, creature.Stats));
            OnHitCreature(creature);
            if (Knockback > 0)
            {
                var dir = GetKnockbackDirection(creature);
                creature.Knockback(dir * Knockback);
            }
        }
        
    }

    protected virtual Vector2 GetKnockbackDirection(AbstractCreature creature)
    {
        return GlobalPosition.DirectionTo(creature.GlobalPosition);
    }

    public virtual void OnAreaExited(Area2D area)
    {
    }

    public virtual void OnBodyEntered(Node body)
    {
        
        if (body is TileMap tileMap)
        {
            OnHitWall();
            if (!DestroyOnWall) return;
            QueueFree();
            return;
        }
        
        
        if (body is not PhysicsBody2D physBody ) return;
        // objects check
        if (!physBody.GetCollisionLayerValue(1)) return;
        OnHitWall();
        if (!DestroyOnWall) return;
        QueueFree();

}
    
    public virtual void OnBodyExited(Node body)
    {
    }

    public void OnShoot()
    {
    }


    public virtual void OnHitCreature(AbstractCreature creature)
    {
        Callbacks.Instance.EmitSignal(Callbacks.SignalName.ProjectileHit, this, creature);
    }
    
    public virtual void OnHitWall()
    {
        Callbacks.Instance.EmitSignal(Callbacks.SignalName.ProjectileHitWall, this);
    }

    public override void _Process(double delta) {
        base._Process(delta);
        if (Range > 0 && startPosition.DistanceTo(GlobalPosition) > Range)
        {
            QueueFree();
        }
    }

    
    
}
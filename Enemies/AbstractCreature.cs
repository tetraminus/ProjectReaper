using Godot;
using Godot.Collections;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Globals;
using ProjectReaper.Items;
using ProjectReaper.Objects;
using ProjectReaper.Player;
using ProjectReaper.Powers;
using ProjectReaper.Util;

namespace ProjectReaper.Enemies;

public abstract partial class AbstractCreature : CharacterBody2D, IProjectileBlocker
{
    public enum HitBoxState
    {
        Normal,
        Invincible,
        Spectral
    }

    public bool Dead;

    public Stats Stats { get; set; } = new();

    public Area2D Hurtbox { get; set; }
    public Node Items { get; set; }
    public Teams Team { get; set; } = Teams.Enemy;
    public Array<AbstractPower> Powers { get; set; } = new();

    public HitBoxState HitState { get; set; } = HitBoxState.Normal;
    
    public int NavGroup;


    public override void _Ready()
    {
        Stats.Init();
        Hurtbox = FindChild("Hurtbox") as Area2D;
        Items = new Node
        {
            Name = "Items"
        };
        AddChild(Items);
        
    }

    public virtual void OnHit()
    {
        
    }
    
    public void AddPower(AbstractPower power)
    {
        power.Creature = this;
        AddChild(power);
        power.OnApply();
        Powers.Add(power);
    }
    
    public bool HasPower(string id)
    {
        foreach (var power in Powers)
            if (power.Id == id)
                return true;
        return false;
    }
    
    public AbstractPower GetPower(string id)
    {
        foreach (var power in Powers)
            if (power.Id == id)
                return power;
        return null;
    }
    
    public void RemovePower(AbstractPower power)
    {
        power.OnRemove();
        Powers.Remove(power);
        power.QueueFree();
    }
    

    public int GetItemStacks<T>() where T : AbstractItem
    {
        var stacks = 0;
        foreach (var item in Items.GetChildren())
            if (item is T)
                stacks += (item as AbstractItem).Stacks;
        return stacks;
    }

    public int GetItemStacks(string id)
    {
        var stacks = 0;
        foreach (var item in Items.GetChildren())
            if (item is AbstractItem abstractItem && abstractItem.Id == id)
                stacks += abstractItem.Stacks;
        return stacks;
    }

    public bool HasItem<T>() where T : AbstractItem
    {
        foreach (var item in Items.GetChildren())
            if (item is T)
                return true;

        return false;
    }

    public bool HasItem(string id)
    {
        foreach (var item in Items.GetChildren())
            if (item is AbstractItem abstractItem && abstractItem.Id == id)
                return true;
        return false;
    }

    private AbstractItem GetItem(string id)
    {
        foreach (var item in Items.GetChildren())
            if (item is AbstractItem abstractItem && abstractItem.Id == id)
                return abstractItem;

        return null;
    }


    public virtual void AddItem(string id, int stacks = 1, bool isPlayer = false)
    {
         var item = GetItem(id);
        if (item != null)
        {
            item.Stacks += stacks;
            item.Gain(stacks);
        }
        else
        {
            var newItem = ItemLibrary.Instance.CreateItem(id);
            newItem.Stacks = stacks;
            Items.AddChild(newItem);
            newItem.Gain(stacks);
            if (isPlayer)
                GameManager.PlayerHud.AddItem(newItem);
        }
    }
    
    public virtual void AddItem(AbstractItem item)
    {
        var existingItem = GetItem(item.Id);
        if (existingItem != null)
        {
            existingItem.Stacks += item.Stacks;
            existingItem.Gain(item.Stacks);
        }
        else
        {
            if (item.Stacks <= 0) item.Stacks = 1;
                
            Items.AddChild(item);
            item.Gain(1);
            GameManager.PlayerHud.AddItem(item);
        }
    }
    
   


    /// <summary>
    ///  Called when the creature should die. Triggers death events
    /// </summary>
    public virtual void OnDeath()
    {
        Callbacks.Instance.EmitSignal(Callbacks.SignalName.CreatureDied, this);
        QuietDie();
    }
    
    /// <summary>
    ///  Kills the creature. does not trigger death event
    /// </summary>
    public void QuietDie()
    {
        Dead = true;
        QueueFree();
    }
    
    
    
    public CollisionShape2D GetCollisionShape()
    {
        CollisionShape2D biggest = null;
        foreach (var child in GetChildren())
        {
            if (child is CollisionShape2D shape)
            {
                if (biggest == null)
                {
                    biggest = shape;
                    continue;
                }
                
                if (shape.Shape.GetRect().Area > biggest.Shape.GetRect().Area)
                    biggest = shape;
                
            }
        }
        
        if (biggest == null)
            GD.PrintErr("No collision shape found");
        
        return biggest;
        
    }


    public void Damage(DamageReport damageReport)
    {
        if (Dead || HitState == HitBoxState.Invincible) return;

        var damage = damageReport.Damage;
        var source = damageReport.Source;
        var target = damageReport.Target;
        var sourceStats = damageReport.SourceStats;
        var targetStats = damageReport.TargetStats;

        var calculatedReport = Stats.CalculatedDamageReport(damageReport);
        var finalDamage = calculatedReport.Damage;
        
        
        Callbacks.Instance.EmitSignal(Callbacks.SignalName.CreatureDamaged, this, finalDamage);

        calculatedReport.finalDamage = Callbacks.Instance.FinalDamageEvent?.Invoke(this, finalDamage) ?? finalDamage;

        GameManager.SpawnDamageNumber(GlobalPosition, calculatedReport);
        
        Stats.Health -= finalDamage;
        if (Stats.Health <= 0)
            OnDeath();
        else
            OnHit();
    }
    
    public enum Teams
    {
        Player,
        Enemy
    }

    public void Knockback(Vector2 knockback)
    {
        if (Dead) return;
        
        Velocity += knockback;
    }

    public bool IsPlayer()
    {
        return Team == Teams.Player;
    }

    public virtual bool CanBlockProjectile(AbstractDamageArea projectile)
    {
        if (HitState == HitBoxState.Spectral) return false;
        return true;
    }

    public virtual void OnProjectileBlocked(AbstractDamageArea projectile)
    {
        Callbacks.Instance.EmitSignal(Callbacks.SignalName.ProjectileHit, projectile);
    }

    public virtual float AimDirection() {
        return GlobalRotation;
    }
    
}
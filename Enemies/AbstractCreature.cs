using Godot;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Globals;
using ProjectReaper.Items;
using ProjectReaper.Player;
using ProjectReaper.Util;

namespace ProjectReaper.Enemies;

public abstract partial class AbstractCreature : CharacterBody2D
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

    public HitBoxState HitState { get; set; } = HitBoxState.Normal;


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
            GD.Print("Adding stacks to item");
            item.Stacks += stacks;
        }
        else
        {
            var newItem = ItemLibrary.Instance.CreateItem(id);
            newItem.Stacks = stacks;
            Items.AddChild(newItem);
            newItem.Gain();
            PlayerHud.Instance.AddItem(newItem);
        }
    }
    
    public virtual void AddItem(AbstractItem item)
    {
        var existingItem = GetItem(item.Id);
        if (existingItem != null)
        {
            existingItem.Stacks += item.Stacks;
        }
        else
        {
            Items.AddChild(item);
            item.Gain();
            PlayerHud.Instance.AddItem(item);
        }
    }


    public virtual void OnDeath()
    {
        Callbacks.Instance.CreatureDiedEvent?.Invoke(this);
        Dead = true;
        QueueFree();
    }


    public void Damage(DamageReport damageReport)
    {
        if (Dead || HitState == HitBoxState.Invincible) return;

        var damage = damageReport.Damage;
        var source = damageReport.Source;
        var target = damageReport.Target;
        var sourceStats = damageReport.SourceStats;
        var targetStats = damageReport.TargetStats;

        var finalDamage = Stats.CalculateDamage(damage, source, target, sourceStats, targetStats);

        finalDamage = Callbacks.Instance.FinalDamageEvent?.Invoke(this, finalDamage) ?? finalDamage;

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
        Velocity += knockback;
    }
}
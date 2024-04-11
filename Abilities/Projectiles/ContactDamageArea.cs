using System.Collections.Generic;
using Godot;
using Godot.Collections;
using ProjectReaper.Enemies;
using ProjectReaper.Objects;
using ProjectReaper.Util;

namespace ProjectReaper.Abilities.Projectiles;

public partial class ContactDamageArea : AbstractDamageArea
{
    public override float Speed { get; set; } = 0;
    [Export] public override float Damage { get; set; } = -1;

    [Export(PropertyHint.Range, "0,100")] 
    public override float Duration { get; set; } = 0.2f;
    public List<IProjectileBlocker> Blockers = new();
     
    public override void _Ready()
    {
        Source = GetParentCreature();
        
        Timer = new Timer();
        AddChild(Timer);
        Timer.OneShot = false;
        Timer.Timeout += OnTimerTimeout;
        
        base._Ready();
        DestroyOnHit = false;
        DestroyOnWall = false;
        Source.Ready += () =>
        {
            if (Damage < 0) Damage = Source.Stats.Damage;
            
        };
    }

    public override void OnAreaEntered(Area2D area)
    {
        
        if (area is not HurtBox hurtBox) return;
        if (Blockers.Contains(hurtBox.GetParentBlocker())) return;
        var creature = hurtBox.GetParentBlocker();
        if (creature == null) return;
        if (creature.Team == Team) return;
        if (Blockers.Count == 0)
        {
            Timer.Start(Duration);
            OnTimerTimeout();
        }
        Blockers.Add(creature);
        
    }
    
    public override void OnAreaExited(Area2D area)
    {
        if (area is not HurtBox hurtBox) return;
        var creature = hurtBox.GetParentBlocker();
        if (creature == null) return;
        if (creature.Team == Team) return;
        Blockers.Remove(creature);
        if (Blockers.Count == 0) Timer.Stop();
    }

    public void OnTimerTimeout()
    {
        Vector2 knockback = new Vector2(0, 0);
        foreach (var blocker in Blockers)
        {
            if (blocker is AbstractCreature creature)
            {
                creature.Damage(new DamageReport(Damage, Source, creature, Source.Stats, creature.Stats));
                knockback += (creature.GlobalPosition - GlobalPosition).Normalized();
            }
            
        }
        
        GetParentCreature().Knockback(knockback * -500);
        
    }
    
    
    
    

    public AbstractCreature GetParentCreature()
    {
        if (GetParent() is AbstractCreature creature)
            return creature;
        return null;
    }
}
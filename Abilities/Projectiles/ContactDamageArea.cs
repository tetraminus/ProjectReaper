using Godot;
using Godot.Collections;
using ProjectReaper.Enemies;
using ProjectReaper.Util;

namespace ProjectReaper.Abilities.Projectiles;

public partial class ContactDamageArea : AbstractDamageArea
{
    public override float Speed { get; set; } = 0;
    [Export(PropertyHint.Range, "0,100")] 
    public override float Damage { get; set; } = 10;
    public override float Duration { get; set; } = 0.2f;
    public Array<AbstractCreature> Creatures = new();
     
    public override void _Ready()
    {
        Source = GetParentCreature();
        Timer = new Timer();
        AddChild(Timer);
        Timer.OneShot = false;
        Timer.Timeout += OnTimerTimeout;
        
        base._Ready();
        DestroyOnHit = false;
    }

    public override void OnAreaEntered(Area2D area)
    {
        
        if (area is not HurtBox hurtBox) return;
        if (Creatures.Contains(hurtBox.GetParentCreature())) return;
        var creature = hurtBox.GetParentCreature();
        if (creature == null) return;
        if (creature.Team == Team) return;
        if (Creatures.Count == 0) Timer.Start(Duration);
        Creatures.Add(creature);
        
    }
    
    public override void OnAreaExited(Area2D area)
    {
        if (area is not HurtBox hurtBox) return;
        var creature = hurtBox.GetParentCreature();
        if (creature == null) return;
        if (creature.Team == Team) return;
        Creatures.Remove(creature);
        if (Creatures.Count == 0) Timer.Stop();
    }

    public void OnTimerTimeout()
    {
        Vector2 knockback = new Vector2(0, 0);
        foreach (var creature in Creatures)
        {
            if (creature.Dead) continue;
            creature.Damage(new DamageReport(Damage, Source, creature, GetParentCreature().Stats, creature.Stats));
            knockback = new Vector2(1,0).Rotated(GlobalPosition.AngleToPoint(creature.GlobalPosition));
            
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
using Godot;
using ProjectReaper.Components;
using ProjectReaper.Enemies;
using ProjectReaper.Powers;

public partial class FireZone : Area2D
{
    private CreatureTrackerComponent _creatureTracker;

    public AbstractCreature.Teams Team { get; set; }
    public AbstractCreature Source { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _creatureTracker = GetNode<CreatureTrackerComponent>("CreatureTrackerComponent");
    }

    
    public void OnTimerTimeout()
    {
        if (_creatureTracker.GetCreatures().Count <= 0) return;
        foreach (var creature in _creatureTracker.GetCreatures())
        {
            if (creature.Team == Team) continue;
            var power = new BurnPower();
            creature.AddPower(power);
            power.SetDuration(1);
            power.Damage = Source.Stats.Damage * 0.05f;
        }
    }
}
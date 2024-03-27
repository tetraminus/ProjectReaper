using Godot;
using System;
using ProjectReaper.Components;
using ProjectReaper.Enemies;

public partial class GravityZone : Area2D
{
	
	private CreatureTrackerComponent _creatureTracker;
	public AbstractCreature.Teams Team { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_creatureTracker = GetNode<CreatureTrackerComponent>("CreatureTrackerComponent");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_creatureTracker.GetCreatures().Count > 0)
		{
			foreach (var creature in _creatureTracker.GetCreatures() )
			{
				if (creature.Team == Team) continue;
				GD.Print(creature.GlobalPosition);
				var direction = (creature.GlobalPosition - GlobalPosition).Normalized();
				creature.Knockback(direction * (float)( -5000 * delta));
			}
		}
	}
}

using Godot;
using System;
using ProjectReaper.Components;

public partial class GravityZone : Area2D
{
	
	private CreatureTrackerComponent _creatureTracker;
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
			foreach (var creature in _creatureTracker.GetCreatures())
			{
				var direction = (creature.GlobalPosition - GlobalPosition).Normalized();
				creature.Knockback(direction * (float)( -1000 * delta));
			}
		}
	}
}

using Godot;
using Godot.Collections;
using ProjectReaper.Enemies;

namespace ProjectReaper.Components;

public partial class CreatureTrackerComponent : Node2D
{
	[Export] public Array<Area2D> Areas;
	
	private Array<AbstractCreature> _creatures = new Array<AbstractCreature>();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (var area in Areas)
		{
			area.BodyEntered += (body) =>
			{
				//GD.Print("Body entered " + body.Name);
				if (body is AbstractCreature creature)
				{
					_creatures.Add(creature);
				}
			};
			area.BodyExited += (body) =>
			{
				//GD.Print("Body exited " + body.Name);
				if (body is AbstractCreature creature)
				{
					_creatures.Remove(creature);
				}
			};
		}
		
	}
	
	public Array<AbstractCreature> GetCreatures()
	{
		return _creatures;
	}
	
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
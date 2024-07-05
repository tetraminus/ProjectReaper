using Godot;
using System;
using ProjectReaper.Globals;

public partial class BossAnimController : Node2D
{
	
	[Export] Node2D layer1;
	[Export] Node2D layer2;
	Vector2 _direction = Vector2.Zero;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	public void Target(Vector2 target)
	{
		_direction = GlobalPosition.DirectionTo(target);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		// layer1.Position = layer1.Position.Lerp(_direction.Normalized() * 8,  (float)delta * 2);
		// layer2.Position = layer2.Position.Lerp(_direction.Normalized() * 2,  (float)delta * 2); 
		
		layer1.Position = Mathutils.FriLerp(layer1.Position, _direction.Normalized() * 8, 2f, (float)delta); 
		layer2.Position = Mathutils.FriLerp(layer2.Position, _direction.Normalized() * 2, 2f, (float)delta);
	}
}

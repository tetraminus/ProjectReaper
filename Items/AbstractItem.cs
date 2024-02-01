using Godot;
using System;

public abstract partial class AbstractItem : Node2D
{
	
	public int Stacks { get; set; } = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	public int GetStacks() {
		return Stacks;
	}
	
	public abstract void init();

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

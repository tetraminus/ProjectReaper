using Godot;
using System;

public partial class TimedDeleteComponent : Node2D
{
	private Timer _timer;
	[Export] public float TimeToDelete = 1.0f;
	[Export] Node NodeToDelete { get; set; }
	[Export] bool DefferedDelete { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_timer = GetNode<Timer>("Timer");
		_timer.WaitTime = TimeToDelete;
		_timer.Timeout += () =>
		{
			if (DefferedDelete)
			{
				NodeToDelete.CallDeferred(Node.MethodName.QueueFree);
			}
			else
			{
				NodeToDelete.QueueFree();
			}
			
		};
		_timer.Start();
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

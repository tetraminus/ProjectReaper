using Godot;
using System;
using ProjectReaper.Globals;
using ProjectReaper.Player;

public partial class Portal : Node2D
{
	private bool ready = false;
	
	private AnimationPlayer _animationPlayer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_animationPlayer.Play("idle");
		GetTree().CreateTimer(1).Timeout += () => ready = true;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void OnBodyEntered(Node body)
	{
		if (body is Player && ready)
		{
			_animationPlayer.Play("Vanish");
			(body as Player).Hide();
			GameManager.GoToNextLevel();
		}
		
	}
}

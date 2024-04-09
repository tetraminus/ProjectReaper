using Godot;
using System;
using ProjectReaper.Globals;

/// <summary>
/// Main Start Node, 
/// </summary>
public partial class Main : Node2D
{
	
	[Signal]
	public delegate void TransitionCompleteEventHandler();
	
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameManager.MainNode = this;
		GameManager.GoToMainMenu();
		
	}
	
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

using Godot;
using System;
using ProjectReaper.Globals;

public partial class PauseMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameManager.PauseMenu = this;
		Hide();
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        
		if (Input.IsActionJustPressed("pause"))
		{
			if (GameManager.Paused)
			{
				GameManager.UnpauseGame();
			}
			else
			{
				GameManager.PauseGame();
			}
		}
        
	}
	
	public void OnQuitButtonPressed()
	{
		GameManager.GoToMainMenu();
	}
}

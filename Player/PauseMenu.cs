using Godot;
using System;
using ProjectReaper.Globals;

public partial class PauseMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	private Button _resumeButton;
	public override void _Ready()
	{
		_resumeButton = GetNode<Button>("%ResumeButton");
		GameManager.PauseMenu = this;
		FocusEntered += Focus;
		Hide();
		
	}
	
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        
		if (Input.IsActionJustPressed("pause"))
		{
			if (GameManager.Paused && GameManager.CurrentScreen == this)
			{
				GameManager.UnpauseGame();
			}
			else if (!GameManager.Paused)
			{
				GameManager.PauseGame();
			}
		}
        
	}
	
	public void OnResumeButtonPressed()
	{
		GameManager.UnpauseGame();
	}
	
	public void OnLibraryButtonPressed()
	{
		GameManager.GoToLibrary();
	}
	
	public void OnQuitButtonPressed()
	{
		GameManager.GoToMainMenu();
	}
	
	public void Focus()
	{
		_resumeButton.GrabFocus();
	}
}

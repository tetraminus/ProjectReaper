using Godot;
using System;
using ProjectReaper.Globals;
using ProjectReaper.Menu.ItemLibraryScreen;
using Control = Godot.Control;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	
	private ItemLibraryScreen _itemLibraryScreen;
	private VortexEffect _vortexEffect;
	private Control _menuScreen;
	private PackedScene libScene = ResourceLoader.Load<PackedScene>("res://Menu/ItemLibraryScreen/ItemLibraryScreen.tscn");
	
	public override void _Ready()
	{
		_menuScreen = GetNode<Control>("MenuScreen");
		
		FocusEntered += Focus;
		_vortexEffect = GetNode<VortexEffect>("%VortexEffect");
		
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_vortexEffect.Visible = Visible;
	}
	
	public void OnStartButtonPressed()
	{
		GameManager.StartRun();
	}
	
	public void OnLibraryButtonPressed()
	{
		GameManager.GoToLibrary();
	}
	
	public void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
	

	public void Focus()
	{
		GetNode<Control>("%StartButton").GrabFocus();
	}
}

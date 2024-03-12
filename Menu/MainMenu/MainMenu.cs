using Godot;
using System;
using ProjectReaper.Globals;
using ProjectReaper.Menu.ItemLibraryScreen;
using Control = Godot.Control;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	
	private ItemLibraryScreen _itemLibraryScreen;
	private Control _menuScreen;
	private PackedScene libScene = ResourceLoader.Load<PackedScene>("res://Menu/ItemLibraryScreen/ItemLibraryScreen.tscn");
	
	public override void _Ready()
	{
		var library = libScene.Instantiate<ItemLibraryScreen>();
		AddChild(library);
		_itemLibraryScreen = library;
		_itemLibraryScreen.Hide();
		_itemLibraryScreen.CloseRequested += HideItemLibrary;
		
		_menuScreen = GetNode<Control>("MenuScreen");
		
		
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void OnStartButtonPressed()
	{
		GameManager.StartRun();
	}
	
	public void OnLibraryButtonPressed()
	{
		ShowItemLibrary();
	}
	
	public void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
	
	public void ShowItemLibrary()
	{
		_itemLibraryScreen.Show();
		_itemLibraryScreen.Focus();
		_menuScreen.Hide();
	}
	
	public void HideItemLibrary()
	{
		_itemLibraryScreen.Hide();
		_menuScreen.Show();
		Focus();
	}

	public void Focus()
	{
		GetNode<Control>("%StartButton").GrabFocus();
	}
}

using Godot;
using System;
/// <summary>
/// interface with the Resonate GDscript file
/// </summary>
public partial class AudioManager : Node
{
	public static AudioManager Instance;
	
	private Node _musicManager;
	private Node _soundManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		
		_soundManager = GetTree().Root.GetNode("SoundManager");
		_musicManager = GetTree().Root.GetNode("MusicManager");
		
	}
	
	public void PlaySound(string bank, string sound)
	{
		_soundManager.Call("play", bank, sound);
	}
	public void PlaySoundVaried(string bank, string sound, double pitch = 1.0, double volume = 1.0)
	{
		_soundManager.Call("play_varied", bank, sound, pitch, volume);
	}
	
	
	
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

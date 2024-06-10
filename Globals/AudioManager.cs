using System;
using Godot;

/// <summary>
///     interface with the Resonate GDscript file
/// </summary>
public partial class AudioManager : Node
{
    [Signal]
    public delegate void MusicManagerLoadedEventHandler();

    [Signal]
    public delegate void MusicManagerUpdatedEventHandler();

    [Signal]
    public delegate void SoundManagerLoadedEventHandler();

    [Signal]
    public delegate void SoundManagerUpdatedEventHandler();
    
    [Signal] public delegate void MusicManagerLoopedEventHandler(string bank, string music);

    public static AudioManager Instance;

    private Node _musicManager;
    private Node _soundManager;

    public bool IsMusicManagerLoaded => _musicManager.Get("has_loaded").AsBool();

    public bool IsSoundManagerLoaded => _soundManager.Get("has_loaded").AsBool();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance = this;

        _soundManager = GetTree().Root.GetNode("SoundManager");
        _musicManager = GetTree().Root.GetNode("MusicManager");

        _musicManager.Connect("loaded", Callable.From(OnMusicManagerLoaded));
        _soundManager.Connect("loaded", Callable.From(OnSoundManagerLoaded));

        _musicManager.Connect("updated", Callable.From(OnMusicManagerUpdated));
        _soundManager.Connect("updated", Callable.From(OnSoundManagerUpdated));

        _musicManager.Connect("song_loop_completed", Callable.From(new Action<string,string>(OnMusicManagerLooped)));
    }

    private void OnMusicManagerLoaded()
    {
        EmitSignal(SignalName.MusicManagerLoaded);
    }

    private void OnSoundManagerLoaded()
    {
        EmitSignal(SignalName.SoundManagerLoaded);
    }

    private void OnMusicManagerUpdated()
    {
        EmitSignal(SignalName.MusicManagerUpdated);
    }

    private void OnSoundManagerUpdated()
    {
        EmitSignal(SignalName.SoundManagerUpdated);
    }
    
    private void OnMusicManagerLooped(string bank, string music)
    {
        EmitSignal(SignalName.MusicManagerLooped, bank, music);
    }
        
    /*--------------------Sound--------------------*/
    /// <summary>
    ///    Play a sound from a bank
    /// </summary>
    /// <param name="bank"></param>
    /// <param name="sound"></param>
    public void PlaySound(string bank, string sound)
    {
        _soundManager.Call("play", bank, sound);
    }
    
    /// <summary>
    ///   Play a sound from a bank at a specific position
    /// </summary>
    /// <param name="bank"></param>
    /// <param name="sound"></param>
    /// <param name="position"></param>
    public void PlaySoundAtPosition(string bank, string sound, Vector2 position)
    {
        _soundManager.Call("play_at_position", bank, sound, position);
    }
    
    /// <summary>
    ///  Play a sound from a bank attached to a specific node
    /// </summary>
    /// <param name="bank"></param>
    /// <param name="sound"></param>
    /// <param name="node"></param>
    public void PlaySoundOnNode(string bank, string sound, Node node)
    {
        _soundManager.Call("play_on_node", bank, sound, node);
    }

    /// <summary>
    /// Play a sound from a bank with varied pitch and volume
    /// </summary>
    /// <param name="bank"></param>
    /// <param name="sound"></param>
    /// <param name="pitch"></param>
    /// <param name="volume"></param>
    public void PlaySoundVaried(string bank, string sound, double pitch = 1.0, double volume = 1.0)
    {
        _soundManager.Call("play_varied", bank, sound, pitch, volume);
    }
    
   
    public void PlaySoundAtPositionVaried(string bank, string sound, Vector2 position, double pitch = 1.0, double volume = 1.0)
    {
        _soundManager.Call("play_at_position_varied", bank, sound, position, pitch, volume);
    }
    
    public void PlaySoundOnNodeVaried(string bank, string sound, Node node, double pitch = 1.0, double volume = 1.0)
    {
        _soundManager.Call("play_on_node_varied", bank, sound, node, pitch, volume);
    }
    
    /*--------------------Music--------------------*/

    public void PlayMusic(string bank, string music, float time = 0f)
    {
        _musicManager.Call("play", bank, music, time);
    }

    public void EnableStem(string name)
    {
        _musicManager.Call("enable_stem", name);
    }

    public void DisableStem(string name)
    {
        _musicManager.Call("disable_stem", name);
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void StopMusic(float time = 0f)
    {
        _musicManager.Call("stop", time);
    }

    public void PlayStem(string stem, float time)
    {
        _musicManager.Call("enable_stem", stem, time);
    }

    public void StopStem(string stem, float time)
    {
        _musicManager.Call("disable_stem", stem, time);
    }

    public void SetMasterVolume(float value)
    {
        // value [0, 100] -> db
        var valueDb = Mathf.LinearToDb(value / 100);
        
        
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), valueDb);
        GD.Print("Set volume to " + valueDb);
    }
}
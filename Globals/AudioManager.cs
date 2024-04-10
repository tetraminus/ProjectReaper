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


    public void PlaySound(string bank, string sound)
    {
        _soundManager.Call("play", bank, sound);
    }

    public void PlaySoundVaried(string bank, string sound, double pitch = 1.0, double volume = 1.0)
    {
        _soundManager.Call("play_varied", bank, sound, pitch, volume);
    }

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
}
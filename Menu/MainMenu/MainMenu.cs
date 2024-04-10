using Godot;
using ProjectReaper.Globals;
using ProjectReaper.Menu.ItemLibraryScreen;
using ProjectReaper.Menu.MainMenu;
using Control = Godot.Control;

public partial class MainMenu : Control
{
    // Called when the node enters the scene tree for the first time.

    private Bg _bg;
    private ItemLibraryScreen _itemLibraryScreen;
    private Control _menuScreen;


    public override void _Ready()
    {
        _menuScreen = GetNode<Control>("MenuScreen");

        FocusEntered += Focus;


        _menuScreen.Modulate = new Color(1, 1, 1, 0);
        _menuScreen.ProcessMode = Control.ProcessModeEnum.Disabled;

        FadeIn();

        GetBgNode();
    }

    public void FadeIn(float interval = 1f, float fadetime = 1.5f)
    {
        var tween = GetTree().CreateTween();
        tween.TweenInterval(interval);
        tween.TweenProperty(_menuScreen, "modulate", new Color(1, 1, 1), fadetime);
        tween.Finished += () =>
        {
            _menuScreen.ProcessMode = Control.ProcessModeEnum.Inherit;
            
            SetProcessInput(true);
        };
        tween.Play();
    }

    public void FadeOut(float interval = 1f, float fadetime = 1.5f)
    {
        SetProcessInput(false);
        _menuScreen.ProcessMode = Control.ProcessModeEnum.Disabled;
        
        var tween = GetTree().CreateTween();
        tween.TweenInterval(interval);
        tween.TweenProperty(_menuScreen, "modulate", new Color(1, 1, 1, 0), fadetime);
        tween.Finished += () =>
        {
            
        };
        tween.Play();
    }


    public async void GetBgNode()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        _bg = GameManager.MainNode.GetNode<Bg>("%Bg");
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void HideBg()
    {
        if (_bg != null) _bg.Visible = false;
    }

    public void ShowBg()
    {
        if (_bg != null) _bg.Visible = true;
    }

    public void OnStartButtonPressed()
    {
        if (!GameManager.fadingOut) GameManager.StartRun();
    }

    public void OnLibraryButtonPressed()
    {
        if (!GameManager.fadingOut) GameManager.GoToLibrary();
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
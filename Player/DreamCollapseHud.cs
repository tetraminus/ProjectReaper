using Godot;
using System;
using ProjectReaper.Globals;

public partial class DreamCollapseHud : Control
{
	
	private Label _dreamCollapseLabel;
	private TextureRect _timerTexture;
	private TextureProgressBar _timerProgressBar;
	private Vector2 _basePosition;
	private Vector2 _offset = new Vector2(0, 0);
	private Vector2 _truePosition;
	
	private double _time = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_basePosition = Position;
		
		_dreamCollapseLabel = GetNode<Label>("Abbr/DreamCollapseText");
		_dreamCollapseLabel.Visible = false;
		
		_timerTexture = GetNode<TextureRect>("Abbr/TimerBg");
		
		_timerProgressBar = GetNode<TextureProgressBar>("TimerGroup/TextureProgressBar");

		Setup();

	}
	
	private async void Setup()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		Callbacks.Instance.CollapseStart += StartCollapse;
		Callbacks.Instance.LevelLoaded += Reset;
	}
	
	public void Reset()
	{
		_dreamCollapseLabel.Visible = false;
		_time = 0;
		_timerTexture.Visible = true;
		_timerProgressBar.Visible = true;
	}
	
	public void StartCollapse()
	{
		_dreamCollapseLabel.Visible = true;
		_time = 0;
		_timerTexture.Visible = false;
		_timerProgressBar.Visible = false;
		
	}
	
	public void HideHud()
	{
		var tween = GetTree().CreateTween();
		tween.SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(this, "_truePosition", _basePosition + new Vector2(0, +200), 0.5);
		
		
	}
	
	
	public void ShowHud()
	{
		var tween = GetTree().CreateTween();
		tween.TweenProperty(this, "_truePosition", _basePosition, 0.5);
		
	}
	
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_time += delta;
		// back and forth rotation once per second
		Rotation = (float)Math.Sin(_time * Math.PI) * 0.1f;
        _offset = new Vector2(0, (float)Math.Sin(_time * Math.Tau) * 10) ;
        
        Position = _truePosition + _offset;
		
		if (GameManager.Level != null)
		{
			_timerProgressBar.MaxValue = (float)(GameManager.Level.CollapseTimeMax);
			_timerProgressBar.Value = (float)(GameManager.Level.CollapseTime);
		}
			
	}
}

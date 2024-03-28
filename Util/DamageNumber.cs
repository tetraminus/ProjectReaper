using Godot;
using ProjectReaper.Util;


public partial class DamageNumber : Node2D
{
	private Label _label;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_label = GetNode<Label>("%number");
		_label.LabelSettings = new LabelSettings();
		GetNode<AnimationPlayer>("AnimationPlayer").Play("main");
	}
	
	public void SetNumber(DamageReport report)
	{
		var number = report.Damage;
		// number with 1 decimal
		_label.Text = number.ToString("0.0");
		for (int i = 0; i < report.critlv; i++)
		{
			_label.Text += "!";
			
		}

		if (number > 200)
		{
			_label.LabelSettings.FontColor = Colors.Red;
		}
		else if (number > 100)
		{
			_label.LabelSettings.FontColor = Colors.Orange;
		}
		else if (number > 50)
		{
			_label.LabelSettings.FontColor = Colors.Yellow;
		}
		else if (number > 0)
		{
			_label.LabelSettings.FontColor = Colors.White;
		}
		
		
		
	}
	
	public void OnAnimationFinished(string animName)
	{
		QueueFree();
	}
	
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

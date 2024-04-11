using Godot;
using ProjectReaper.Globals;

namespace ProjectReaper.Menu.MainMenu;

public partial class Bg : ColorRect
{
	private ShaderMaterial _shaderMaterial;
	
	[Export] public float ColorSpeed = 0.5f;
	[Export]
	private float _baseColorSteps = 32f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_shaderMaterial = Material as ShaderMaterial;
		// randomize color
		_shaderMaterial.GetShaderParameter("spiral_color").AsColor()
			.ToHsv(out var Hue, out var saturation, out var outValue);

		var newcolor = Color.FromHsv(GD.Randf(), saturation, outValue);

		_shaderMaterial.SetShaderParameter("spiral_color", newcolor);

		_shaderMaterial.SetShaderParameter("color_steps", 0);
	}
	
	public void SwirlIn(float dur = 3)
	{
		var tween = GetTree().CreateTween();
		tween.TweenMethod(Callable.From<float>(f =>
		{
			_shaderMaterial.SetShaderParameter("color_steps", f);
		}), 0.0, _baseColorSteps, dur);
	
		tween.Play();
		
	}
	
	public void SwirlOut(float dur = 2)
	{
		var tween = GetTree().CreateTween();
		tween.TweenMethod(Callable.From<float>(f =>
		{
			_shaderMaterial.SetShaderParameter("color_steps", f);
		}), _baseColorSteps, 0.0, dur);
	
		tween.Play();
		tween.Finished += () =>
		{
			GameManager.MainNode.EmitSignal(Main.SignalName.TransitionComplete);
		};
	}
	
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var prevColor = _shaderMaterial.GetShaderParameter("spiral_color").AsColor();
		// hue shift
		prevColor.ToHsv(out var Hue, out var saturation,out var outValue);
		Hue += ColorSpeed * (float)delta;
		if (Hue > 1)
		{
			Hue = 0;
		}
		
		prevColor = Color.FromHsv(Hue, saturation, outValue);
		_shaderMaterial.SetShaderParameter("spiral_color", prevColor);
		

	}
}
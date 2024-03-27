using Godot;

namespace ProjectReaper.Menu.MainMenu;

public partial class Bg : ColorRect
{
	private ShaderMaterial _shaderMaterial;
	
	[Export] public float ColorSpeed = 0.5f;
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
		
		var tween = GetTree().CreateTween();
		//tween.TweenInterval(1);
		tween.TweenMethod(Callable.From<float>(f =>
		{
			_shaderMaterial.SetShaderParameter("color_steps", f);
		}), 0.0, 6.0, 4);
	
		tween.Play();
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
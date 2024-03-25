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
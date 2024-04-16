using Godot;
using System;

public partial class julia : ColorRect
{
	
	ShaderMaterial shaderMaterial;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		shaderMaterial = Material as ShaderMaterial;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		shaderMaterial.SetShaderParameter("alpha", (GetParent() as CanvasItem).Modulate.A);
		//GD.Print((GetParent() as CanvasItem).Modulate.A);
	}
}

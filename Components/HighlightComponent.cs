using Godot;
using System;
public partial class HighlightComponent : CanvasGroup
{
	
	private bool _on;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	public void Enable() {
		var mat = Material as ShaderMaterial;
		if (mat != null) mat.SetShaderParameter("on", true);
		_on = true;
	}
	
	public void Disable() {
		var mat = Material as ShaderMaterial;
		if (mat != null) mat.SetShaderParameter("on", false);
		_on = false;
	}
	
	public bool IsOn() {
		return _on;
	}
	
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}

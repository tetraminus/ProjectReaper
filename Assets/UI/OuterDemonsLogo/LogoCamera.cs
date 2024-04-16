using Godot;

namespace ProjectReaper.Assets.UI.OuterDemonsLogo;

public partial class LogoCamera : Camera3D
{
	
	private Vector3 _origin;
	private float _time = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_origin = GlobalPosition;
		_time = GD.Randf() * Mathf.Tau;
		GlobalPosition = _origin + (new Vector3(Mathf.Cos(_time), Mathf.Sin(_time), 0) * 1);
		LookAt(Vector3.Zero, Vector3.Up);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// move in a slow circle on the xy plane
		_time +=(float) delta * 0.5f;
		
		GlobalPosition = _origin + (new Vector3(Mathf.Cos(_time ), Mathf.Sin(_time), 0) * 1);
		
		LookAt(Vector3.Zero, Vector3.Up);
	}
}
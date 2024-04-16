using Godot;

namespace ProjectReaper.Components;

public partial class UiWiggler3D : Node3D
{
	private Vector3 _initialPosition;
	private Vector3 initialRotation;
	private Vector3 _velocity;
	private Vector3 _rotationVelocity;
	[Export] public float gravity = 1f;
	[Export] public float turbulence = 50f;
	[Export] public float mouseInfluence = 0.1f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_initialPosition = Position;
		initialRotation = Rotation;
		
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_velocity += (new Vector3(1, 0, 0)).Rotated(Vector3.Up, (float) GD.RandRange(0, Mathf.Pi * 2)) * (float)delta * turbulence;
		
		
		_rotationVelocity += (new Vector3(1, 0, 0)).Rotated(Vector3.Up, (float) GD.RandRange(0, Mathf.Pi * 2)) * (float)delta * turbulence/10f;
		
		_rotationVelocity += (new Vector3(0, 0, 1)).Rotated(Vector3.Up, (float) GD.RandRange(0, Mathf.Pi * 2)) * (float)delta * turbulence/10f;
		
		// mouse influence
		var mousePos = GetViewport().GetMousePosition();
		var projectedMousePos = GetViewport().GetCamera3D().ProjectPosition(mousePos,1);
		
		var dir = projectedMousePos - GlobalPosition;
		var distance = dir.Length();
		
		_velocity += dir.Normalized() * mouseInfluence * (float) delta * Mathf.Clamp(1 - distance / 500, 0, 1);
		
		// move
		Position += _velocity * (float) delta;
		Rotation += _rotationVelocity * (float) delta;
		
		_velocity = _velocity.Lerp(Vector3.Zero, 0.001f * (float) delta);
		// gravity based on distance and velocity 
		
		
		_velocity = _velocity.Lerp(_initialPosition - Position, gravity * (float) delta * (Position.DistanceTo(_initialPosition) / 10));
		_rotationVelocity = _rotationVelocity.Lerp( initialRotation - Rotation , gravity * (float) delta);
		
		
	}
	
	
}
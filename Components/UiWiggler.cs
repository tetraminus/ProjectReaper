using Godot;

namespace ProjectReaper.Components;

public partial class UiWiggler : Node2D
{
	private Vector2 _initialPosition;
	private float initialRotation;
	private Vector2 _velocity;
	private float _rotationVelocity;
	[Export] public float gravity = 1f;
	[Export] public float turbulence = 50f;
	[Export] public float mouseInfluence = 0.1f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_initialPosition = Position;
		
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_velocity +=  (new Vector2(0, 1)).Rotated((float) GD.RandRange(0, Mathf.Pi * 2)) * (float)delta * turbulence;
		_rotationVelocity += (float) GD.RandRange(-1, 1) * (float)delta * turbulence/10f;
		
		// mouse influence
		var mousePos = GetGlobalMousePosition();
		var dir = mousePos - GlobalPosition;
		var distance = dir.Length();
		
		_velocity += dir.Normalized() * mouseInfluence * (float) delta * Mathf.Clamp(1 - distance / 500, 0, 1);
		
		// move
		Position += _velocity * (float) delta;
		Rotation += _rotationVelocity * (float) delta;
		
		_velocity = _velocity.Lerp(Vector2.Zero, 0.001f * (float) delta);
		// gravity based on distance and velocity 
		
		
		_velocity = _velocity.Lerp(_initialPosition - Position, gravity * (float) delta * (Position.DistanceTo(_initialPosition) / 10));
		_rotationVelocity = Mathf.Lerp(_rotationVelocity,  initialRotation -Rotation , gravity * (float) delta);
		
		
	}
	
	
}
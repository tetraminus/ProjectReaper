using Godot;

namespace ProjectReaper.Components;

public partial class UiWiggler : Node2D
{
	private Vector2 _initialPosition;
	private float initialRotation;
	private Vector2 _velocity;
	[Export] public float gravity = 1f;
	[Export] public float turbulence = 50f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_initialPosition = Position;
		
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_velocity +=  (new Vector2(0, 1)).Rotated((float) GD.RandRange(0, Mathf.Pi * 2)) * (float)delta * turbulence;
		
		
		Position += _velocity * (float) delta;
		
		_velocity = _velocity.Lerp(Vector2.Zero, 0.001f * (float) delta);
		// gravity based on distance and velocity 
		
		
		_velocity = _velocity.Lerp(_initialPosition - Position, gravity * (float) delta );
		
		Rotation = _velocity.Angle();
		
	}
	
	
}
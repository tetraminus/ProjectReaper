using Godot;
using ProjectReaper.Globals;

namespace ProjectReaper.Enemies;

public partial class Goober : AbstractCreature
{
	private AnimatedSprite2D _sprite;
	private NavigationAgent2D _navigationAgent;
	private Vector2 _movementTargetPosition = Vector2.Zero;
	private bool _directSight = false;

	private const float Accelfac = 20.0f;
	public Vector2 MoveDirection { get; set; }

	
	
	
	public Vector2 MovementTarget
	{
		get { return _navigationAgent.TargetPosition; }
		set { _navigationAgent.TargetPosition = value; }
	}

	public override void _Ready()
	{
		base._Ready();
		Stats.Speed = 80;
		Stats.Health = 20;
		Stats.MaxHealth = 20;

		_sprite = GetNode<AnimatedSprite2D>("Sprite");
		_sprite.Play();
		
		_navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");

		// These values need to be adjusted for the actor's speed
		// and the navigation layout.
		_navigationAgent.PathDesiredDistance = 4.0f;
		_navigationAgent.TargetDesiredDistance = 4.0f;

		// Make sure to not await during _Ready.
		Callable.From(ActorSetup).CallDeferred();

		Callbacks.Instance.EnemyRenav += Renav;
		
		

	}

	public override void _ExitTree()
	{
		Callbacks.Instance.EnemyRenav -= Renav;
		base._ExitTree();
	}

	private void Renav(Vector2 position, int group)
	{
		if (group != NavGroup) return;
		
		MovementTarget = position;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		MoveAndSlide();
	}

	public override void _PhysicsProcess(double delta)
	{
		// raycast to player
		var player = GameManager.Player;
		if (player.Dead) return;

		if (player.GlobalPosition.DistanceTo(GlobalPosition) < 500){
			var parameters = new PhysicsRayQueryParameters2D();
			parameters.From = GlobalPosition;
			parameters.To = player.GlobalPosition;
			// bit 1 is terrain
			parameters.CollisionMask = 1;

			var ray = GetWorld2D().DirectSpaceState.IntersectRay(parameters);

			_directSight = ray.Count == 0;
		}
		else
		{
			_directSight = false;
		}

		// follow nav path if no direct sight
		if (!_directSight)
		{
			FollowPath(delta);
		}
		// else move towards player
		else
		{
			MoveDirection = (player.GlobalPosition - GlobalPosition).Normalized();
			Velocity += MoveDirection * Stats.Speed * (float)delta * Accelfac;
		}

		// simulate friction with delta
		if (Velocity.Length() > Stats.Speed || MoveDirection == Vector2.Zero)
		{
			Velocity = Velocity.Lerp(Vector2.Zero, 0.1f);
		}
	}
	private void FollowPath(double delta)
	{
		var nextPos = _navigationAgent.GetNextPathPosition();
		if (nextPos != Vector2.Zero)
		{
			MoveDirection = (nextPos - GlobalPosition).Normalized();
			Velocity += MoveDirection * Stats.Speed * (float)delta * Accelfac;
		}
		else
		{
			MoveDirection = Vector2.Zero;
		}

		// simulate friction with delta
		if (Velocity.Length() > Stats.Speed || MoveDirection == Vector2.Zero)
		{
			Velocity = Velocity.Lerp(Vector2.Zero, 0.1f);
		}
	}

	private async void ActorSetup()
	{
		// Wait for the first physics frame so the NavigationServer can sync.
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

		// Now that the navigation map is no longer empty, set the movement target.
		MovementTarget = GameManager.Player.GlobalPosition;
	}
}

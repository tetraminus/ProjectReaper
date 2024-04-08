using Godot;
using GodotStateCharts;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Globals;

namespace ProjectReaper.Enemies.Slime;

public partial class Slimebert : AbstractCreature
{
	private AnimatedSprite2D _sprite;
	private NavigationAgent2D _navigationAgent;
	private Vector2 _movementTargetPosition = Vector2.Zero;
	private bool _directSight = false;

	private const float Accelfac = 20.0f;
	private const float TargetChaseDistance = 125;
	private PackedScene _bulletScn = GD.Load<PackedScene>("res://Enemies/Slime/SlimeBullet.tscn");
	
	public Vector2 MoveDirection { get; set; }
	private StateChart _stateChart;
	
	public Vector2 MovementTarget
	{
		get { return _navigationAgent.TargetPosition; }
		set { _navigationAgent.TargetPosition = value; }
	}

	public override void _Ready()
	{
		base._Ready();
		Stats.Speed = 60;
		Stats.MaxHealth = 35 * (GameManager.GetRunDifficulty() + 2)/3; //divide by 3 for tier 1 enemies
		Stats.Health = Stats.MaxHealth;
		

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
		
		_stateChart = StateChart.Of(GetNode("StateChart"));

	}

	public override void _ExitTree()
	{
		Callbacks.Instance.EnemyRenav -= Renav;
		base._ExitTree();
	}
	
	public void Timeout()
	{
		_stateChart.SendEvent("Shoot");
	}

	private void Renav(Vector2 position, int group)
	{
		if (group != NavGroup) return;
		MovementTarget = position;
	}
	
	public override void _PhysicsProcess(double delta) {
		// raycast to player
		var player = GameManager.Player;
		if (player.Dead) return;

		if (player.GlobalPosition.DistanceTo(GlobalPosition) < 500) {
			var parameters = new PhysicsRayQueryParameters2D();
			parameters.From = GlobalPosition;
			parameters.To = player.GlobalPosition;
			// bit 1 is terrain
			parameters.CollisionMask = 1;

			var ray = GetWorld2D().DirectSpaceState.IntersectRay(parameters);

			_directSight = ray.Count == 0;
		}
		else {
			_directSight = false;
		}
		
		if (_directSight) {
			_stateChart.SendEvent("PlayerSeen");
		}
		else {
			_stateChart.SendEvent("PlayerLost");
		}
		Move(); 
	}

	public override void _Process(double delta)
	{
	   _sprite.FlipH = MoveDirection.X > 0;
	}

	public void EnterShoot()
	{
		if (GameManager.Player.Dead) return;
		var bullet = _bulletScn.Instantiate<BasicBullet>();
		bullet.Init(this, Team, GlobalPosition, GetAngleTo(GameManager.Player.GlobalPosition));
		bullet.Speed = 200;
		GameManager.Level.AddChild(bullet);
		MoveDirection = Vector2.Zero;
		Velocity = Vector2.Zero;
		
		
	}
	
	public void ChasePlayer(double delta)
	{
		var player = GameManager.Player;
		if (player.Dead) return;

		float distanceToPlayer = player.GlobalPosition.DistanceTo(GlobalPosition);

		if (distanceToPlayer > TargetChaseDistance + 50)
		{
			MoveDirection = (player.GlobalPosition - GlobalPosition).Normalized();
		}
		else if (distanceToPlayer < TargetChaseDistance)
		{
			MoveDirection = (GlobalPosition - player.GlobalPosition).Normalized();
		}
		else
		{
			MoveDirection = Vector2.Zero;
		}

		Velocity += MoveDirection * Stats.Speed * (float)delta * Accelfac;
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
			Velocity = Vector2.Zero;
		}

		MoveAndSlide();
	}
	private void Move() {
		// simulate friction with delta
		if (Velocity.Length() > Stats.Speed || MoveDirection == Vector2.Zero)
		{
			Velocity = Velocity.Lerp(Vector2.Zero, 0.1f);
		}
		
		MoveAndSlide();
	}

	private async void ActorSetup()
	{
		// Wait for the first physics frame so the NavigationServer can sync.
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

		// Now that the navigation map is no longer empty, set the movement target.
		MovementTarget = GameManager.Player.GlobalPosition;
	}
}

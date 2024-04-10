using Godot;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Components;
using ProjectReaper.Globals;


namespace ProjectReaper.Enemies.Bosses.Leech;
	
public partial class ShootState : AbstractState

{
	[Export] private float _shootTime = 0.5f;
	private Vector2 _shootDirection;
	private PackedScene _bulletScn = GD.Load<PackedScene>("res://Enemies/Bosses/Leech/LeechBullet.tscn");
	private Node2D shootPivot;
	private bool _shooting = true;
	public void Timeout()
	{
		var leechbert = StateMachine.Creature as Leechbert;
		var bullet = _bulletScn.Instantiate<BasicBullet>(); 
		bullet.GlobalPosition = leechbert.GlobalPosition;
		bullet.Init(leechbert, AbstractCreature.Teams.Enemy);
		GetTree().Root.AddChild(bullet);
		
	}
	
	
	
	
	
	public override void OnEnter(params object[] args)
    {
        EnterShoot();
        var leechbert = StateMachine.Creature as Leechbert;
        GetTree().CreateTimer(_shootTime).Timeout += LeaveShoot;
        _shootDirection = (GameManager.Player.GlobalPosition - leechbert.GlobalPosition).Normalized(); 
        shootPivot.Rotation = _shootDirection.Angle();
        _shooting = true;
	    
    }

	
	public override void OnExit()
    {
        _shooting = false;
    }
	
	
	
	
	public override void _Ready()
	{
		shootPivot = GetNode<Node2D>("%ShootPivot");

		

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void EnterShoot()
	{
		var leechbert = StateMachine.Creature as Leechbert;
		var bullet = _bulletScn.Instantiate<BasicBullet>(); 
		bullet.GlobalPosition = leechbert.GlobalPosition;
		_shootDirection = (GameManager.Player.GlobalPosition - leechbert.GlobalPosition).Normalized();
		bullet.Init(leechbert, AbstractCreature.Teams.Enemy, _shootDirection.Angle());
		GetTree().Root.AddChild(bullet);
		
		
		ShootState shootState = this;
		GetTree().CreateTimer(_shootTime).Timeout += LeaveShoot; 
		
		
	}
	
	
	private void LeaveShoot()
	{
		if (StateMachine.GetCurrentStateName() != Name) return;
		var timer = GetTree().CreateTimer(0.5f);
	    GetTree().CreateTimer(timer.TimeLeft).Timeout += ChangeToChargeState;
		
	}
	
	private void ChangeToChargeState()
	{
		StateMachine.ChangeState("ChargeState");
		
	}
}

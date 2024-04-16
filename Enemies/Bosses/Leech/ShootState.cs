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
	
	
	
	public override void OnEnter(params object[] args)
    {
        EnterShoot();
	    
    }

	
	public override void OnExit()
    {
        _shooting = false;
    }
	
	
	
	private void EnterShoot()
	{
		var leechbert = StateMachine.Creature as Leechbert;
		var bullet = _bulletScn.Instantiate<BasicBullet>(); 
		bullet.GlobalPosition = leechbert.GlobalPosition;
		_shootDirection = (GameManager.Player.GlobalPosition - leechbert.GlobalPosition).Normalized();
		bullet.Init(leechbert, AbstractCreature.Teams.Enemy, _shootDirection.Angle());
		bullet.Speed = 300;
		GetTree().Root.AddChild(bullet);
		
		StateMachine.ChangeState("ChaseState");
		
		
		
	}
	
}

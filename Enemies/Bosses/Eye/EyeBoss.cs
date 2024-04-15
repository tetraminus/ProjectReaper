using Godot;
using System;
using Godot.Collections;
using GodotStateCharts;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Player;

public partial class EyeBoss : AbstractCreature
{
	private BossAnimController _animController;
	private StateChart _stateChart;
	
	private float _burstfireTimer = 0;
	private float _burstfireDirection = 0;
	
	private float _floodfireTimer = 0;
	private int _floodfireWave = 0;
	
	[Export] public int FloodfireCount = 20;
	[Export] public Array<string> Attacks;
	
	
	private PackedScene BulletScn = GD.Load<PackedScene>("res://Abilities/Projectiles/BasicBullet.tscn");
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animController = GetNode<BossAnimController>("BossAnimController");
		_stateChart = StateChart.Of(GetNode("StateChart"));
		base._Ready();
		
		Stats.MaxHealth = 1000;
		Stats.Health = 1000;
		
		
	}

	public override float AimDirection()
	{
		return GlobalPosition.DirectionTo(GameManager.Player.GlobalPosition).Angle();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GameManager.Player != null)
		{
			_animController.Target(GameManager.Player.GlobalPosition);
		}
	}
	
	public void BurstfireEnter()
	{
		_burstfireTimer = 0;
		_burstfireDirection = AimDirection();
		
	}
	
	public void BurstfireProcess(float delta)
	{
		_burstfireTimer += delta;
		if (_burstfireTimer > 0.1)
		{
			_burstfireTimer = 0;
			FireBullet(_burstfireDirection);
		}
	}
	
	public void FloodfireEnter()
	{
		_floodfireTimer = 0;
		_floodfireWave = 0;
	}
	
	public void FloodfireProcess(float delta)
	{
		_burstfireTimer += delta;
		if (_burstfireTimer > 0.5)
		{
			_burstfireTimer = 0;
			_floodfireWave++;
			for (int i = 0; i < FloodfireCount; i++)
			{
				FireBullet( (Mathf.Tau / FloodfireCount * i) + _floodfireWave * Mathf.Tau / FloodfireCount/ 2f, 100);
			}
			
		}
	}
	
	public void PickNewAttack()
	{
		_stateChart.SendEvent(Attacks[GD.RandRange(0, Attacks.Count-1)] + "Picked");
	}

	public override void OnDeath()
	{
		Callbacks.Instance.EmitSignal(Callbacks.SignalName.BossDied);
		base.OnDeath();
	}


	private void FireBullet(float direction, float speed = 200)
	{
		var bullet = BulletScn.Instantiate<BasicBullet>();
		bullet.Init(this, Team, GlobalPosition, direction);
		bullet.Speed = speed;
		bullet.Duration = -1;
		GameManager.Level.AddChild(bullet);
	}
	
	
}

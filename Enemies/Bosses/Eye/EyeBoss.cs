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
	[Export] public int SummonCount = 6;
	[Export] public Array<string> Attacks;
	private Array<string> availableAttacks = new Array<string>();
	private Texture2D _bulletTexture = GD.Load<Texture2D>("res://Assets/Enemies/Eye_Blood.png");
	
	
	private PackedScene BulletScn = GD.Load<PackedScene>("res://Abilities/Projectiles/BasicBullet.tscn");
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animController = GetNode<BossAnimController>("BossAnimController");
		_stateChart = StateChart.Of(GetNode("StateChart"));
		base._Ready();
		
		Stats.MaxHealth = 1000;
		Stats.Health = 1000;
		availableAttacks.AddRange(Attacks);
		
		
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
	
	public void SummonEnter()
	{
		for (int i = 0; i < SummonCount; i++)
		{
			var enemy = SpawnDirector.Instance.GetRandomSpawn(50);
			var instance = enemy.GetEnemy().Instantiate<AbstractCreature>();
			
			instance.GlobalPosition = GlobalPosition + (Vector2.Right.Rotated(Mathf.Tau / SummonCount * i) * 50);
			instance.AddToGroup("spawnedenemies");
			GameManager.Level.AddChild(instance);
			
		}
	}
	
	public void PickNewAttack()
	{
		if (availableAttacks.Count == 0)
		{
			availableAttacks.AddRange(Attacks);
		}
		var attack = availableAttacks.PickRandom();
		availableAttacks.Remove(attack);
		_stateChart.SendEvent(attack + "Picked");
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
		bullet.Resprite(_bulletTexture, Vector2.Zero, -Mathf.Pi/2f);
	}
	
	
}

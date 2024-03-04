using Godot;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Components;
using ProjectReaper.Enemies;
using ProjectReaper.Objects;
using ProjectReaper.Util;

namespace ProjectReaper.Items.Prefabs;

public partial class ForcefieldOrbital : Node2D, IProjectileBlocker
{
	
	[Export] public float Distance { get; set; } = 20;
	[Export] public float Speed { get; set; } = 1;
	[Export] public float DissipateTime { get; set; } = 1f;
	public AbstractCreature.Teams Team { get; } = AbstractCreature.Teams.Player;
	private Timer _dissipateTimer = new Timer();
	private AnimatedSprite2D _sprite;
	private CreatureOwnerComponent CreatureOwnerComponent => GetNode<CreatureOwnerComponent>("CreatureOwnerComponent") ;
	private HurtBox Shield => GetNode<HurtBox>("shield");
	
	private bool _dissipated = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Shield.Position = new Vector2(Distance, 0);
		
		Shield.AreaEntered += OnAreaEntered;
		
		_dissipateTimer.OneShot = true;
		_dissipateTimer.Timeout += Reappear;
		AddChild(_dissipateTimer);
		
		_sprite = GetNode<AnimatedSprite2D>("shield/Sprite");
	}
	

	private void OnAreaEntered(Area2D area)
	{
		if (area is not HurtBox hurtBox) return;
		if (hurtBox.GetParentBlocker() is not AbstractCreature creature) return;
		Dissipate();
		creature.Damage(new DamageReport(5, creature));
		creature.Knockback(GlobalPosition.DirectionTo(creature.GlobalPosition) * 1000);
	}

	public void SetDistance(float distance)
	{
		Distance = distance;
		Shield.Position = new Vector2(Distance, 0);
	}
	
	public void SetSpeed(float speed)
	{
		Speed = speed;
	}
	
	public void SetRotation(float rotation)
	{
		Rotation = rotation;
	}
	
	public void Dissipate()
	{
		Shield.Disable();
		_sprite.Play("off");
		_dissipateTimer.Start(DissipateTime);
		_dissipated = true;
	}
	
	public void Reappear()
	{
		Shield.Enable();
		_sprite.Play("on");
		_dissipateTimer.Stop();
		_dissipated = false;
	}
	
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Rotate((float)delta * Speed);
		
	}

	public bool CanBlockProjectile(AbstractDamageArea projectile)
	{
		return true;
	}

	public void OnProjectileBlocked(AbstractDamageArea projectile)
	{
		Dissipate();
		
	}
}
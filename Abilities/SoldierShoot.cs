using Godot;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Abilities;

public partial class SoldierShoot : AbstractAbility
{
	public override void _Ready()
	{
		base._Ready();
		AttackSpeedEffectsCooldown = true;
	}

	private static PackedScene BulletScene { get; } =
		GD.Load<PackedScene>("res://Abilities/Projectiles/BasicBullet.tscn");

	public override float Cooldown { get; set; } = 0.2f;
	

	public override void Use()
	{
		AudioManager.Instance.PlaySoundVaried("player", "shoot", GD.RandRange(0.8,1.2));
		
		var bullet = (AbstractDamageArea)BulletScene.Instantiate();
		Callbacks.Instance.EmitSignal(Callbacks.SignalName.BulletCreated, bullet);
		var src = GameManager.Player;
		bullet.Init(src, src.Team, src.GlobalPosition,src.AimDirection());
		GetTree().Root.AddChild(bullet);
		
	}
}

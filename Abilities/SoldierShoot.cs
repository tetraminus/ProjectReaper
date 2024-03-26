using Godot;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Abilities;

public partial class SoldierShoot : AbstractAbility
{
	private static PackedScene BulletScene { get; } =
		GD.Load<PackedScene>("res://Abilities/Projectiles/BasicBullet.tscn");

	public override float Cooldown { get; set; } = 0.2f;
	

	public override void Use()
	{
		var bullet = (AbstractDamageArea)BulletScene.Instantiate();
		Callbacks.Instance.EmitSignal(Callbacks.SignalName.BulletCreated, bullet);
		var src = GameManager.Player;
		bullet.Init(src, src.Team, src.GlobalPosition,src.AimDirection());
		
		GetTree().Root.AddChild(bullet);
		
	}
}

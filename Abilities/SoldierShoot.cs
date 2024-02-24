using Godot;
using ProjectReaper.Abilities.Projectiles;

namespace ProjectReaper.Abilities;

public partial class SoldierShoot : AbstractAbility
{
    private static PackedScene BulletScene { get; } = GD.Load<PackedScene>("res://Abilities/Projectiles/BasicBullet.tscn");
    public override void Use()
    {
        var bullet = (AbstractDamageArea) BulletScene.Instantiate();
        Globals.Callbacks.Instance.BulletCreatedEvent?.Invoke(bullet);
        bullet.Position = Globals.GameManager.Player.GlobalPosition;
        bullet.LookAt(Globals.GameManager.Player.GetGlobalMousePosition());
        GetTree().Root.AddChild(bullet);
        bullet.Source = Globals.GameManager.Player;
    }

	public override float Cooldown { get; set; } = 0.2f;
	
}

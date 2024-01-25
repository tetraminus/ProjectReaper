using Godot;
using ProjectReaper.Abilities.Projectiles;

namespace ProjectReaper.Abilities;

public partial class EnemyShoot : AbstractAbility
{
    private static PackedScene BulletScene { get; } = GD.Load<PackedScene>("res://Abilities/Projectiles/BasicBullet.tscn");
    public override void Use()
    {
        var bullet = (AbstractDamageArea) BulletScene.Instantiate();
        Globals.Callbacks.Instance.EmitSignal(Globals.Callbacks.SignalName.BulletCreated, bullet);
        bullet.Position = GetParent<Node2D>().GlobalPosition;
        bullet.LookAt( GameManager.Player.GlobalPosition);
        GetTree().Root.AddChild(bullet);
    }

	public override float Cooldown { get; set; } = 0.2f;
	
}
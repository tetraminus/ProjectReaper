using Godot;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;

namespace ProjectReaper.Abilities;

public partial class SnowPeaShoot : AbstractAbility
{
    private static PackedScene BulletScene { get; } = GD.Load<PackedScene>("res://Abilities/Projectiles/BasicBullet.tscn");
    public override void Use(float angle)
    {
        var bullet = (AbstractDamageArea) BulletScene.Instantiate();
        Globals.Callbacks.Instance.EmitSignal(Globals.Callbacks.SignalName.BulletCreated, bullet);
        bullet.Position = GetParent<Node2D>().GlobalPosition;
        bullet.Rotation = angle;
        GetTree().Root.AddChild(bullet);
        bullet.Source = GetParent<AbstractCreature>();
    }

	public override float Cooldown { get; set; } = 0.2f;
	
}

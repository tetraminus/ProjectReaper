using Godot;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Abilities;

public partial class SnowPeaShoot : AbstractAbility
{
    private static PackedScene BulletScene { get; } =
        GD.Load<PackedScene>("res://Abilities/Projectiles/BasicBullet.tscn");

    public override float Cooldown { get; set; } = 0.2f;

    public override void Use(float angle)
    {
        var bullet = (AbstractDamageArea)BulletScene.Instantiate();
        bullet.Duration = 0.2f;
        Callbacks.Instance.EmitSignal(Callbacks.SignalName.BulletCreated, bullet);
        bullet.Position = GetParent<Node2D>().GlobalPosition;
        bullet.Rotation = angle;

        GetTree().Root.AddChild(bullet);
        bullet.Source = GetParent<AbstractCreature>();
    }
}
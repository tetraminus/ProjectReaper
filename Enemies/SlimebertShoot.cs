using Godot;
using ProjectReaper.Abilities;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Globals;

namespace ProjectReaper.Enemies;

public partial class SlimeBertShoot : AbstractAbility
{
    private static PackedScene BulletScene { get; } =
        GD.Load<PackedScene>("res://Abilities/Projectiles/SlimeBullet.tscn");

    public override float Cooldown { get; set; } = 0.2f;

    public override void Use()
    {
        var bullet = (AbstractDamageArea)BulletScene.Instantiate();
        Callbacks.Instance.BulletCreatedEvent?.Invoke(bullet);
        bullet.Position = GetParent<Node2D>().GlobalPosition;
        bullet.LookAt(GameManager.Player.GlobalPosition);
        GetTree().Root.AddChild(bullet);
        bullet.Source = GetParent<AbstractCreature>();
    }
}
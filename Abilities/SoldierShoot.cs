using Godot;
using ProjectReaper.Abilities.Projectiles;

namespace ProjectReaper.Abilities;

public partial class SoldierShoot : AbstractAbility
{
    private static PackedScene BulletScene { get; } = GD.Load<PackedScene>("res://Abilities/Projectiles/BasicBullet.tscn");
    public override void Use()
    {
        var bullet = (AbstractBullet) BulletScene.Instantiate();
        Globals.Callbacks.Instance.EmitSignal(Globals.Callbacks.SignalName.BulletCreated, bullet);
        bullet.Position = GameManager.Player.GlobalPosition;
        bullet.LookAt(GetViewport().GetMousePosition());
        GetTree().Root.AddChild(bullet);
    }

    public override float Cooldown { get; set; } = 0.2f;
    
}
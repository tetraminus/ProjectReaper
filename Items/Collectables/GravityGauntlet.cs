using Godot;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Items.Collectables;

public partial class GravityGauntlet : AbstractItem
{
    private static PackedScene _gravZoneScene = ResourceLoader.Load<PackedScene>("res://Items/Prefabs/GravityZone.tscn");
    public override string Id => "gravity_gauntlet";
    public override ItemRarity Rarity => ItemRarity.Rare;
    public override void OnInitalPickup()
    {
        Callbacks.Instance.ProjectileHit += OnBulletHit;
        Callbacks.Instance.ProjectileHitWall += OnBulletHit;
    }

    public override void Cleanup()
    {
        Callbacks.Instance.ProjectileHit -= OnBulletHit;
        Callbacks.Instance.ProjectileHitWall -= OnBulletHit;
    }
    
    private void OnBulletHit(AbstractDamageArea bullet)
    {
        if (bullet.Source == GetHolder() && GameManager.RollBool(0.10f))
        {

            var gravZone = _gravZoneScene.Instantiate<Node2D>();
            GameManager.Level.CallDeferred(Node.MethodName.AddChild, gravZone);
            gravZone.GlobalPosition = bullet.GlobalPosition;
        }

    }
    private void OnBulletHit(AbstractDamageArea bullet, AbstractCreature creature)
    {
        OnBulletHit(bullet);
    }
    
    
    
}
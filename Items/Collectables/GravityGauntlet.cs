using Godot;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Items.Collectables;

public partial class GravityGauntlet : AbstractItem
{
    private const float Sizeperstack = 0.3f;

    private static readonly PackedScene _gravZoneScene =
        ResourceLoader.Load<PackedScene>("res://Items/Prefabs/GravityZone.tscn");

    public override string Id => "gravity_gauntlet";
    public override ItemRarity Rarity => ItemRarity.Uncommon;

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
        if (bullet.Source == GetHolder() && GameManager.RollProc(0.10f, bullet, GetHolder().Stats.Luck))
        {
            var gravZone = _gravZoneScene.Instantiate<GravityZone>();
            gravZone.Scale *= new Vector2(1 + Sizeperstack * Stacks, 1 + Sizeperstack * Stacks);
            gravZone.Team = GetHolder().Team;
            GameManager.Level.CallDeferred(Node.MethodName.AddChild, gravZone);
            gravZone.GlobalPosition = bullet.GlobalPosition;
        }
    }

    private void OnBulletHit(AbstractDamageArea bullet, AbstractCreature creature)
    {
        OnBulletHit(bullet);
    }
}
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Items.Collectables;

public partial class GravityGauntlet : AbstractItem
{
    public override string Id => "gravity_gauntlet";
    public override ItemRarity Rarity => ItemRarity.Rare;
    public override void OnInitalPickup()
    {
        Callbacks.Instance.BulletHitEvent += OnBulletHit;
    }

    public override void Cleanup()
    {
        Callbacks.Instance.BulletHitEvent -= OnBulletHit;
    }
    
    private void OnBulletHit(AbstractDamageArea bullet, AbstractCreature creature)
    {
        if (bullet.Source is Player.Player)
        {
            creature.Knockback(GetHolder().GlobalPosition.DirectionTo(creature.GlobalPosition) * -1000);
        }
    }
    
    
    
}
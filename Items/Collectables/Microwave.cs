using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Powers;

namespace ProjectReaper.Items.Collectables;

public partial class Microwave : AbstractItem
{
    public override string Id => "microwave";
    public override ItemRarity Rarity => ItemRarity.Common;

    public override void OnInitalPickup()
    {
        base.OnInitalPickup();
        Callbacks.Instance.ProjectileHit += OnBulletHit;
    }

    public override void Cleanup()
    {
        base.Cleanup();
        Callbacks.Instance.ProjectileHit -= OnBulletHit;
    }

    private void OnBulletHit(AbstractDamageArea bullet, AbstractCreature target)
    {
        if (bullet.Source == GetHolder() && GameManager.RollProc(.20f, bullet) && target.Team != GetHolder().Team)
        {
            var power = new BurnPower();
            target.AddPower(power);
            power.SetDuration(1);
            power.Damage = GetHolder().Stats.Damage * (0.05f * Stacks);
        }
    }
}
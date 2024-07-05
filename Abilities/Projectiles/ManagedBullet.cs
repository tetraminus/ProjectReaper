namespace ProjectReaper.Abilities.Projectiles;

public partial class ManagedBullet : AbstractDamageArea
{
    public override float Speed { get; set; } = 0f;
    public override float Damage { get; set; } = 0f;
    public override float Duration { get; set; } = -1f;
}
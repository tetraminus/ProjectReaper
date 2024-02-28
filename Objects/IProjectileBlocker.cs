using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;

namespace ProjectReaper.Objects;

public interface IProjectileBlocker
{
	bool CanBlockProjectile(AbstractDamageArea projectile);
	void OnProjectileBlocked(AbstractDamageArea projectile);
	AbstractCreature.Teams Team { get; }
}
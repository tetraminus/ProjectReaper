using Godot;
using ProjectReaper.Abilities;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;

namespace ProjectReaper.Globals;

public partial class Callbacks : Node
{
	
	public static Callbacks Instance { get; private set; }

	public delegate void AbilityUsedEventHandler(AbstractAbility ability, int slot);
	public AbilityUsedEventHandler AbilityUsedEvent; 

	public delegate void BulletCreatedEventHandler(AbstractDamageArea bullet);
	public BulletCreatedEventHandler BulletCreatedEvent;

	public delegate void BulletHitEventHandler(AbstractDamageArea bullet);
	public BulletHitEventHandler BulletHitEvent;

	public delegate void CreatureDamagedEventHandler(AbstractCreature creature, float damage);
	public CreatureDamagedEventHandler CreatureDamagedEvent;

	public delegate void CreatureDiedEventHandler(AbstractCreature creature);
	public CreatureDiedEventHandler CreatureDiedEvent;

	public delegate void CreatureSpawnedEventHandler(AbstractCreature creature);
	public CreatureSpawnedEventHandler CreatureSpawnedEvent;

	public override void _Ready() {
		Instance = this;
	}
}
using Godot;
using ProjectReaper.Abilities;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;

namespace ProjectReaper.Globals;

public partial class Callbacks : Node
{
	
	public static Callbacks Instance { get; private set; }
	
	[Signal]
	public delegate void AbilityUsedEventHandler(AbstractAbility ability, int slot);
	
	[Signal]
	public delegate void BulletCreatedEventHandler(AbstractDamageArea bullet);
	
	[Signal]
	public delegate void BulletHitEventHandler(AbstractDamageArea bullet);
	
	[Signal]
	public delegate void CreatureDamagedEventHandler(AbstractCreature creature, float damage);
	
	[Signal]
	public delegate void CreatureDiedEventHandler(AbstractCreature creature);
	
	[Signal]
	public delegate void CreatureSpawnedEventHandler(AbstractCreature creature);
	
	public override void _Ready() {
		Instance = this;
	}
}
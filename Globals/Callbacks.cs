using Godot;
using ProjectReaper.Abilities;
using ProjectReaper.Abilities.Projectiles;

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
	
	public override void _Ready() {
		Instance = this;
	}
}
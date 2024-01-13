using Godot;
using ProjectReaper.Abilities;
using ProjectReaper.Abilities.Projectiles;

namespace ProjectReaper.Globals;

public partial class Callbacks : Node
{
	// Called when the node enters the scene tree for the first time.
	
	public static Callbacks Instance { get; private set; }
	
	[Signal]
	public delegate void AbilityUsedEventHandler(AbstractAbility ability, int slot);
	
	[Signal]
	public delegate void BulletCreatedEventHandler(AbstractBullet bullet);
	
	public override void _Ready()
	{
		Instance = this;
		GD.Print("Callbacks: _Ready");
	}
}
using Godot;
using ProjectReaper.Abilities;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;

namespace ProjectReaper.Globals;

public partial class Callbacks : Node
{
    [Signal]
    public delegate void AbilityUsedEventHandler(AbstractAbility ability, int slot);
    [Signal]
    public delegate void BulletCreatedEventHandler(AbstractDamageArea bullet);
   
    [Signal]
    public delegate void CreatureDamagedEventHandler(AbstractCreature creature, float damage);
    [Signal]
    public delegate void CreatureDiedEventHandler(AbstractCreature creature);
    [Signal]
    public delegate void CreatureSpawnedEventHandler(AbstractCreature creature);
    
    public delegate float FinalDamageEventHandler(AbstractCreature creature, float damage);
    [Signal]
    public delegate void PlayerDeathEventHandler();
    [Signal]
    public delegate void EnemyRenavEventHandler(Vector2 position, int group);
    [Signal]
    public delegate void ProjectileHitEventHandler(AbstractDamageArea projectile, AbstractCreature creature);
    [Signal]
    public delegate void BossDiedEventHandler();
    [Signal]
    public delegate void ProjectileHitWallEventHandler(AbstractDamageArea bullet);

    
    public FinalDamageEventHandler FinalDamageEvent = (creature, damage) => damage;
    
    public static Callbacks Instance { get; private set; }
    
    public override void _Ready()
    {
        Instance = this;
    }
}
using Godot;
using ProjectReaper.Abilities;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;

namespace ProjectReaper.Globals;

public partial class Callbacks : Node
{
    public delegate void AbilityUsedEventHandler(AbstractAbility ability, int slot);
    public delegate void BulletCreatedEventHandler(AbstractDamageArea bullet);
    public delegate void BulletHitEventHandler(AbstractDamageArea bullet, AbstractCreature creature);
    public delegate void CreatureDamagedEventHandler(AbstractCreature creature, float damage);
    public delegate void CreatureDiedEventHandler(AbstractCreature creature);
    public delegate void CreatureSpawnedEventHandler(AbstractCreature creature);
    public delegate float FinalDamageEventHandler(AbstractCreature creature, float damage);
    public delegate void PlayerDeathEventHandler();
    public delegate void EnemyRenavEventHandler(Vector2 position, int group);
    public delegate void ProjectileHitEventHandler(AbstractDamageArea projectile, AbstractCreature creature);
    public delegate void BossDiedEventHandler();

    public AbilityUsedEventHandler AbilityUsedEvent;
    public BulletCreatedEventHandler BulletCreatedEvent;
    public BulletHitEventHandler BulletHitEvent;
    public CreatureDamagedEventHandler CreatureDamagedEvent;
    public CreatureDiedEventHandler CreatureDiedEvent;
    public CreatureSpawnedEventHandler CreatureSpawnedEvent;
    public FinalDamageEventHandler FinalDamageEvent = (creature, damage) => damage;
    public PlayerDeathEventHandler PlayerDeathEvent;
    public EnemyRenavEventHandler EnemyShouldRenavEvent;
    public ProjectileHitEventHandler ProjectileHitEvent;
    public BossDiedEventHandler BossDiedEvent;

    public static Callbacks Instance { get; private set; }
    


    public override void _Ready()
    {
        Instance = this;
    }
}
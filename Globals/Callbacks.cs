using Godot;
using ProjectReaper.Abilities;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;
using ProjectReaper.Interactables;

namespace ProjectReaper.Globals;

public partial class Callbacks : Node
{
    [Signal]
    public delegate void AbilityUsedEventHandler(AbstractAbility ability, int slot);

    [Signal]
    public delegate void BossDiedEventHandler();

    [Signal]
    public delegate void BulletCreatedEventHandler(AbstractDamageArea bullet);

    [Signal]
    public delegate void CreatureDamagedEventHandler(AbstractCreature creature, float damage);

    [Signal]
    public delegate void CreatureDiedEventHandler(AbstractCreature creature);

    [Signal]
    public delegate void CreatureSpawnedEventHandler(AbstractCreature creature);

    [Signal]
    public delegate void EnemyRenavEventHandler(Vector2 position, int group);

    public delegate float FinalDamageEventHandler(AbstractCreature creature, float damage);

    public delegate float HealEventHandler(AbstractCreature creature, float heal);
    
    public delegate void CalculateStatEventHandler(ref float stat, string statName, AbstractCreature creature);
    public CalculateStatEventHandler CalculateStat;
    
    [Signal]
    public delegate void RecalculateStatsEventHandler();

    [Signal]
    public delegate void LevelLoadedEventHandler();

    [Signal]
    public delegate void PlayerDeathEventHandler();

    [Signal]
    public delegate void ProjectileHitEventHandler(AbstractDamageArea projectile, AbstractCreature creature);

    [Signal]
    public delegate void ProjectileHitWallEventHandler(AbstractDamageArea bullet);
    [Signal]
    public delegate void AbilityTriggerEventHandler(AbstractAbility ability, AbstractCreature creature, int slot, Vector2 position);
    [Signal]
    public delegate void PreChestOpenedEventHandler(Chest chest);
    
    [Signal] 
    public delegate void CollapseStartEventHandler();
    

    public FinalDamageEventHandler FinalDamageEvent = (creature, damage) => damage;

    public HealEventHandler HealEvent = (creature, heal) => heal;
    public static Callbacks Instance { get; private set; }
    


    public override void _Ready()
    {
        Instance = this;
    }
    
}
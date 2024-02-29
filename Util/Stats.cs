using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Util;

public class Stats
{
    public delegate void ValueChangedDelegate(float oldValue, float newValue);

    private float _health;

    private float _maxHealth;
    public ValueChangedDelegate HealthChanged;
    public ValueChangedDelegate MaxHealthChanged;

    public float Health
    {
        get => _health;
        set
        {
            if (value != _health)
            {
                var handler = HealthChanged;
                handler?.Invoke(_health, value);
                _health = value;
            }
        }
    }

    public float MaxHealth
    {
        get => _maxHealth;
        set
        {
            if (value != _maxHealth)
            {
                var handler = MaxHealthChanged;
                handler?.Invoke(_maxHealth, value);
                _maxHealth = value;
            }
        }
    }

    public float Damage { get; set; }
    
    public bool ContactDamage { get; set; }
    public float Defense { get; set; }

    public float Armor { get; set; }

    public float Speed { get; set; }
    public float CritChance { get; set; }
    public float CritDamage { get; set; }
    public float AttackSpeed { get; set; }

    public float Range { get; set; }

    public float AttackDamage { get; set; }

    public float AttackKnockback { get; set; }

    public float AttackStun { get; set; }

    public float AttackDuration { get; set; }

    public float AttackSpread { get; set; }
    public int Luck { get; set; }

    public void Init()
    {
        Health = 100;
        MaxHealth = 100;
        Damage = 10;
        Speed = 100;
        Range = 100;
        Defense = 0;
        Armor = 0;
        CritChance = 0.01f;
        CritDamage = 2f;
        AttackSpeed = 1f;
        AttackDamage = 10f;
        AttackKnockback = 0f;
        AttackStun = 0f;
        AttackDuration = 1f;
        AttackSpread = 0f;
        Luck = 0;
    }
    
    public Stats Initalized()
    {
        var stats = new Stats();
        stats.Init();
        return stats;
    }

    public static float CalculateDamage(float damage, AbstractCreature source, AbstractCreature target,
        Stats sourceStats, Stats targetStats)
    {
        sourceStats ??= new Stats().Initalized();
        targetStats ??= new Stats().Initalized();

        var crit = sourceStats.CritChance;
        var critDamage = sourceStats.CritDamage;
        var defense = targetStats.Defense;
        var armor = targetStats.Armor;
        var damageMultiplier = 1f;
        var critMultiplier = 1f;
        var defenseMultiplier = 1f;
        var armorMultiplier = 1f;
        var finalDamage = 0f;

        if (crit > 0f)
        {
            var roll = GameManager.Randf(0f, 1f, sourceStats.Luck);
            if (roll <= crit) critMultiplier = critDamage;
        }

        if (defense > 0f) defenseMultiplier = 1f - defense;

        if (armor > 0f) armorMultiplier = 1f - armor;

        damageMultiplier = critMultiplier * defenseMultiplier * armorMultiplier;
        finalDamage = damage * damageMultiplier;

        return finalDamage;
    }
}
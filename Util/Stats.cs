using Godot;
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
        CritDamage = 1.2f;
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

    public static DamageReport CalculatedDamageReport(DamageReport report)
    {
        report.calculated = true;
        report.Damage = CalculateDamage(report.Damage, report.Source, report.Target, report.SourceStats, report.TargetStats, report);
        return report;
    }

    public static float CalculateDamage(float damage, AbstractCreature source, AbstractCreature target,
        Stats sourceStats, Stats targetStats, DamageReport report = null)
    {
        sourceStats ??= new Stats().Initalized();
        targetStats ??= new Stats().Initalized();

        var crit = sourceStats.CritChance;
        var critDamage = sourceStats.CritDamage;
        var defense = targetStats.Defense;
        var armor = targetStats.Armor;
        var damageMultiplier = 1f;
       
        var finalDamage = 0f;

        var critMultiplier = 1f;
        int critLevel = 0;
        while (crit > 0f)
        {
            var roll = GD.Randf();
            if (roll <= crit)
            {
                critMultiplier *= critDamage;
                crit -= 1f;
                critLevel++;
            }
            else
            {
                break;
            }
        }

        if (report != null)
        {
            report.critlv = critLevel;
        }
        

     
        damageMultiplier = critMultiplier;
        finalDamage = damage * damageMultiplier;

        return finalDamage;
    }
}
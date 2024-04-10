using System.Collections.Generic;
using Godot;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Util;

public partial class Stats : Node
{
    public delegate void ValueChangedDelegate(float oldValue, float newValue);
    public AbstractCreature Creature;

    private float _damage;

    private float _health;
    
    private float _defense;
    private float _armor;
    private float _speed;
    private float _critChance;
    private float _critDamage;
    private float _attackSpeed;
    private float _range;
    private float _attackDamage;
    private float _attackKnockback;
    private float _attackStun;
    private float _attackDuration;
    private float _attackSpread;

    private float _maxHealth;
    public ValueChangedDelegate HealthChanged;
    public ValueChangedDelegate MaxHealthChanged;
    
    private Dictionary<string, float> _calculatedStats = new ();

    private float CalculateStat(float baseStat, string statName)
    {
        if (_calculatedStats.TryGetValue(statName, out var calculatedStat))
        {
            return calculatedStat;
        }

        calculatedStat = Callbacks.Instance.CalculateStat(baseStat, statName);
        _calculatedStats[statName] = calculatedStat;
        return calculatedStat;
    }

    public Stats()
    {                                               
        Callbacks.Instance.RecalculateStats += RecalculateStats;
    }
    
    public void SetCreature(AbstractCreature creature)                   
    {
        Creature = creature;
        Callbacks.Instance.CreatureDied += (diedcreature) =>
        {
            if (diedcreature == Creature)
            {
                Callbacks.Instance.RecalculateStats -= RecalculateStats;
            }
        };
    }
    
    private void RecalculateStats()
    {
        _calculatedStats.Clear();
    }
    
    


    public float Health
    {
        get => _health;
        set
        {
            var oldValue = _health;
            _health = value;
            HealthChanged?.Invoke(oldValue, value);
        }
    }

    public float MaxHealth
    {
        get => _maxHealth;
        set
        {
            var oldValue = _maxHealth;
            _maxHealth = value;
            MaxHealthChanged?.Invoke(oldValue, value);
            
        }
    }

    public float Damage
    {
        get => CalculateStat(_damage, "Damage");
        set => _damage = value;
    }

    public float Defense
    {
        get => CalculateStat(_defense, "Defense");
        set => _defense = value;
    }

    public float Armor
    {
        get => CalculateStat(_armor, "Armor");
        set => _armor = value;
    }

    public float Speed
    {
        get => CalculateStat(_speed, "Speed");
        set => _speed = value;
    }

    public float CritChance
    {
        get => CalculateStat(_critChance, "CritChance");
        set => _critChance = value;
    }

    public float CritDamage
    {
        get => CalculateStat(_critDamage, "CritDamage");
        set => _critDamage = value;
    }

    public float AttackSpeed
    {
        get => CalculateStat(_attackSpeed, "AttackSpeed");
        set => _attackSpeed = value;
    }

    public float Range
    {
        get => CalculateStat(_range, "Range");
        set => _range = value;
    }

    public float AttackDamage
    {
        get => CalculateStat(_attackDamage, "AttackDamage");
        set => _attackDamage = value;
    }

    public float AttackKnockback
    {
        get => CalculateStat(_attackKnockback, "AttackKnockback");
        set => _attackKnockback = value;
    }

    public float AttackStun
    {
        get => CalculateStat(_attackStun, "AttackStun");
        set => _attackStun = value;
    }

    public float AttackDuration
    {
        get => CalculateStat(_attackDuration, "AttackDuration");
        set => _attackDuration = value;
    }

    public float AttackSpread
    {
        get => CalculateStat(_attackSpread, "AttackSpread");
        set => _attackSpread = value;
    }

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
        report.Damage = CalculateDamage(report.Damage, report.Source, report.Target, report.SourceStats,
            report.TargetStats, report);
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
        var critLevel = 0;
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

        if (report != null) report.critlv = critLevel;


        damageMultiplier = critMultiplier;
        finalDamage = damage * damageMultiplier;

        return finalDamage;
    }
}
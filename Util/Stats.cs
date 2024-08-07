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
    private List<string> _uncacheableStats = new ();

    private float CalculateStat(float baseStat, string statName)
    {
        if (_calculatedStats.TryGetValue(statName, out var calculatedStat))
        {
            return calculatedStat;
        }
        
        float alteredStat = baseStat;
        Callbacks.Instance.CalculateStat?.Invoke(ref alteredStat, statName, Creature);
        calculatedStat = alteredStat;
        if (statName == "MaxHealth")
        {
            calculatedStat = Mathf.Clamp(calculatedStat, 1, Mathf.Inf);
            MaxHealthChanged?.Invoke(_maxHealth, calculatedStat);
        }
        if (statName == "Health")
        {
            calculatedStat = Mathf.Clamp(calculatedStat, 0, MaxHealth);
            HealthChanged?.Invoke(_health, calculatedStat);
        }
        
        if (!_uncacheableStats.Contains(statName))
        {
            _calculatedStats[statName] = calculatedStat;
        }
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
        MaxHealthChanged?.Invoke(_maxHealth, MaxHealth);
        HealthChanged?.Invoke(_health, Health);
    }

    public float Health
    {
        get => CalculateStat(_health, "Health");
        set
        {
            var oldHealth = _health;
            _health = value;
            RecalculateStats();
            HealthChanged?.Invoke(oldHealth, Health);
        }
    }

    public float MaxHealth
    {
        get
        {
            var maxHealth = CalculateStat(_maxHealth, "MaxHealth");
            return maxHealth;
        }
        set
        {
            var oldMaxHealth = _maxHealth;
            _maxHealth = value;
            RecalculateStats();
            MaxHealthChanged?.Invoke(oldMaxHealth, Health);
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
        Damage = 1f;
        Speed = 100;
        Range = 100;
        Defense = 0;
        Armor = 0;
        CritChance = 0.01f;
        CritDamage = 1.2f;
        AttackSpeed = 1f;
        AttackKnockback = 0f;
        AttackStun = 0f;
        AttackDuration = 1f;
        AttackSpread = 0f;
        Luck = 1;
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
        var damageMultiplier = sourceStats.Damage;

        var finalDamage = 0f;

        var critMultiplier = 1f;
        var critLevel = 0;
        while (crit > 0f)
        {
            var roll = GameManager.RollFloat(sourceStats.Luck);
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


        damageMultiplier *= critMultiplier;
        finalDamage = damage * damageMultiplier;

        return finalDamage;
    }

    public void MakeUncacheable(string name)
    {
        if (_calculatedStats.ContainsKey(name))
        {
            _calculatedStats.Remove(name);
        }
        if (!_uncacheableStats.Contains(name))
        {
            _uncacheableStats.Add(name);
        }
    }

    public void MakeCacheable(string damage)
    {
        if (_uncacheableStats.Contains(damage))
        {
            _uncacheableStats.Remove(damage);
        }
    }
}
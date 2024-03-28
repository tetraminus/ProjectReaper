using System.Diagnostics.CodeAnalysis;
using Godot;
using ProjectReaper.Enemies;

namespace ProjectReaper.Util;

public partial class DamageReport : GodotObject
{
    public DamageReport(float damage, AbstractCreature source, AbstractCreature target, Stats sourceStats = null,
        Stats targetStats = null)
    {
        Damage = damage;
        BaseDamage = damage;
        Source = source;
        Target = target;
        SourceStats = sourceStats;
        TargetStats = targetStats;
    }


    public DamageReport(float damage, AbstractCreature target, Stats targetStats = null, Stats sourceStats = null)
    {
        Damage = damage;
        BaseDamage = damage;
        Target = target;
        TargetStats = targetStats;
    }

    public float Damage { get; set; }
    public float BaseDamage { get; set; }
    public bool calculated = false;
    public int critlv = 0;
    public float finalDamage = -1;


    public AbstractCreature Source { get; set; }

    public AbstractCreature Target { get; set; }

    public Stats SourceStats { get; set; }

    public Stats TargetStats { get; set; }

    public float GetDamage()
    {
        return Damage;
    }

    public void ChangeDamage(float damage)
    {
        Damage = damage;
    }
    
    
}
using System.Diagnostics.CodeAnalysis;
using Godot;

namespace ProjectReaper.Util; 

public partial class DamageReport : GodotObject {
    
    public float Damage { get; set; }
    public float BaseDamage { get; set; }
    
    
    [AllowNull] public Node2D Source { get; set; }
    
    public Node2D Target { get; set; }
    
    public DamageReport(float damage, Node2D source, Node2D target)
    {
        Damage = damage;
        BaseDamage = damage;
        Source = source;
        Target = target;
    }
    
    public DamageReport(float damage, Node2D target)
    {
        Damage = damage;
        BaseDamage = damage;
        Target = target;
    }
    
    public float GetDamage()
    {
        return Damage;
    }
    public void ChangeDamage(float damage)
    {
        Damage = damage;
    }
    
}
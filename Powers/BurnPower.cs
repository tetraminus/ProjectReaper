using Godot;
using ProjectReaper.Util;

namespace ProjectReaper.Powers;

public partial class BurnPower : AbstractPower
{
    public override string Id => "burn";
    public override float DefaultDuration => 2;
    private Timer _damageTimer;
    public float Damage = 2;
    
    public override void _Ready()
    {
        base._Ready();
        _damageTimer = new Timer
        {
            Autostart = false,
            OneShot = false,
            WaitTime = 0.1f
        };
        AddChild(_damageTimer);
        _damageTimer.Timeout += OnDamageTimerTimeout;
        
    }

    public override void OnApply()
    {
        base.OnApply();
        _damageTimer.Start();
        StartTimer();
    }

    private void OnDamageTimerTimeout()
    {
        Creature.Damage(new DamageReport(Damage, null, Creature,null, Creature.Stats));
    }

    protected override string GetImagePath()
    {
        return "res://Assets/Power/burn-damage.png";
    }
}
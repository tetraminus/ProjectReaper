using System;
using Godot;
using ProjectReaper.Abilities;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Vfx;

namespace ProjectReaper.Items;

public partial class GrapeShot : AbstractItem
{
    
    private int _count = 0; 
    private const float Spread = 0.2f;
    private const int BaseBullets = 3;
    public override string Id => "grapeshot";
    
    private PackedScene _bulletScene = GD.Load<PackedScene>("res://Items/Prefabs/GrapeBullet.tscn");
    public override void OnInitalPickup()
    {
        Callbacks.Instance.AbilityUsedEvent += OnAbilityUsed;
    }
    
    public void OnAbilityUsed(AbstractAbility ability, int slot)
    {
        _count++;
        if (slot == 1 && _count >= 3)
        {
            FireGrapeshot();
            _count = 0;
        }
    }

    private void FireGrapeshot()
    {
        var bullets = BaseBullets + Stacks - 1;
        
        // fire bullets in a spread
        for (var i = 0; i < bullets; i++)
        {
            var bullet = _bulletScene.Instantiate<BasicBullet>();
            bullet.Damage = 4;
            bullet.Speed = 500;
            bullet.Range = 200;
            var angle = GetHolder().AimDirection() + (i - bullets / 2) * Spread;
            bullet.Init(GetHolder(), GetHolder().Team, GetHolder().GlobalPosition, angle);
            GameManager.Level.AddChild(bullet);
        }
    }
}
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
    private float _spreaddeg = 15;
    private const float MinSpreadDeg = 3;
    private const int BaseBullets = 2;
    public override string Id => "grapeshot";
    public override ItemRarity Rarity => ItemRarity.Uncommon;
    
    private PackedScene _bulletScene = GD.Load<PackedScene>("res://Items/Prefabs/GrapeBullet.tscn");
    public override void OnInitalPickup()
    {
        Callbacks.Instance.AbilityUsed += OnAbilityUsed;
    }
    public override void Cleanup()
    {
        
        Callbacks.Instance.AbilityUsed -= OnAbilityUsed;
    }

    public override void OnStack(int newstacks)
    {
        // reduce spread
        _spreaddeg = Mathf.Max(MinSpreadDeg, _spreaddeg - newstacks );
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
            var angle = GetHolder().AimDirection() + Mathf.DegToRad(_spreaddeg * i - _spreaddeg * (bullets - 1) / 2);
            bullet.Init(GetHolder(), GetHolder().Team, GetHolder().GlobalPosition, angle);
            bullet.ProcCoef = 0.5f;
            GameManager.Level.AddChild(bullet);
            
        }
    }
}
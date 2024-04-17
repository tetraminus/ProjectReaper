using Godot;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Globals;
using ProjectReaper.Player;

namespace ProjectReaper.Abilities;

public partial class SoldierWhack : AbstractAbility
{
    public override void _Ready()
    {
        base._Ready();
        AttackSpeedEffectsCooldown = true;
    }

    private static PackedScene WhackScene { get; } = GD.Load<PackedScene>("res://Abilities/Whack/WhackScene.tscn");

    protected override float Cooldown { get; set; } = 1f;
	

    public override void Use()
    {
        
        AudioManager.Instance.PlaySoundVaried("player", "shoot", GD.RandRange(0.4,0.6));
        var bullet = WhackScene.Instantiate<Node2D>();
        
        var src = GameManager.Player;
        var offset = Vector2.FromAngle(src.AimDirection());  
        
        bullet.Position = offset * 20;
        bullet.GlobalRotation = src.AimDirection();
        bullet.GetNode<MeleeArea>("MeleeArea").Init(src, src.Team, src.AimDirection());
        Callbacks.Instance.EmitSignal(Callbacks.SignalName.AbilityTrigger, this, Creature, (int) AbilityManager.AbilitySlot.Secondary, src.GlobalPosition + offset * 20);
        GameManager.Player.AddChild(bullet);
		
    }
    
}
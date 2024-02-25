using ProjectReaper.Enemies;

namespace ProjectReaper.Abilities;

public partial class SoldierRoll : AbstractAbility
{
    public override float Cooldown { get; set; } = 1f;
    
    public override void Use()
    {
        if (Owner is Player.Player player)
        {
            var direction = player.MoveDirection;
            
            
            player.Velocity = direction.Normalized() * 1500;
            player.HitState = AbstractCreature.HitBoxState.Invincible;
            // remove layer 3
            player.SetCollisionMaskValue(3, false);
            GetTree().CreateTimer(0.3).Timeout += () =>
            {
                player.HitState = AbstractCreature.HitBoxState.Normal;
                    
                // add layer 3
                player.SetCollisionMaskValue(3, true);
            };
        }
    }
}
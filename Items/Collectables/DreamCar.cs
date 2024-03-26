using ProjectReaper.Globals;
using ProjectReaper.Player;

namespace ProjectReaper.Items.Collectables;

public partial class DreamCar : AbstractItem
{
    public override string Id => "dream_car";
    public override ItemRarity Rarity => ItemRarity.Uncommon;
    private const float CooldownReduction = 0.1f; // 10% per stack
    private float currentReduction = 0f;
    public override void OnInitalPickup()
    {
        base.OnInitalPickup();
        var originalCooldown = GameManager.Player.GetAbility(AbilityManager.AbilitySlot.Utility).Cooldown;
        currentReduction = originalCooldown * CooldownReduction * Stacks;
        GameManager.Player.GetAbility(AbilityManager.AbilitySlot.Utility).Cooldown -= currentReduction;
    }

    public override void OnStack(int newstacks) {
        base.OnStack(newstacks);
        GameManager.Player.GetAbility(AbilityManager.AbilitySlot.Utility).Cooldown += currentReduction;
        var originalCooldown = GameManager.Player.GetAbility(AbilityManager.AbilitySlot.Utility).Cooldown;
        currentReduction = originalCooldown * CooldownReduction * Stacks;
        GameManager.Player.GetAbility(AbilityManager.AbilitySlot.Utility).Cooldown -= currentReduction;
    }
}
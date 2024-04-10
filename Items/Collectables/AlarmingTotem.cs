using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Powers;

namespace ProjectReaper.Items.Collectables;

/// <summary>
///     20% speed increase per stack on kill
/// </summary>
public partial class AlarmingTotem : AbstractItem
{
    public override string Id => "alarming_totem";
    public override ItemRarity Rarity => ItemRarity.Uncommon;


    public override void OnInitalPickup()
    {
        Callbacks.Instance.CreatureDied += OnCreatureDied;
    }


    public override void Cleanup()
    {
        Callbacks.Instance.CreatureDied -= OnCreatureDied;
    }


    public void OnCreatureDied(AbstractCreature creature)
    {
        var powid = AbstractPower.GetId<Speedboost>();
        if (GameManager.Player.HasPower(powid))
        {
            if (GameManager.Player.GetPower(powid).Stacks < 5) GameManager.Player.GetPower(powid).AddStacks(1, true);
        }
        else
        {
            var power = new Speedboost();
            power.SetStacks(1);
            GameManager.Player.AddPower(power);
        }
    }
}
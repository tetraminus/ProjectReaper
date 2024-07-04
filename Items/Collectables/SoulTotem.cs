using System.Collections.Generic;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Powers;

namespace ProjectReaper.Items.Collectables;

/// <summary>
///     15% Damage increase per stack on kill
/// </summary>
public partial class SoulTotem : AbstractItem
{
    public override string Id => "soul_totem";
    public override ItemRarity Rarity => ItemRarity.Uncommon;
    public override List<string> Tags => new List<string> { TotemTag };

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
        var powid = AbstractPower.GetId<StatBoostPower>();
        if (GameManager.Player.HasPower(powid) && GameManager.Player.GetPower<StatBoostPower>(powid).Source == Id)
        {
            GameManager.Player.GetPower<StatBoostPower>(powid).AddStacks(1);
        }
        else
        {
            var power = new StatBoostPower();
            power.SetStacks(1);
            power.Multiplier = 0.15f;
            power.StatName = "Damage";
            power.Source = Id;
            GameManager.Player.AddPower(power);
        }
    }
}
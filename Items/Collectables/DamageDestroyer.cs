using Godot;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Items.Collectables;
[ItemLibrary.IgnoreAutoadd]
public partial class DamageDestroyer : AbstractItem
{
    public override string Id => "damage_destroyer";
    public override Texture2D Icon => GD.Load<Texture2D>("res://Items/Collectables/Icons/boom_stick.png");

    public override void Init()
    {
        Callbacks.Instance.FinalDamageEvent += onFinalDamage;
    }


    public float onFinalDamage(AbstractCreature creature, float damage)
    {
        return damage * 2;
    }
}
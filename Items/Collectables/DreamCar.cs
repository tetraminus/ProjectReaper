using ProjectReaper.Globals;

namespace ProjectReaper.Items.Collectables;

public partial class DreamCar : AbstractItem
{
    public override string Id => "dream_car";
    
    public override void OnInitalPickup()
    {
        GameManager.Player.GetAbility(3).Cooldown = 0;
    }
}
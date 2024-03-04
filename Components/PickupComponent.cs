using Godot;
using ProjectReaper.Globals;

namespace ProjectReaper.Components; 

public partial class PickupComponent : Area2D {

    [Export(PropertyHint.TypeString,"Node2D")] public Node2D Pickup { get; set; }
    private IPickup _pickup;
    public override void _Ready()
    {
        _pickup = Pickup as IPickup;
        BodyEntered += OnBodyEntered;
        
        MouseEntered += () =>
        {
            _pickup.Hover();
        };
        MouseExited += () =>
        {
            _pickup.Unhover();
        };
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Player.Player)
        {
            _pickup.Pickup();
        }
    }

    
    
    
}
public interface IPickup
{
    void Pickup();
    void Hover();
    void Unhover();
    
}
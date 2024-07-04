using Godot;

namespace ProjectReaper.Components;

public partial class PickupComponent : Area2D
{
    private IPickup _pickup;

    [Export]
    public Node2D Pickup { get; set; }

    public override void _Ready()
    {
        _pickup = Pickup as IPickup;
        BodyEntered += OnBodyEntered;

        MouseEntered += () => { _pickup.Hover(); };
        MouseExited += () => { _pickup.Unhover(); };
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Player.Player) _pickup.Pickup();
    }
}

public interface IPickup
{
    void Pickup();
    void Hover();
    void Unhover();
}
using Godot;
using ProjectReaper.Components;
using ProjectReaper.Globals;

namespace ProjectReaper.Objects.BossGate; 

public partial class BossKey : Node2D, IPickup {
    private const string Id = "boss_key";
    public void Pickup() {
        GameManager.Player.AddKey(Id);
    }

    public void Hover() {
        
    }

    public void Unhover() {
        
    }
}
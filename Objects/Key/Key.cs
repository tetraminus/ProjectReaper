using Godot;
using ProjectReaper.Components;
using ProjectReaper.Globals;

namespace ProjectReaper.Objects.Key;

public partial class Key : Node2D, IPickup
{
    public const string BasicKeyId = "key";
        
    public void Pickup()
    {
        GameManager.Player.AddKey(BasicKeyId);
        QueueFree();
    }

    public void Hover()
    {
       
    }

    public void Unhover()
    {
        
    }
}
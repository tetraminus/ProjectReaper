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
    
    public bool checkDropPosition(Vector2 position, Vector2 DropDirection)
    {
        // query for terrain
        var parameters = new PhysicsRayQueryParameters2D();
        parameters.From = position;
        parameters.To = position + DropDirection;
        parameters.CollisionMask = 1;

        var ray = GameManager.Level.GetWorld2D().DirectSpaceState.IntersectRay(parameters);

        return ray.Count == 0;
    }
}
using Godot;

namespace ProjectReaper.Globals;

public partial class Mathutils : Node
{
    // --- framerate independent lerp ---
    public static float FriLerp(float a, float b, float decay, float delta)
    {
        return a + (b - a) * Mathf.Exp(-decay * delta);
    }
    
    
    public static Vector2 FriLerp(Vector2 a, Vector2 b, float decay, float delta)
    {
        return a + (b - a) * Mathf.Exp(-decay * delta);
    }
    
    
    
    
    
}
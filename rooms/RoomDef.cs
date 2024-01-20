using Godot;
using Godot.Collections;

namespace ProjectReaper.Util;

public class RoomDef : GodotObject
{
    public string ID { get; set; }
    
    public Array<bool> Connections { get; set; } = new Array<bool>();
    
    public enum Side {
        Top,
        Bottom,
        Left,
        Right
    }
    
}
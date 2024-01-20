using Godot;
using Godot.Collections;

namespace ProjectReaper.Util;

public class RoomSet : GodotObject
{
    Array<RoomDef> Rooms { get; set; }
    
    public RoomSet(Array<RoomDef> rooms) {
        Rooms = rooms;
    }
    
    
}
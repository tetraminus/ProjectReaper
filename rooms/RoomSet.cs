using System.Linq;
using Godot;
using Godot.Collections;

namespace ProjectReaper.Util;

public partial class RoomSet : GodotObject
{
    public string ID { get; set; }
    public Array<RoomDef> Rooms { get; set; }
    public Array<ConnectionDef> Connections { get; set; }
    
    public RoomSet(Array<RoomDef> rooms, Array<ConnectionDef> connections, string id = null) {
        Rooms = rooms;
        // add rotations to rooms
        foreach (var room in Rooms) {
            if (room.Rotatable) {
                for (var i = 0; i < 3; i++) {
                    var rotatedRoom = new RoomDef(room.ID, room.SideConnections);
                    rotatedRoom.Rotation = i + 1;
                    Rooms.Add(rotatedRoom);
                }
            }
            else
            { // add 3 copies of room if it's not rotatable 
                for (var i = 0; i < 4; i++) {
                    Rooms.Add(room.Copy());
                }
                
            }
        }
        
        Connections = connections;
        ID = id;
    }
    
    public ConnectionDef GetConnectionOfId(int id) {
        return Connections.First(connection => connection.Id == id);
    }
    
    public bool connectionAllowed(int id, int connectionId) {
        return GetConnectionOfId(id).AllowedConnections.Contains(connectionId);
    }
    
    public partial class ConnectionDef : GodotObject
    {
        public int Id;
        public Array<int> AllowedConnections;
    }
    
    
    
}
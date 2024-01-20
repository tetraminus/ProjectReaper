using Godot;
using Godot.Collections;

namespace ProjectReaper.Util;

public partial class RoomSetLoader : Node
{
    public static RoomSetLoader Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }
    
    //example of roomset xml, welcome to hell :)
    /*<rooms>
    <room id="room0">
        <connections>
            <top/>
            <bottom/>
            <left/>
            <right/>
        </connections>
    </room>
    <room id="room1">
        <connections>
            <top/>
            <left/>
            <right/>
        </connections>
    </room>
    <room id="room2">
        <connections>
            <top/>
            <bottom/>
            <left/>
        </connections>
    </room>
    <room id="room3">
        <connections>
            <top/>
            <bottom/>
            <right/>
        </connections>
    </room>
    <room id="room4">
        <connections>
            <bottom/>
            <left/>
            <right/>
        </connections>
    </room>
    
</rooms>*/ 
    
    public RoomSet LoadRoomSet(string name) {
        
        var roomDefs = new Array<RoomDef>();

        var xml = new XmlParser();
        xml.Open("res://rooms/" + name + "/" + name + ".xml");


        while (xml.Read() == Error.Ok) {
            if (xml.GetNodeType() == XmlParser.NodeType.Element) {
                if (xml.GetNodeName() == "room") {
                    var roomDef = new RoomDef();
                    roomDef.ID = xml.GetAttributeValue(0);
                    var connections = new Array<bool>();
                    connections.Resize((int) RoomDef.Side.Right + 1);
                    for (var i = 0; i < connections.Count; i++) {
                        connections[i] = false;
                    }
                    while (xml.Read() == Error.Ok) {
                        if (xml.GetNodeType() == XmlParser.NodeType.Element) {
                            if (xml.GetNodeName() == "connections") {
                                while (xml.Read() == Error.Ok) {
                                    if (xml.GetNodeType() == XmlParser.NodeType.Element) {
                                        if (xml.GetNodeName() == "top") {
                                            connections[(int) RoomDef.Side.Top] = true;
                                        } else if (xml.GetNodeName() == "bottom") {
                                            connections[(int) RoomDef.Side.Bottom] = true;
                                        } else if (xml.GetNodeName() == "left") {
                                            connections[(int) RoomDef.Side.Left] = true;
                                        } else if (xml.GetNodeName() == "right") {
                                            connections[(int) RoomDef.Side.Right] = true;
                                        }
                                    } else if (xml.GetNodeType() == XmlParser.NodeType.ElementEnd) {
                                        if (xml.GetNodeName() == "connections") {
                                            break;
                                        }
                                    }
                                }
                            }
                        } else if (xml.GetNodeType() == XmlParser.NodeType.ElementEnd) {
                            if (xml.GetNodeName() == "room") {
                                break;
                            }
                        }
                    }

                    roomDef.Connections = connections;
                    roomDefs.Add(roomDef);
                }
            }
        }
        
        var roomSet = new RoomSet(roomDefs);
        
        return roomSet;
    }
    
    
    
    
    
}
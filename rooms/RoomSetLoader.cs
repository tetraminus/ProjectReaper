using System;
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
            <top type="1"/>
            <bottom type="1"/>
            <left type="1"/>
            <right type="1"/>
        </connections>
    </room>
    <room id="room1">
        <rotatable/>
        <connections>
            <top type="1"/>
            <bottom type="0"/>
            <left type="1"/>
            <right type="1"/>
        </connections>
    </room>
    <room id="room5">
        <connections>
            <top type="0"/>
            <bottom type="0"/>
            <left type="0"/>
            <right type="0"/>
        </connections>
    </room>

    <relations>
        <relation id="0" allowed="0"/>
        <relation id="1" allowed="1"/>
    </relations>
</rooms>*/

    public static RoomSet LoadRoomSet(string name)
    {
        var roomDefs = new Array<RoomDef>();
        var relations = new Array<RoomSet.ConnectionDef>();

        var xml = new XmlParser();
        xml.Open("res://rooms/" + name + "/" + name + ".xml");


        while (xml.Read() == Error.Ok)
            if (xml.GetNodeType() == XmlParser.NodeType.Element)
            {
                if (xml.GetNodeName() == "room")
                {
                    var roomDef = new RoomDef();
                    roomDef.ID = xml.GetAttributeValue(0);
                    roomDefs.Add(roomDef);
                }

                if (xml.GetNodeName() == "rotatable")
                {
                    var roomDef = roomDefs[roomDefs.Count - 1];
                    roomDef.Rotatable = true;
                }

                if (xml.GetNodeName() == "connections")
                {
                    var roomDef = roomDefs[roomDefs.Count - 1];
                    var connections = new Dictionary<RoomDef.Side, int>();

                    while (xml.Read() == Error.Ok)
                    {
                        if (xml.GetNodeType() == XmlParser.NodeType.Element)
                        {
                            var side = xml.GetNodeName();
                            var type = int.Parse(xml.GetAttributeValue(0));
                            connections[side switch
                            {
                                "top" => RoomDef.Side.Top,
                                "right" => RoomDef.Side.Right,
                                "bottom" => RoomDef.Side.Bottom,
                                "left" => RoomDef.Side.Left,
                                _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
                            }] = type;
                        }

                        if (xml.GetNodeType() == XmlParser.NodeType.ElementEnd)
                            if (xml.GetNodeName() == "connections")
                            {
                                roomDef.SideConnections = connections;

                                break;
                            }
                    }
                }


                if (xml.GetNodeName() == "relations")
                    while (xml.Read() == Error.Ok)
                    {
                        if (xml.GetNodeType() == XmlParser.NodeType.Element)
                            if (xml.GetNodeName() == "relation")
                            {
                                var connectionDef = new RoomSet.ConnectionDef();
                                connectionDef.Id = int.Parse(xml.GetAttributeValue(0));
                                connectionDef.AllowedConnections = new Array<int>();
                                var allowedConnections = xml.GetAttributeValue(1).Split(",");

                                foreach (var allowedConnection in allowedConnections)
                                    connectionDef.AllowedConnections.Add(int.Parse(allowedConnection));

                                relations.Add(connectionDef);
                            }

                        if (xml.GetNodeType() == XmlParser.NodeType.ElementEnd)
                            if (xml.GetNodeName() == "relations")
                                break;
                    }
            }

        var roomSet = new RoomSet(roomDefs, relations, name);

        return roomSet;
    }
}
using Godot;
using Godot.Collections;

namespace ProjectReaper.Util;

public partial class TileWeightLoader : Node
{
    public static TileWeightLoader Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }
    
    //example of tileset xml, welcome to hell :)
    /*<tileset name="TestSet" >
           <tile id="Ground">
               <Weights>
                   <Weight id="Ground" value="0.8" />
                   <Weight id="Wall" value="0.5" />
               </Weights>
           </tile>
           <tile id="Wall">
               <Weights>
                   <Weight id="Ground" value="0.5" />
                   <Weight id="Wall" value="0.8" />
               </Weights>
           </tile>
       </tileset>*/ 

    public static TilesetWeights GetTilesetWeights(string tilesetName)
    {
       
        var dict = new Dictionary();
        var parser = new XmlParser();
        parser.Open($"res://Tilesets/{tilesetName}.xml");

        while (parser.Read() == Error.Ok)
        {
            if (parser.GetNodeType() == XmlParser.NodeType.Element)
            {
                if (parser.GetNodeName() == "tile")
                {
                    var tileId = parser.GetAttributeValue(0);
                    var tileDict = new Dictionary();
                    while (parser.Read() == Error.Ok)
                    {
                        if (parser.GetNodeType() == XmlParser.NodeType.Element)
                        {
                            if (parser.GetNodeName() == "Weights")
                            {
                                while (parser.Read() == Error.Ok)
                                {
                                    if (parser.GetNodeType() == XmlParser.NodeType.Element)
                                    {
                                        if (parser.GetNodeName() == "Weight")
                                        {
                                            var weightId = parser.GetAttributeValue(0);
                                            var weightValue = double.Parse(parser.GetAttributeValue(1));
                                            tileDict[weightId] = weightValue;
                                        }
                                    }
                                    else if (parser.GetNodeType() == XmlParser.NodeType.ElementEnd)
                                    {
                                        if (parser.GetNodeName() == "Weights")
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else if (parser.GetNodeType() == XmlParser.NodeType.ElementEnd)
                        {
                            if (parser.GetNodeName() == "tile")
                            {
                                break;
                            }
                        }
                    }
                    dict[tileId] = tileDict;
                }
            }
        }
        
        GD.Print(dict);
        
        var weights = new TilesetWeights(dict);
        return weights;
    }
    
}
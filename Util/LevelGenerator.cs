using System.Xml;
using Godot;
using Godot.Collections;

namespace ProjectReaper.Util; 

public partial class LevelGenerator : Node {
	
	// Wave Function Collapse level generator
	
	public static LevelGenerator Instance { get; private set; }
	
	public override void _Ready() {
		Instance = this;
	}
	
	private float SimilarityWeight = 0.8f;
	private State[,] _states;
	private Dictionary<string, Weights> _weights;

	public void GenerateLevel(TileMap tileMap, TileSet set, int width, int height, ulong seed) {
		var random = new RandomNumberGenerator();
		random.Seed = seed;
		// clear map
		tileMap.Clear();

		// get ground tile
		var groundTile = set.GetSource(0);
		var wallTile = set.GetSource(1);
		_states = new State[width, height];
		
		
		//Collapse();
	}
	
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
	   </tileset>*/ //example of tileset xml

	public override void _EnterTree() {
		var parser = new XmlParser();
		parser.Open("res://Tilesets/TestSet.xml");
		while (parser.Read() != Error.FileEof) {
			if (parser.GetNodeType() == XmlParser.NodeType.Element;
			
		}
	}

	private partial class Weights : GodotObject {
		public float SimilarityWeight { get; set; }
		public Dictionary TileWeights { get; set; }
		
		public Weights(string id, Dictionary tileWeights)  {
			TileWeights = new Dictionary();
			foreach (var tileWeight in tileWeights) {
				TileWeights[tileWeight.Key] = tileWeight.Value;
			}
		}
		
	}
	
	
	
	private enum State {
		Empty,
		Ground,
		Wall
	}
}

using System.Linq;
using Godot;
using Godot.Collections;

namespace ProjectReaper.Util;

public partial class TilesetWeights: GodotObject
{
    public Dictionary<string, Weights> TileWeights { get; set; }
    
    public TilesetWeights(Dictionary tileWeights)  {
        TileWeights = new Dictionary<string, Weights>();
        foreach (var tileWeight in tileWeights) {
            TileWeights[tileWeight.Key.AsString()] = new Weights(tileWeight.Key.AsString(), tileWeight.Value.AsGodotDictionary());
        }
    }
    
    public Weights GetWeights(string id) {
        return TileWeights[id];
    }
    
    public string[] GetTileIds() {
        return TileWeights.Keys.ToArray();
    }
    
    
    
    
    public partial class Weights : GodotObject {
        public double SimilarityWeight { get; set; }
        public Dictionary<string, double> TileWeights { get; set; }
        
        public Weights(string id, Dictionary tileWeights)  {
            TileWeights = new Dictionary<string, double>();
            foreach (var tileWeight in tileWeights) {
                TileWeights[tileWeight.Key.AsString()] = tileWeight.Value.AsDouble();
            }
        }
        
        public double GetWeight(string id) {
            return (float) TileWeights[id];
        }
        
        public double GetSelfWeight() {
            return SimilarityWeight;
        }
        
    }
    
}
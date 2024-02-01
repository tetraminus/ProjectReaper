using Godot;

namespace ProjectReaper.Enemies; 

public class EnemySpawnCard : GodotObject{
    
    public PackedScene Enemy;
    public string id;
    public int Cost;
    
    public EnemySpawnCard(PackedScene enemy, string id, int cost) {
        Enemy = enemy;
        this.id = id;
        Cost = cost;
    }
     
    public int GetCost() {
        return Cost;
    }
    
    public string GetId() {
        return id;
    }
    
    public PackedScene GetEnemy() {
        return Enemy;
    }
    
}
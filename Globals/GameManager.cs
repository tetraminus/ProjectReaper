using Godot;
using ProjectReaper.Enemies;
using ProjectReaper.Player;

namespace ProjectReaper.Globals;

public partial class GameManager : Node
{
    public static PackedScene ExplosionScene =
        ResourceLoader.Load<PackedScene>("res://Abilities/Projectiles/Explosion.tscn");

    // Called when the node enters the scene tree for the first time.
    public static Player.Player Player { get; set; }
    public static PlayerHud PlayerHud { get; set; }
    public static Level Level { get; set; }

    public static bool RandomBool(int luck)
    {
        for (var i = 0; i < luck + 1; i++)
            if (GD.RandRange(0, 1) == 1)
                return true;
        return false;
    }

    public static float Randf(float min, float max, int luck)
    {
        float result = 0;
        for (var i = 0; i < luck + 1; i++)
        {
            var rand = GD.Randf();
            rand = rand * (max - min) + min;

            if (rand > result) result = rand;
        }

        return result;
    }

    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public static void SpawnExplosion(Vector2 globalPosition, int damage, float scale = 1f,
        AbstractCreature creature = null)
    {
        var explosion = (Explosion)ExplosionScene.Instantiate();
        explosion.GlobalPosition = globalPosition;
        explosion.Scale = new Vector2(scale, scale);
        explosion.setDamage(damage);
        explosion.Source = creature ?? Player;
        Level.CallDeferred("add_child", explosion);
    }

    public static void GameOver()
    {
        
    }
    
}
using Godot;
using Godot.Collections;

namespace ProjectReaper.Globals;

public partial class LevelLoader : Node
{
    private const string LevelPath = "res://Levels/";

    public Dictionary<int, Array<PackedScene>> LevelScenes = new();

    public static LevelLoader Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
        LoadLevelScenes();
        PrintVariantScenes();
    }

    private void LoadLevelScenes()
    {
        for (var levelIndex = 1;; levelIndex++)
        {
            var leveldir = LevelPath + "Level" + levelIndex;
            var levelScenes = new Array<PackedScene>();

            for (var levelVariantIndex = 1;; levelVariantIndex++)
            {
                var levelVariant = leveldir + "/Level" + levelIndex + "-" + levelVariantIndex + ".tscn";

                if (ResourceLoader.Exists(levelVariant) == false)
                    break;

                var levelVariantScene = GD.Load<PackedScene>(levelVariant);
                levelScenes.Add(levelVariantScene);
            }

            if (levelScenes.Count == 0)
                break;

            LevelScenes.Add(levelIndex, levelScenes);
        }
    }

    public PackedScene GetLevelScene(int levelIndex, int variantIndex)
    {
        return LevelScenes[levelIndex][variantIndex];
    }

    public PackedScene GetRandomLevelScene(int levelIndex)
    {
        var levelScenes = LevelScenes[levelIndex];
        var randomIndex = GameManager.LevelRng.RandiRange(0, levelScenes.Count - 1);
        return levelScenes[randomIndex];
    }

    public void PrintVariantScenes()
    {
        foreach (var (levelIndex, levelScenes) in LevelScenes)
            GD.Print("Level " + levelIndex + " has " + levelScenes.Count + " variants");
    }
}
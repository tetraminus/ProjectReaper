using Godot;

namespace ProjectReaper.Globals; 
using HarmonyLib;
public partial class Modloader : Node {
    
    public void LoadMods() {
        
        GD.Print("Project Reaper Modloader Loaded");
        var harmony = new Harmony("us.tetramin.tetraminus.projectreaper");
        GD.Print("Project Reaper Modloader Harmony Loaded, Patching...");
        harmony.PatchAll();
        GD.Print("Project Reaper Modloader: Patched");
        
    }
    
    
    
}
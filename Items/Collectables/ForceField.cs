using System;
using System.Collections.Generic;
using Godot;
using ProjectReaper.Components;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Items.Prefabs;

namespace ProjectReaper.Items.Collectables;

public partial class ForceField : AbstractItem {
    public override string Id => "force_field";
    private PackedScene _forceFieldOrbitalScn = GD.Load<PackedScene>("res://Items/Prefabs/Forcefield.tscn");
    private List<Prefabs.ForcefieldOrbital> _forceFieldOrbitals = new();


    public override void OnInitalPickup()
    {
        AddOrbital();
        
    }

    public override void OnStack(int stacks)
    {
        for (var i = 0; i < stacks; i++)
        {
            AddOrbital();
        }
    }
    
    private void AddOrbital()
    {
        var orbital = _forceFieldOrbitalScn.Instantiate<Prefabs.ForcefieldOrbital>();
        orbital.GlobalPosition = GlobalPosition;
        GetHolder().CallDeferred("add_child", orbital);
        _forceFieldOrbitals.Add(orbital);
        SetOrbitalOwner(orbital);
        
        // evenlly distribute the orbitals rotation
        for (var i = 0; i < _forceFieldOrbitals.Count; i++)
        {
            _forceFieldOrbitals[i].SetRotation(Mathf.Tau / _forceFieldOrbitals.Count * i);
            
        }
        
        
        
    }
    
    private async void SetOrbitalOwner(ForcefieldOrbital orbital)
    {
        await ToSignal(orbital, Node.SignalName.Ready);
        orbital.GetNode<CreatureOwnerComponent>("CreatureOwnerComponent").SetCreature(GetHolder());
    }
   
    
}
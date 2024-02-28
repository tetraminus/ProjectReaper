using System;
using System.Collections.Generic;
using Godot;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Items.Collectables;

public partial class ForceField : AbstractItem {
    public override string Id => "force_field";
    private PackedScene _forceFieldOrbitalScn = GD.Load<PackedScene>("res://Items/Prefabs/Forcefield.tscn");
    private List<ForcefieldOrbital> _forceFieldOrbitals = new();


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
        var orbital = _forceFieldOrbitalScn.Instantiate<ForcefieldOrbital>();
        orbital.GlobalPosition = GlobalPosition;
        GetHolder().AddChild(orbital);
        _forceFieldOrbitals.Add(orbital);
        // evenlly distribute the orbitals rotation
        for (var i = 0; i < _forceFieldOrbitals.Count; i++)
        {
            _forceFieldOrbitals[i].SetRotation(Mathf.Tau / _forceFieldOrbitals.Count * i);
            
        }
        
    }
    
}
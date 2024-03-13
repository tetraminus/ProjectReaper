using System;
using Godot;
using GodotStateCharts;
using ProjectReaper.Abilities;
using ProjectReaper.Globals;

namespace ProjectReaper.Enemies;

public partial class Snowpeabert : AbstractCreature
{
    private float _burrowTime = 2.0f; // Time in seconds for how long the character will burrow
    private GpuParticles2D _particles;
    private float _shootangle;
    public AnimatedSprite2D Sprite;
    private StateChart _stateChart;

    private void Burrow()
    {
        
        Sprite.Visible = false; // Make the KinematicBody2D invisible
        HitState = HitBoxState.Spectral; // Make the enemy invincible
        _particles.Emitting = true;
        
        
    }

    private void Unburrow()
    {
        Sprite.Visible = true; // Make the KinematicBody2D visible again
        HitState = HitBoxState.Normal;
        _particles.Emitting = false;
    }

    public override void _Ready()
    {
        _stateChart = StateChart.Of(GetNode("StateChart"));
        _stateChart.SendEvent("StartBurrowing");
        Stats.Init();
        Stats.Health = 20;
        _particles = GetNode<GpuParticles2D>("BurrowParticles");


        // Create and add a timer for state switching

        Sprite = FindChild("Snowboy") as AnimatedSprite2D;
        Sprite.Play();


      

        
    }

    
}
using Godot;
using System;
using Godot.Collections;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;
using ProjectReaper.Util;

public partial class Explosion : AbstractDamageArea

{
	// Called when the node enters the scene tree for the first time.
	private GpuParticles2D _particles1;
	private GpuParticles2D _ringParticle;

	
	private bool _particles1Done = false;

	
	Array<AbstractCreature> _creatures = new Array<AbstractCreature>();
	

	public override void _Ready() {
		_particles1 = GetNode<GpuParticles2D>("Particles1");
		_ringParticle = GetNode<GpuParticles2D>("RingParticle");

		// scale particles according to this node's scale
		ScaleParticles();
		_particles1.Finished += () => { _particles1Done = true; };
	
		
		_particles1.Restart();
		_ringParticle.Restart();
	
		AreaEntered += _areaEntered;
		Timer = new Timer();
		AddChild(Timer);
		Timer.Start(Duration);
		Timer.Timeout += () => {FindChild("CollisionShape2D").SetDeferred("disabled", true);};
		
	}

	private void ScaleParticles() {
		_particles1.ProcessMaterial.Set("initial_velocity",
			(_particles1.ProcessMaterial.Get("initial_velocity").AsVector2()) * Scale);
		_particles1.ProcessMaterial.Set("scale_min",
			(_particles1.ProcessMaterial.Get("scale_min").AsDouble()) * Scale.X);
		_particles1.ProcessMaterial.Set("scale_max",
			(_particles1.ProcessMaterial.Get("scale_max").AsDouble()) * Scale.X);
		
		_ringParticle.ProcessMaterial.Set("scale_min",
			(_ringParticle.ProcessMaterial.Get("scale_min").AsDouble()) * Scale.X);
		_ringParticle.ProcessMaterial.Set("scale_max",
			(_ringParticle.ProcessMaterial.Get("scale_max").AsDouble()) * Scale.X);
	}

	public void setDamage(float damage) {
		Damage = damage;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		if (_particles1Done) {
			CallDeferred("queue_free");
		}
	}
	
	private void _areaEntered (Area2D area) {
		if (area is HurtBox hurtBox && !_creatures.Contains(hurtBox.GetParentCreature())) {
			hurtBox.GetParentCreature().Damage(new DamageReport(10, Source, hurtBox.GetParentCreature(), Source.Stats, hurtBox.GetParentCreature().Stats));
			_creatures.Add(hurtBox.GetParentCreature());
			GD.Print("bonk");
		}
	}
	
	public override float Speed { get; set; } = 0f;
		
	public override float Damage { get; set; }
		
	public override float Duration { get; set; } = 0.1f;
}

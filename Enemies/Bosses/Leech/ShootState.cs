using Godot;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Components;
using ProjectReaper.Globals;


namespace ProjectReaper.Enemies.Bosses.Leech;
	
public partial class ShootState : AbstractState

{
	private Vector2 _shootDirection;
	private bool _shooting;														
	[Export] private float _shootTime;
	private Node2D shootPivot;
	private Area2D LeechBullet;

	public override void _Ready()
	{
		shootPivot = GetNode<Node2D>("%ShootPivot");
		LeechBullet = GetNode<Area2D>("%LeechBullet");
		
		
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

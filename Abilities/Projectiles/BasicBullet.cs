using Godot;

namespace ProjectReaper.Abilities.Projectiles;

public partial class BasicBullet : AbstractBullet
{
        
        private Timer _timer = new Timer();
        public override void _Ready()
        {
                AddChild(_timer);
                _timer.Timeout += () => QueueFree();
                _timer.Start(Duration);
        }

        public override void _Process(double delta)
        {
                //move towards local x
                Translate(Transform.X * Speed * (float) delta);
        }

        public override float Speed { get; set; } = 1000f;
        
        public override float Damage { get; set; } = 10f;
        
        public override float Duration { get; set; } = 1f;
    
    
}
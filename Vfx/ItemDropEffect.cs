using Godot;
using ProjectReaper.Items;

namespace ProjectReaper.Vfx;

public partial class ItemDropEffect : Node2D
{
    private Sprite2D _sprite;
    private PathFollow2D _pathFollow2D;
    private Path2D _path2D;
    private Vector2 _targetPosition;
    public AbstractItem Item { get; set; }
 

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Path2D/PathFollow2D/Sprite2D");
        _pathFollow2D = GetNode<PathFollow2D>("Path2D/PathFollow2D");
        _path2D = GetNode<Path2D>("Path2D");
        
        
    }

    public void StartEffect(Vector2 fromPosition, Vector2 toPosition, AbstractItem item)
    {
        Item = item;
        // smooth curve from the start to the end
        
        _path2D.Curve.ClearPoints();
        _path2D.Curve.AddPoint(fromPosition, Vector2.Down, Vector2.Up * 50);
        
        _path2D.Curve.AddPoint(toPosition, Vector2.Up * 50, Vector2.Down);
        
        
        // smooth curve from the start to the end
        
        
        // tween the sprite to the target position in a curve
        _pathFollow2D.ProgressRatio = 0;
        var tween = GetTree().CreateTween();
        tween.TweenProperty(_pathFollow2D, "progress_ratio", .5, 0.15)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Sine);
        
        tween.TweenProperty(_pathFollow2D, "progress_ratio", 1, 0.35)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Bounce);
            
        tween.Finished += Finish;
        tween.Play();
        
        _targetPosition = toPosition;
        
    }


    private void Finish()
    {
        Item.Spawn(_targetPosition);
        QueueFree();
    }
}
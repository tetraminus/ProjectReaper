using Godot;
using ProjectReaper.Globals;
using ProjectReaper.Player;

public partial class Portal : Node2D
{

    public enum PortalTypes
    {
        NextLevel,
        Shop
    }
    
    private AnimationPlayer _animationPlayer;
    private bool ready;
    public PortalTypes PortalType = PortalTypes.NextLevel;
 
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _animationPlayer.Play("idle");
        GetTree().CreateTimer(1).Timeout += () => ready = true;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void OnBodyEntered(Node body)
    {
        if (body is Player player && ready)
        {
            _animationPlayer.Play("Vanish");
            player.Hide();
            
            if (PortalType == PortalTypes.NextLevel)
            {
                GameManager.GoToNextLevel();
            }
            else if (PortalType == PortalTypes.Shop)
            {
                GameManager.GoToShop();
            }
            
        }
    }
}
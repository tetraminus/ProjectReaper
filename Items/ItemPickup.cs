using Godot;

namespace ProjectReaper.Items;

public partial class ItemPickup : Area2D
{
    
    public AbstractItem Item { get; set; }
    
    private Label _label;
    private Sprite2D _sprite;
    
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        
        _label = GetNode<Label>("Label");
        _sprite = GetNode<Sprite2D>("Sprite");
    }
    
    public void SetItem(AbstractItem item)
    {
        Item = item;
    }
    
    private void OnBodyEntered(Node body)
    {
        if (body is Player.Player player)
        {
            player.AddItem(Item);
            QueueFree();
        }
    }
    
}
using Godot;
using ProjectReaper.Globals;

namespace ProjectReaper.Items;

public partial class ItemPickup : Area2D
{
    public AbstractItem Item { get; set; }
    
    [Export] public bool RollItem = false;
    
   
    private Sprite2D _sprite;
    
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        
        MouseEntered += () =>
        {
            GameManager.PlayerHud.ShowItemInfo(Item);
        };
        
        MouseExited += () =>
        {
            GameManager.PlayerHud.HideItemInfo();
        };
        
        _sprite = GetNode<Sprite2D>("Sprite");
        
        if (RollItem)
        {
            SetItem(Globals.ItemLibrary.Instance.RollItem());
        }
    }

    public override void _ExitTree()
    {
        
        BodyEntered -= OnBodyEntered;
        GameManager.PlayerHud.HideItemInfo();
        base._ExitTree();
        
    }

    public void SetItem(AbstractItem item)
    {
        Item = item;
        _sprite.Texture = item.Icon;
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
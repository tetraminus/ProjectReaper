using Godot;
using ProjectReaper.Globals;

namespace ProjectReaper.Items;

public partial class ItemPickup : Area2D
{
    public AbstractItem Item { get; set; }
    
    /// <summary>
    /// for testing purposes, roll an item on spawn
    /// </summary>
    [Export] public bool RollItem = false;
    
    /// <summary>
    ///   for testing purposes, override the item id on spawn
    /// </summary>
    [Export] public string OverrideItemId = "";
    
   
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
            if (OverrideItemId != "")
            {
                var item = ItemLibrary.Instance.CreateItem(OverrideItemId);
                SetItem(item);
                
            }
            else
            {
                var item = ItemLibrary.Instance.RollItem();
                SetItem(item);
            }
            
        }
       
        
    }

    public override void _ExitTree()
    {
        
        BodyEntered -= OnBodyEntered;
        GameManager.PlayerHud.HideItemInfo();
        
        
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
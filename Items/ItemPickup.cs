using Godot;
using ProjectReaper.Components;
using ProjectReaper.Globals;

namespace ProjectReaper.Items;

public partial class ItemPickup : Node2D, IPickup
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
        GameManager.PlayerHud.HideItemInfo();
    }

    public void SetItem(AbstractItem item)
    {
        Item = item;
        _sprite.Texture = item.Icon;
    }
    

    public void Pickup() {
        if (Item != null)
        {
            GameManager.Player.AddItem(Item);
            QueueFree();
        }
    }

    public void Hover() {
        GameManager.PlayerHud.ShowItemInfo(Item);
    }

    public void Unhover() {
        GameManager.PlayerHud.HideItemInfo();
    }
}
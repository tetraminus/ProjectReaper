using Godot;
using ProjectReaper.Globals;
using ProjectReaper.Player;
using Key = ProjectReaper.Objects.Key.Key;

namespace ProjectReaper.Items.ShopItems;

public partial class ShopItemCollectible : AbstractShopItem
{
     
    public AbstractItem Item;
    public Sprite2D ItemDisplay;
    public RichTextLabel Label;
    public int Cost = 1;
    
    public override bool CheckCost()
    {
        if (GameManager.Player.GetKeyCount(Key.BasicKeyId) >= Cost)
        {
            return true;
        }
        return false;
    }
    
    
    public override void _Ready()
    {
        base._Ready();
        Item = ItemLibrary.Instance.RollItem();
        ItemDisplay = GetNode<Sprite2D>("%Sprite");
        ItemDisplay.Texture = Item.Icon;
        
        Label = GetNode<RichTextLabel>("Label");
        Label.Text = $"{Cost} keys";
        
        
    }
    
    public override void Buy()
    {
        GameManager.Player.UseKey(Key.BasicKeyId, Cost);
        GameManager.Player.AddItem(Item);
        QueueFree();
    }
}
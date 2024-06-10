using ProjectReaper.Globals;

namespace ProjectReaper.Items.ShopItems;

public partial class ShopItemCollectible : AbstractShopItem
{
    
    public AbstractItem Item;
    
    public override void _Ready()
    {
        base._Ready();
        Item = ItemLibrary.Instance.RollItem();
        Sprite2D.Texture = Item.Icon;

        

    }
    
    public override void Buy()
    {
        
        GameManager.Player.AddItem(Item);
        
    }
}
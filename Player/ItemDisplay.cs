using Godot;
using ProjectReaper.Globals;
using ProjectReaper.Items;

namespace ProjectReaper.Player;

public partial class ItemDisplay : Control
{
    public AbstractItem Item { get; set; }
    
    private TextureRect Icon => GetNode<TextureRect>("Icon");
    private Label Stacks => GetNode<Label>("Icon/Stacks");

    public void SetItem(AbstractItem item)
    {
        MouseEntered += () =>
        {
          
            GameManager.PlayerHud.ShowItemInfo(Item);
        };
        
        MouseExited += () =>
        {
            GameManager.PlayerHud.HideItemInfo();
        };
        
        Item = item;
        Icon.Texture = item.Icon;
        Stacks.Text = item.Stacks.ToString();
        Item.StacksChanged += UpdateStacks;
    }
    
    public int UpdateStacks(AbstractItem item, int stacks)
    {
        Stacks.Text = stacks.ToString();
        
        return stacks;
    }
    
    
}
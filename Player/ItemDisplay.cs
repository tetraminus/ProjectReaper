using Godot;
using ProjectReaper.Globals;
using ProjectReaper.Items;
using System;

namespace ProjectReaper.Player;

public partial class ItemDisplay : Control
{
    public AbstractItem Item { get; set; }
    
    private TextureRect Icon => GetNode<TextureRect>("Icon");
    private Label Stacks => GetNode<Label>("Icon/Stacks");

    // Define events
    public event Action<AbstractItem> MouseEnteredItem;
    public event Action MouseExitedItem;

    public void SetItem(AbstractItem item)
    {
        MouseEntered += () =>
        {
            // Raise the event instead of directly calling the method
            MouseEnteredItem?.Invoke(Item);
        };
        
        MouseExited += () =>
        {
            // Raise the event instead of directly calling the method
            MouseExitedItem?.Invoke();
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

    public void HideStacks()
    {
        Stacks.Hide();
    }
    
    public void ShowStacks()
    {
        Stacks.Show();
    }
}
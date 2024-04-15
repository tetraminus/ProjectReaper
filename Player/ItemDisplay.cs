using Godot;
using ProjectReaper.Globals;
using ProjectReaper.Items;
using System;

namespace ProjectReaper.Player;

public partial class ItemDisplay : Control
{
    public AbstractItem Item { get; set; }
    
    private TextureRect Icon => GetNode<TextureRect>("%Icon");
    private Label Stacks => GetNode<Label>("%Icon/Stacks");
    private Label MimicStacks => GetNode<Label>("%Icon/MimicStacks");
    private bool focusable = false;
    
    [Export] public StyleBox FocusStyle { get; set; }
    [Export] public StyleBox NormalStyle { get; set; }

    // Define events
    public event Action<AbstractItem> MouseEnteredItem;
    public event Action MouseExitedItem;
    public event Action<AbstractItem> FocusEnteredItem;
    public event Action FocusExitedItem;


    public override void _Ready()
    {
        MouseEntered += () =>
        {
            // Raise the event instead of directly calling the method
            MouseEnteredItem?.Invoke(Item);
            if (focusable)
            {
                Highlight();
            }
        };
        
        MouseExited += () =>
        {
            // Raise the event instead of directly calling the method
            MouseExitedItem?.Invoke();
            if (focusable)
            {
                Unhighlight();
            }
        };

        FocusEntered += () =>
        {
            // Raise the event instead of directly calling the method
            FocusEnteredItem?.Invoke(Item);
            if (focusable)
            {
                Highlight();
            }
            
        };
        
        FocusExited += () =>
        {
            // Raise the event instead of directly calling the method
            FocusExitedItem?.Invoke();
            if (focusable)
            {
                Unhighlight();
            }
        };
        
        focusable = FocusMode != FocusModeEnum.None;
        
        Unhighlight();
        
    }

    public void SetItem(AbstractItem item)
    {
       
        Item = item;
        Icon.Texture = item.Icon;
        Stacks.Text = item.GetRealStacks().ToString();
        if (item.MimicStacks > 0)
        {
            MimicStacks.Show();
            MimicStacks.Text = "+" + item.MimicStacks;
        }
        else
        {
            MimicStacks.Hide();
        }
        Item.StacksChanged += UpdateStacks;
    }

    public override void _ExitTree()
    {
        Item.StacksChanged -= UpdateStacks;
        base._ExitTree();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (HasFocus() && Input.IsActionJustPressed("ui_accept"))
        {
            ClickedItem?.Invoke(Item);
        }
        
    }

    public void Highlight()
    {
        var panel = GetNode<Panel>("Panel");
        panel.AddThemeStyleboxOverride("panel", FocusStyle);
    }
    
    public void Unhighlight()
    {
        var panel = GetNode<Panel>("Panel");
        panel.AddThemeStyleboxOverride("panel", NormalStyle);
    }

    


    public int UpdateStacks(AbstractItem item, int stacks)
    {
        Stacks.Text = item.GetRealStacks().ToString();
        if (item.MimicStacks > 0)
        {
            MimicStacks.Show();
            MimicStacks.Text = "+" + item.MimicStacks;
        }
        else
        {
            MimicStacks.Hide();
        }
        
        return stacks;
    }

    public void HideStacks()
    {
        Stacks.Hide();
        MimicStacks.Hide();
    }
    
    public void ShowStacks()
    {
        Stacks.Show();
        MimicStacks.Show();
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouseButton)
        {
            if (eventMouseButton.ButtonIndex == MouseButton.Left && eventMouseButton.Pressed)
            {
                ClickedItem?.Invoke(Item);
            }
        }
    }

    public event Action<AbstractItem> ClickedItem;
}
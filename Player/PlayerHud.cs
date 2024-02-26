using System.Globalization;
using Godot;
using ProjectReaper.Globals;
using ProjectReaper.Items;

namespace ProjectReaper.Player;

public partial class PlayerHud : Control
{
    public static PackedScene ItemHudPopupScn => GD.Load<PackedScene>("res://Items/ItemHudPopup.tscn");
    public static PackedScene ItemDisplay => GD.Load<PackedScene>("res://Player/ItemDisplay.tscn");
    
    public ItemHudPopup ItemHudPopup;
    public Label FPS => GetNode<Label>("FPS");
    public override void _Ready()
    {
        GD.Print("PlayerHud ready");
        GameManager.PlayerHud = this;

        ItemHudPopup = ItemHudPopupScn.Instantiate<ItemHudPopup>();
        AddChild(ItemHudPopup);
        
        ItemHudPopup.Visible = false;
        
        

        GetTree().ProcessFrame += OnProcessFrame;
    }

    public void UpdateHealth(float oldHealth, float health)
    {
        GetNode<Label>("Health/Value").Text = health.ToString();
    }

    public override void _Process(double delta)
    {
        FPS.Text = Engine.GetFramesPerSecond().ToString();
        base._Process(delta);
    }

    public void OnProcessFrame()
    {
        if (GameManager.Player != null)
        {
            GameManager.Player.Stats.HealthChanged += UpdateHealth;
            UpdateHealth(0, GameManager.Player.Stats.Health);
        }

        GetTree().ProcessFrame -= OnProcessFrame;
    }
    
    public void AddItem(AbstractItem item)
    {
        var itemDisplay = ItemDisplay.Instantiate<ItemDisplay>();
        itemDisplay.SetItem(item);
        GetNode<GridContainer>("ItemGrid").AddChild(itemDisplay);
    }
    
    public void RemoveItem(AbstractItem item)
    {
        foreach (var child in GetNode<GridContainer>("ItemGrid").GetChildren())
        {
            if (child is ItemDisplay itemDisplay && itemDisplay.Item == item)
            {
                itemDisplay.QueueFree();
                break;
            }
        }
    }

    public void ShowItemInfo(AbstractItem item)
    {
        ItemHudPopup.SetItem(item);
        ItemHudPopup.Visible = true;
        ItemHudPopup.QueueRedraw();
    }
    
    public void HideItemInfo()
    {
        ItemHudPopup.Visible = false;
        ItemHudPopup.QueueRedraw();
       
    }
}


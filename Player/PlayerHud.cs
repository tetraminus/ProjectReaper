using System.Globalization;
using Godot;
using ProjectReaper.Globals;
using ProjectReaper.Items;

namespace ProjectReaper.Player;

public partial class PlayerHud : Control
{
    public static PackedScene ItemHudPopupScn => GD.Load<PackedScene>("res://Items/ItemHudPopup.tscn");
    public static PackedScene ItemDisplay => GD.Load<PackedScene>("res://Player/ItemDisplay.tscn");
    
    public ItemHudPopup InfoHudPopup;
    public Label FPS => GetNode<Label>("FPS");
    public override void _Ready()
    {
        GD.Print("PlayerHud ready");
        GameManager.PlayerHud = this;

        InfoHudPopup = ItemHudPopupScn.Instantiate<ItemHudPopup>();
        AddChild(InfoHudPopup);
        
        InfoHudPopup.Visible = false;
        
        GetNode<Label>("DeathQuote").Hide();
    }
    
    public void ShowDeathQuote()
    {
        var label = GetNode<Label>("DeathQuote");
        label.Show();
        label.VisibleRatio = 0;
        var tween = GetTree().CreateTween();
        tween.TweenProperty(label, "visible_ratio", 1, 1);
        tween.Pause();
        GetTree().CreateTimer(1.5).Timeout += () =>
        {
            tween.Play();
        };
        
    }
    

    public void UpdateHealth(float oldHealth, float health)
    {
        GetNode<TextureProgressBar>("%HealthBar").Value = health;
        GetNode<Label>("%HealthBar/Value").Text = health.ToString();
    }

    public override void _Process(double delta)
    {
        FPS.Text = Engine.GetFramesPerSecond().ToString();
        base._Process(delta);
    }

    public void SetPlayer(Player player)
    {
        player.Stats.HealthChanged += UpdateHealth;
        UpdateHealth(0, player.Stats.Health);
    }
    
    public void AddItem(AbstractItem item)
    {
        var itemDisplay = ItemDisplay.Instantiate<ItemDisplay>();
        itemDisplay.SetItem(item);
        GetNode<HFlowContainer>("%ItemGrid").AddChild(itemDisplay);
    }
    
    public void RemoveItem(AbstractItem item)
    {
        foreach (var child in GetNode<HFlowContainer>("%ItemGrid").GetChildren())
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
        if (InfoHudPopup.IsQueuedForDeletion()) return;
        InfoHudPopup.SetItem(item);
        InfoHudPopup.Show();
        
    }
    
    public void HideItemInfo()
    {
        if (InfoHudPopup.IsQueuedForDeletion()) return;
        InfoHudPopup.Hide();
    }

    public void ShowInfoHud(string infoTitle, string infoText) {
        
        InfoHudPopup.SetInfo(infoTitle, infoText);
        InfoHudPopup.Show();
    }
    
}


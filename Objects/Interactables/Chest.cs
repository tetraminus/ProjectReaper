using System.Collections.Generic;
using Godot;
using Godot.Collections;
using ProjectReaper.Globals;
using ProjectReaper.Items;
using Key = ProjectReaper.Objects.Key.Key;

namespace ProjectReaper.Interactables;

public partial class Chest : Node2D, IInteractable
{
    
    private AnimatedSprite2D AnimatedSprite2D => GetNode<AnimatedSprite2D>("HighlightComponent/Sprite");
    private bool _opened;
    private List<AbstractItem> _items = new List<AbstractItem>();
    private ShaderMaterial _shaderMaterial;
    private const int Items = 2;
    
    
    
    public override void _Ready()
    {

        var rarity = ItemLibrary.Instance.RollRarity();
        for (var i = 0; i < Items; i++)
        {
            AbstractItem item;
            var tries = 0;
            do
            {
                item = ItemLibrary.Instance.RollItem(rarity);
                tries++;
            } while (_items.Contains(item) && tries < 10);
            _items.Add(item);
            
        }
        _opened = false;
        _shaderMaterial = AnimatedSprite2D.Material as ShaderMaterial;
    }
    
    public void Interact()
    {
        if (!_opened)
        {
            _opened = true;
            
            ChooseItemMenu scr = GameManager.GetScreenByName("ChooseItemMenu") as ChooseItemMenu;
            scr?.AddItems(_items);
            GameManager.ShowScreen(scr);
            GameManager.Player.Heal(5);
            
            AnimatedSprite2D.Play("open");
            GameManager.Player.UseKey(Key.BasicKeyId);
        }
    }
    
    

    public bool CanInteract() {
        return !_opened && GameManager.Player.HasKey(Key.BasicKeyId);
    }

    public string GetPrompt(bool Interactable = false)
    {
        if (_opened)
        {
            return "";
        }
        return Interactable ? Tr("ui_open_chest") : Tr("ui_locked_chest") + 1;
    }

    public ItemRarity GetRarity()
    {
        return _items[0].Rarity;
    }
}
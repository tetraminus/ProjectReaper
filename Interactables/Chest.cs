using Godot;
using ProjectReaper.Globals;
using ProjectReaper.Items;

namespace ProjectReaper.Interactables;

public partial class Chest : Node2D
{
    
    private Area2D Area2D => GetNode<Area2D>("Area2D");
    private AnimatedSprite2D AnimatedSprite2D => GetNode<AnimatedSprite2D>("Sprite");
    private bool _playerInRadius;
    private bool _opened;
    private AbstractItem _item;
    private ShaderMaterial _shaderMaterial;
    
    
    
    public override void _Ready()
    {
        Area2D.BodyEntered += OnBodyEntered;
        Area2D.BodyExited += OnBodyExited;
        _item = ItemLibrary.Instance.RollItem();
        _opened = false;
        _shaderMaterial = AnimatedSprite2D.Material as ShaderMaterial;
    }
    
    private void Open()
    {
        if (!_opened)
        {
            _opened = true;
            _item.Drop(GlobalPosition);
            AnimatedSprite2D.Play("open");
        }
        
    }
    
    private void OnBodyEntered(Node body)
    {
        GD.Print(body.Name);
        if (body == GameManager.Player)
        {
            _shaderMaterial.SetShaderParameter("width", 1);
            _playerInRadius = true;
        }
    }
    
    private void OnBodyExited(Node body)
    {
        GD.Print(body.Name);
        if (body == GameManager.Player)
        {
            _shaderMaterial.SetShaderParameter("width", 0);
            _playerInRadius = false;
        }
    }

    public override void _Process(double delta)
    {
        if (_playerInRadius && Input.IsActionJustPressed("interact"))
        {
            Open();
        }
    }
}
using System;
using Godot;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Vfx;

namespace ProjectReaper.Items;

public abstract partial class AbstractItem : Node2D
{
    public static PackedScene ItemPickupScene = GD.Load<PackedScene>("res://Items/ItemPickup.tscn");
    public static PackedScene ItemDropEffectScene = GD.Load<PackedScene>("res://Vfx/ItemDropEffect.tscn");
    public delegate int StackChangeEventHandler(AbstractItem item, int stacks);
    private int _stacks { get; set; }
    public int Stacks
    {
        get => _stacks;
        set
        {
            _stacks = value;
            StacksChanged?.Invoke(this, _stacks);
            
            if (_stacks <= 0)
            {
                QueueFree();
            }
        }
    }
    public event StackChangeEventHandler StacksChanged;
    
    
    public abstract string Id { get; }
    public Texture2D Icon => GD.Load<Texture2D>($"res://Assets/Icons/{Id}.png");

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    public void Gain(int newstacks)
    {
        if (Stacks <= 1)
        {
            OnInitalPickup();
        }
        else
        {
            OnStack(newstacks);
        }
        
    }
    
    public virtual void OnStack(int newstacks)
    {
        
    }

    public virtual void OnInitalPickup() { }


    public int GetStacks()
    {
        return Stacks;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }


    /// <summary>
    ///     Make a copy of the item, used for creating items from the library
    /// </summary>
    /// <returns> A copy of the item </returns>
    public AbstractItem MakeCopy(bool keepStacks = false)
    {
        var copy = (AbstractItem)Duplicate();
        copy.Stacks = keepStacks ? Stacks : 1;
        return copy;
    }
    
    
    public string GetLocalizedName()
    {
        return TranslationServer.Translate(GetNameKey());
    }
    public string GetNameKey()
    {
        return "n_" + Id;
    }
    
    public string GetLocalizedDescription()
    {
        return TranslationServer.Translate(GetDescriptionKey());
    }
    public string GetDescriptionKey()
    {
        return "d_" + Id;
    }
    
    public string GetLocalizedFlavor()
    {
        return TranslationServer.Translate(GetFlavorKey());
    }
    public string GetFlavorKey()
    {
        return "f_" + Id;
    }

    public void Spawn(Vector2 globalPosition)
    {
        var pickup = ItemPickupScene.Instantiate<ItemPickup>();
        pickup.CallDeferred("SetItem", this);
        pickup.GlobalPosition = globalPosition;
        GameManager.Level.AddChild(pickup);
        
    }
    
    public void Drop(Vector2 globalPosition)
    {
        var spawneffect = ItemDropEffectScene.Instantiate<ItemDropEffect>();
        GameManager.Level.AddChild(spawneffect);
        Vector2 direction;
        do
        {
            direction = Vector2.FromAngle((float)(GD.Randf() * Math.Tau));
            direction = direction * (25f); 

        } while (!checkDropPosition(globalPosition, direction));
        
        
        spawneffect.StartEffect(globalPosition, globalPosition + direction, this);
    }
    
    private bool checkDropPosition(Vector2 position, Vector2 DropDirection)
    {
        // query for terrain
        var parameters = new PhysicsRayQueryParameters2D();
        parameters.From = position;
        parameters.To = position + DropDirection;
        parameters.CollisionMask = 1;
        
        var ray = GameManager.Level.GetWorld2D().DirectSpaceState.IntersectRay(parameters);
        
        return ray.Count == 0;
    }
    
    public AbstractCreature GetHolder()
    {
        if (GetParent().GetParent() is AbstractCreature creature)
            return creature;
        return null;
    }
    
}
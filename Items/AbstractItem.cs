using System;
using Godot;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Vfx;

namespace ProjectReaper.Items;

public abstract partial class AbstractItem : Node2D
{
    public delegate int StackChangeEventHandler(AbstractItem item, int stacks);

    public static PackedScene ItemPickupScene = GD.Load<PackedScene>("res://Items/ItemPickup.tscn");
    public static PackedScene ItemDropEffectScene = GD.Load<PackedScene>("res://Vfx/ItemDropEffect.tscn");

    private int _mimicStacks;
    public int MimicStacks
    {
        get { return _mimicStacks; }
        set
        {
            var old_mimicStacks = _mimicStacks;
            _mimicStacks = value;
            var stacksdelta = _mimicStacks - old_mimicStacks;
            OnStack(stacksdelta);
            StacksChanged?.Invoke(this, Stacks);
        }
    }




    private int _stacks;

    public int Stacks
    {
        get => Math.Max(_stacks + MimicStacks, 1);
        private set {}

    }
    
    public void AddStacks(int stacks)
    {
        _stacks += stacks;
        StacksChanged?.Invoke(this, Stacks);

        if (_stacks <= 0) QueueFree();
    }
    
    public int GetRealStacks()
    {
        return _stacks;
    }


    public abstract string Id { get; }

    public Texture2D Icon
    {
        get
        {
            if (!ResourceLoader.Exists($"res://Assets/Icons/{Id}.png"))
                return GD.Load<Texture2D>("res://Assets/Icons/missing.png");
            return GD.Load<Texture2D>($"res://Assets/Icons/{Id}.png");
        }
    }

    public abstract ItemRarity Rarity { get; }
    public event StackChangeEventHandler StacksChanged;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    public void Gain(int newstacks)
    {
        AddStacks(newstacks);
        if (Stacks <= 1)
            OnInitalPickup();
        else
            OnStack(newstacks);
        Callbacks.Instance.EmitSignal(Callbacks.SignalName.RecalculateStats);
    }

    public virtual void OnStack(int newstacks)
    {
    }


    public virtual void OnInitalPickup()
    {
    }
    
    public virtual bool CanSpawn()
    {
        return true;
    }

    public virtual void Cleanup()
    {
    }

    public override void _ExitTree()
    {
        if (GetHolder().IsQueuedForDeletion()) Cleanup();
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
            direction = direction * 25f;
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

    public void SetStacks(int stacks)
    {
        _stacks = stacks;
    }
}
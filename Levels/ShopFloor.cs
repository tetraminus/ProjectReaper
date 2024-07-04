using System;
using System.Collections.Generic;

using Godot;
using Godot.Collections;


namespace ProjectReaper;

public partial class ShopFloor : Level
{
    
    public Array<Node2D> ShopItemPoints = new Array<Node2D>();

    public static List<PackedScene> ShopItemTypes = new List<PackedScene>();

    static ShopFloor()
    {
        //load all shop items
        var folder = "res://Items/ShopItems/";
        
        DirAccess dir = DirAccess.Open(folder);
        dir.ListDirBegin();
        string file = dir.GetNext();
        while (file != "")
        {
            if (file.EndsWith(".tscn"))
            {
                var scene = GD.Load<PackedScene>(folder + file);
                ShopItemTypes.Add(scene);
            }
            file = dir.GetNext();
        }
        
        
            
    }
    
    

    public override void _Ready()
    {
        DisableSpawning = true;
        base._Ready();
        foreach (var node in GetTree().GetNodesInGroup("ShopItemSpawnPoints"))
        {
            if (node is Node2D node2D){

                ShopItemPoints.Add(node2D);
            }
        }
    }

    public override void Generate()
    {
       
        // Generate shop items
        foreach (var shopItempoint in ShopItemPoints)
        {
            var shopItem = GetRandomShopItem();
            
            shopItem.Position = shopItempoint.Position;
            AddChild(shopItem);
            shopItempoint.QueueFree();
            
        }
        
    }
    
    
    public AbstractShopItem GetRandomShopItem()
    {
        var random = new Random();
        var index = random.Next(ShopItemTypes.Count);
        var type = ShopItemTypes[index];
        var shopItem = (AbstractShopItem) type.Instantiate<Node2D>();
        return shopItem;
    }
    
    
    
}
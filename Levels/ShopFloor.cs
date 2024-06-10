using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using ProjectReaper.Items.ShopItems;

namespace ProjectReaper;

public partial class ShopFloor : Level
{
    
    public Array<Node2D> ShopItemPoints = new Array<Node2D>();
    
    public static List<Type> ShopItemTypes = new List<Type>()
    {
        typeof(ShopItemCollectible),
        
    };

    

    public override void _Ready()
    {
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
        var shopItem = (AbstractShopItem) Activator.CreateInstance(type);
        return shopItem;
    }
    
    
    
}
﻿using Godot;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;

namespace ProjectReaper.Components;

public partial class BulletGroup : Node2D
{
    [Export]
    private float speed = 100f;
    
    [Export]
    private float rotationSpeed = 0f;

    

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        
        Translate(Transform.X * speed * (float)delta);
        Rotation += rotationSpeed * (float)delta;
        
        var children = GetChildren();
        foreach (var child in children)
        {
            if (child is not AbstractDamageArea )
            {
                children.Remove(child);
            }
            
        }
        
        if (children.Count == 0)
        {
            QueueFree();
        }
    }
    
    
    public void Init(AbstractCreature source, AbstractCreature.Teams team, Vector2 position, float rotation = 0f)
    {
        GlobalPosition = position;
        
        for (int i = 0; i < GetChildCount(); i++)
        {
            if (GetChild(i) is AbstractDamageArea bullet)
            {
                bullet.Init(source, team);
            }
        }
    }
    
    public void SetDamage(float damage)
    {
        for (int i = 0; i < GetChildCount(); i++)
        {
            if (GetChild(i) is AbstractDamageArea bullet)
            {
                bullet.Damage = damage;
            }
        }
    }
    
}
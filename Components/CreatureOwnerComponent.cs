using Godot;
using ProjectReaper.Enemies;

namespace ProjectReaper.Components;
 
public partial class CreatureOwnerComponent : Node
{
    public AbstractCreature Creature { get; set; }
    
    public void Init(AbstractCreature creature)
    {
        Creature = creature;
    }
    
    public void SetCreature(AbstractCreature creature)
    {
        Creature = creature;
    }
    
    public AbstractCreature GetCreature()
    {
        return Creature;
    }
}
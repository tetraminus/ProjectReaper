using System.Linq;
using Godot;
using Godot.Collections;

namespace ProjectReaper.Enemies;

public partial class Spawnset : GodotObject
{
	private Array<EnemySpawnCard> _enemies = new();

	public void AddEnemy(EnemySpawnCard enemy)
	{
		_enemies.Add(enemy);
	}

	public void RemoveEnemy(EnemySpawnCard enemy)
	{
		_enemies.Remove(enemy);
	}

	public void RemoveEnemy(string id)
	{
		_enemies = new Array<EnemySpawnCard>(_enemies.Where(e => e.id != id));
	}

	public EnemySpawnCard GetEnemy(string id)
	{
		return _enemies.FirstOrDefault(e => e.id == id);
	}

	public Array<EnemySpawnCard> GetAllEnemies()
	{
		return _enemies.Duplicate();
	}

	public EnemySpawnCard GetRandomEnemy()
	{
		return _enemies[GD.RandRange(0, _enemies.Count)];
	}
}

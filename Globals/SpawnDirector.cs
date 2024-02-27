using System.Linq;
using Godot;
using Godot.Collections;
using ProjectReaper.Enemies;

namespace ProjectReaper.Globals;

public partial class SpawnDirector : Node
{
    private const float CreditRate = 1;
    private const float SpawnRate = 1;
    private const float SaveChance = 0.25f;
    private int _credits;
    private Timer _creditsTimer = new();
    private bool _isSpawning;
    private int _lastNumberOfOptions;
    private Spawnset _spawnset;
    private Timer _spawnTimer = new();
    private bool _waiting;

    public static SpawnDirector Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
        AddChild(_creditsTimer);
        _creditsTimer.Timeout += () => { AddCredits((10 + (int)GD.Randi() % 10) * (_waiting ? 1 : 2)); };
        _creditsTimer.WaitTime = CreditRate;
        _creditsTimer.OneShot = false;
        _creditsTimer.Start();

        AddChild(_spawnTimer);
        _spawnTimer.WaitTime = SpawnRate;
        _spawnTimer.Timeout += () =>
        {
            if (_isSpawning) SpawnWave();
            _spawnTimer.Start(SpawnRate + GD.Randf());
        };
        _spawnTimer.Start();
    }

    public void Init(Spawnset spawnset)
    {
        _spawnset = spawnset;
        _credits = 0;
        _isSpawning = false;
    }

    public void StartSpawning()
    {
        _isSpawning = true;
    }

    public void StopSpawning()
    {
        _isSpawning = false;
    }

    private void AddCredits(int amount)
    {
        _credits += amount;
    }

    private void RemoveCredits(int amount)
    {
        _credits -= amount;
    }

    private void SetCredits(int amount)
    {
        _credits = amount;
    }

    public Array<EnemySpawnCard> GetEnemiesToSpawn()
    {
        // get a list of enemies that can be spawned with the current credits
        // prioritize groups of the same enemy

        var availableEnemies = GetAvailableEnemies(_credits);
        // pick a few enemies to spawn
        var creditsLeft = _credits;
        var enemiesToSpawn = new Array<EnemySpawnCard>();
        while (creditsLeft > 0 && availableEnemies.Count > 0)
        {
            // pick the most expensive enemy that can be spawned
            availableEnemies = GetAvailableEnemies(creditsLeft);

            if (availableEnemies.Count == 0) break;

            availableEnemies = new Array<EnemySpawnCard>(availableEnemies.OrderBy(enemy => enemy.Cost));
            var enemy = availableEnemies[availableEnemies.Count - 1];


            if (enemy.Cost <= creditsLeft)
            {
                enemiesToSpawn.Add(enemy);
                creditsLeft -= enemy.Cost;
            }
        }

        _credits = creditsLeft;
        return enemiesToSpawn;
    }

    private Array<EnemySpawnCard> GetAvailableEnemies(int credits)
    {
        var availableEnemies = new Array<EnemySpawnCard>();
        foreach (var enemy in _spawnset.GetAllEnemies())
            if (enemy.Cost <= credits)
                availableEnemies.Add(enemy);
        return availableEnemies;
    }

    private void SpawnWave()
    {
        GD.Print(_credits);

        if (GD.Randf() < SaveChance && _waiting == false && GetAvailableEnemies(_credits).Count < _spawnset.GetAllEnemies().Count)
        {
            _waiting = true;
            _lastNumberOfOptions = GetAvailableEnemies(_credits).Count;
        }

        if (_waiting)
        {
            if (GetAvailableEnemies(_credits).Count > _lastNumberOfOptions && GD.Randf() < SaveChance)
                _waiting = false;
            else
                return;
        }

        var enemies = GetEnemiesToSpawn();
        foreach (var enemy in enemies) SpawnEnemy(enemy);
    }

    public void SpawnEnemy(EnemySpawnCard enemy)
    {
        if (GameManager.Player.Dead) return;
        var enemyInstance = enemy.GetEnemy().Instantiate<AbstractCreature>();
        
        GameManager.Level.AddChild(enemyInstance);
        var level = GameManager.Level;
        
        // check if the enemy is out of bounds
        Vector2 position;
        do 
        {
            position = GameManager.Level.GetSpawnPosition();
            
        } while (CheckOutOfBounds(position, level, enemyInstance));
        
        enemyInstance.GlobalPosition = position;
    }

    /// <summary>
    /// Check if the enemy is out of bounds
    /// </summary>
    /// <param name="enemyInstanceGlobalPosition"></param>
    /// <param name="level"></param>
    /// <param name="enemyInstance"></param>
    /// <returns> True if the enemy is out of bounds </returns>
    private bool CheckOutOfBounds(Vector2 enemyInstanceGlobalPosition, Level level, AbstractCreature enemyInstance) {
        
        var query = new PhysicsShapeQueryParameters2D();

        query.Shape = enemyInstance.GetCollisionShape().Shape.Duplicate();
        
        query.Transform = new Transform2D(0, enemyInstanceGlobalPosition);
        
        
        var collision = level.GetWorld2D().DirectSpaceState.IntersectShape(query,1);
        return collision.Count != 0;
    }

    private static Vector2 RandomDirection()
    {
        var randomDirection = new Vector2();
        randomDirection.X = (float)GD.RandRange(-1.0f, 1.0f);
        randomDirection.Y = (float)GD.RandRange(-1.0f, 1.0f);
        randomDirection = randomDirection.Normalized();
        randomDirection *= 500;
        return randomDirection;
    }

    private int _currentNavGroup = 1;
    public const int MaxNavGroups = 32;

    public int GetNavGroup()
    {
        _currentNavGroup++;
        _currentNavGroup %= MaxNavGroups;
        return _currentNavGroup;
    }
}
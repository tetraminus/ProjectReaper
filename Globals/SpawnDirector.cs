using System.Collections.Generic;
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
    public const int MaxNavGroups = 16;
    private int _credits;
    private Timer _creditsTimer = new();


    private int _currentNavGroup = 1;
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
        _creditsTimer.Timeout += OnCreditsTimerOnTimeout;
        _creditsTimer.WaitTime = CreditRate;
        _creditsTimer.OneShot = false;


        AddChild(_spawnTimer);
        _spawnTimer.WaitTime = SpawnRate;
        _spawnTimer.Timeout += () =>
        {
            if (_isSpawning) SpawnWave();
            _spawnTimer.Start(SpawnRate + GD.Randf());
        };
    }

    private void OnCreditsTimerOnTimeout()
    {
        var credits = 10;
        credits += +(int)GD.Randi() % (int)(10 * GameManager.GetRunDifficulty());
        credits *= _waiting ? 2 : 1;


        AddCredits(credits);
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
        _creditsTimer.Start();
        _spawnTimer.Start();
    }

    public void StopSpawning()
    {
        _isSpawning = false;
        _creditsTimer.Stop();
        _spawnTimer.Stop();
    }

    internal void AddCredits(int amount)
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
        var availableEnemies = GetAvailableEnemies(_credits);
        var creditsLeft = _credits;
        var enemiesToSpawn = new Array<EnemySpawnCard>();
        while (creditsLeft > 0 && availableEnemies.Count > 0)
        {
            availableEnemies = GetAvailableEnemies(creditsLeft);

            if (availableEnemies.Count == 0) break;

            availableEnemies = new Array<EnemySpawnCard>(availableEnemies.OrderBy(enemy => enemy.Cost));
            var enemy = availableEnemies[^1];

            // Check if the cost of the enemy is less than or equal to the remaining credits
            if (enemy.Cost <= creditsLeft)
            {
                enemiesToSpawn.Add(enemy);
                creditsLeft -= enemy.Cost;
            }
            else
            {
                break;
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
        if (GD.Randf() < SaveChance && _waiting == false &&
            GetAvailableEnemies(_credits).Count < _spawnset.GetAllEnemies().Count)
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

    /// <summary>
    ///     Spawn an enemy
    /// </summary>
    /// <param name="enemy"></param>
    public void SpawnEnemy(EnemySpawnCard enemy)
    {
        if (GameManager.Player.Dead) return;
        var enemyInstance = enemy.GetEnemy().Instantiate<AbstractCreature>();

        GameManager.Level.AddChild(enemyInstance);
        var level = GameManager.Level;

        // check if the enemy is out of bounds
        Vector2 position;
        var attempts = 0;
        do
        {
            position = GameManager.Level.GetSpawnPosition();
            attempts++;
        } while (CheckOutOfBounds(position, level, enemyInstance) && attempts < 10);

        if (attempts >= 10)
        {
            enemyInstance.QueueFree();
            return;
        }

        enemyInstance.AddToGroup("spawnedenemies");
        enemyInstance.NavGroup = GetNavGroup();


        enemyInstance.GlobalPosition = position;
    }

    /// <summary>
    ///     Check if the enemy is out of bounds
    /// </summary>
    /// <param name="position"></param>
    /// <param name="level"></param>
    /// <param name="enemyInstance"></param>
    /// <returns> True if the enemy is out of bounds </returns>
    private bool CheckOutOfBounds(Vector2 position, Level level, AbstractCreature enemyInstance)
    {
        var query = new PhysicsShapeQueryParameters2D();
        query.Shape = enemyInstance.GetCollisionShape().Shape.Duplicate();
        query.Transform = new Transform2D(0, position);


        var collision = level.GetWorld2D().DirectSpaceState.IntersectShape(query, 1);
        return collision.Count != 0;
    }

    public void KillAllEnemies()
    {
        foreach (var enemy in GameManager.Level.GetTree().GetNodesInGroup("spawnedenemies"))
            if (enemy is AbstractCreature creature)
                creature.QuietDie();
    }

    public int GetNavGroup()
    {
        _currentNavGroup++;
        _currentNavGroup %= MaxNavGroups;
        return _currentNavGroup;
    }

    public void Clear()
    {
        _credits = 0;
        _isSpawning = false;
        _waiting = false;
        _creditsTimer.Stop();
        _spawnTimer.Stop();
    }

    public EnemySpawnCard GetRandomSpawn(int credits)
    {
        var availableEnemies = GetAvailableEnemies(credits);
        if (availableEnemies.Count == 0) return null;
        return availableEnemies.PickRandom();
    }

    public void PlaceMiniBosses(int count, Level level)
    {
        List<Vector2> Positions = GetSpawnPoints(level, count);

        foreach (var position in Positions)
        {
            var enemyInstance = level.GetRandomMiniBoss();
            enemyInstance.GlobalPosition = position;
            enemyInstance.AddToGroup("spawnedenemies");
            enemyInstance.AddToGroup("miniboss");
            level.AddChild(enemyInstance);
            GD.Print("Placing miniboss at " + enemyInstance.GlobalPosition);
            
            enemyInstance.NavGroup = GetNavGroup();
            
        }
        
        
    }

    private List<Vector2> GetSpawnPoints(Level level, int count)
    {
        //pick count random spawnrects
        // get random position in each spawnrect
        // return list of positions

        List<SpawnRect> spawnRects = new List<SpawnRect>();
        spawnRects.AddRange(level.GetSpawnRects());
        
        var positions = new List<Vector2>();
        
        for (int i = 0; i < count; i++)
        {
            var spawnRect = spawnRects[(int)(GD.Randi() % spawnRects.Count)];
            var position = spawnRect.GetRandomPosition();
            // make sure the spawnrect cant see the player
            while (level.PointSeesPlayer(position))
            {
                spawnRect = spawnRects[(int)(GD.Randi() % spawnRects.Count)];
                position = spawnRect.GetRandomPosition();
            }
            
            positions.Add(position);
            spawnRects.Remove(spawnRect);
        }
        
        return positions;
        
        
    }
}
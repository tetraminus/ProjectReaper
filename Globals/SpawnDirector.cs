using Godot;
using Godot.Collections;
using ProjectReaper.Enemies;

namespace ProjectReaper.Globals; 

public class SpawnDirector : Node {
    const float CreditRate = 1;
    const float SpawnRate = 1;
    private Spawnset _spawnset;
    private int _credits = 0;
    private bool _isSpawning = false;
    private Timer _creditsTimer = new Timer();
    private Timer _spawnTimer = new Timer();
    
    public override void _Ready() {
        AddChild(_creditsTimer);
        _creditsTimer.Timeout += () => {
            AddCredits(10);
        };
        _creditsTimer.WaitTime = CreditRate;
        _creditsTimer.OneShot = false;
        _creditsTimer.Start();
        
        AddChild(_spawnTimer);
        _spawnTimer.WaitTime = SpawnRate;
        _spawnTimer.Timeout += () => {
            if (_isSpawning) {
                SpawnWave();
            }
            _spawnTimer.Start(SpawnRate + GD.Randf());
        };
        _spawnTimer.Start();
        
    }
    
    public void Init(Spawnset spawnset) {
        _spawnset = spawnset;
        _credits = 0;
        _isSpawning = false;
    }
    
    public void StartSpawning() {
        _isSpawning = true;
    }
    
    public void StopSpawning() {
        _isSpawning = false;
    }
    
    private void AddCredits(int amount) {
        _credits += amount;
    }
    
    private void RemoveCredits(int amount) {
        _credits -= amount;
    }
    
    private void SetCredits(int amount) {
        _credits = amount;
    }
    
    public Array<EnemySpawnCard> GetEnemiesToSpawn() {
        // get a list of enemies that can be spawned with the current credits
        // prioritize groups of the same enemy

        var availableEnemies = GetAvailableEnemies(_credits);
        // pick a few enemies to spawn
        var creditsLeft = _credits;
        var enemiesToSpawn = new Array<EnemySpawnCard>();
        while (creditsLeft > 0 && availableEnemies.Count > 0) {
            var enemy = availableEnemies[GD.RandRange(0, availableEnemies.Count)];
            if (enemy.Cost <= creditsLeft) {
                enemiesToSpawn.Add(enemy);
                creditsLeft -= enemy.Cost;
            }
        }
        
        _credits = creditsLeft;
        return enemiesToSpawn;
    }
    
    private Array<EnemySpawnCard> GetAvailableEnemies(int credits) {
        var availableEnemies = new Array<EnemySpawnCard>();
        foreach (var enemy in _spawnset.GetAllEnemies()) {
            if (enemy.Cost <= credits) {
                availableEnemies.Add(enemy);
            }
        }
        return availableEnemies;
    }
    
    private void SpawnWave() {
        var enemies = GetEnemiesToSpawn();
        foreach (var enemy in enemies) {
            SpawnEnemy(enemy);
        }
    }
    
    public void SpawnEnemy(EnemySpawnCard enemy) {
        var enemyInstance = enemy.GetEnemy().Instantiate<AbstractCreature>();
        Vector2 randomDirection = new Vector2();
        randomDirection.X = (float)GD.RandRange(-1.0f, 1.0f);
        randomDirection.Y = (float)GD.RandRange(-1.0f, 1.0f);
        randomDirection = randomDirection.Normalized();
        randomDirection *= 1000;
        enemyInstance.GlobalPosition = randomDirection + GameManager.Player.GlobalPosition;
        GetTree().Root.AddChild(enemyInstance);
    }
}
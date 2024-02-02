using System.Linq;
using Godot;
using Godot.Collections;
using ProjectReaper.Enemies;

namespace ProjectReaper.Globals; 

public partial class SpawnDirector : Node {
    const float CreditRate = 1;
    const float SpawnRate = 1;
    const float SaveChance = 0.25f;
    private Spawnset _spawnset;
    private int _credits = 0;
    private bool _isSpawning = false;
    private Timer _creditsTimer = new Timer();
    private Timer _spawnTimer = new Timer();
    private bool _waiting = false;
    private int _lastNumberOfOptions = 0;
    
    public static SpawnDirector Instance { get; private set; }
    
    public override void _Ready() {
        Instance = this;
        AddChild(_creditsTimer);
        _creditsTimer.Timeout += () => {
            AddCredits((10 + (int)GD.Randi() % 10) * ((_waiting) ? 1 : 2));
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
            // pick the most expensive enemy that can be spawned
            availableEnemies = GetAvailableEnemies(creditsLeft);
            
            if (availableEnemies.Count == 0) {
                break;
            }
            
            availableEnemies = new Array<EnemySpawnCard>(availableEnemies.OrderBy(enemy => enemy.Cost));
            var enemy = availableEnemies[availableEnemies.Count - 1];
            
            
            
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
        GD.Print(_credits);

        if (GD.Randf() < SaveChance && _waiting == false) {
            _waiting = true;
            _lastNumberOfOptions = GetAvailableEnemies(_credits).Count;
        }
        
        if (_waiting) {
            if (GetAvailableEnemies(_credits).Count > _lastNumberOfOptions && GD.Randf() < SaveChance) {
                
                _waiting = false;
            }
            else {
                return;
            }
        }

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
        randomDirection *= 500;
        enemyInstance.GlobalPosition = randomDirection + GameManager.Player.GlobalPosition;
        GetTree().Root.AddChild(enemyInstance);
    }
}
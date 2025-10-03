using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class FortGenerator : GridMap
{
    [Export(PropertyHint.Range, "0,100, 1")] public int FortWidth = 20;
    [Export(PropertyHint.Range, "0,50, 1")] public int FortDepth = 3;
    [Export(PropertyHint.Range, "0,20, 1")] public int FortMaxHeight = 6;
    [Export(PropertyHint.Range, "0,20, 1")] public int FortMinHeight = 1;
    [Export(PropertyHint.Range, "1,50, 1")] public int CratesPerFrame = 6;
    [Export] public PackedScene CrateScene;
    [Export] public PackedScene EnemyScene;
    public int EnemyCount = GameManager.Instance.InitialEnemyCount;

    public const float CRATE_SIZE = 1.88f;
    private Queue<Vector3> _cratePositions = new Queue<Vector3>();
    private List<Crate> _spawnedCrates = new List<Crate>();
    private bool _isGenerating = false;
    private Random _random;

    public override void _Ready()
    {
        if (CrateScene == null)
        {
            GD.PrintErr("CrateScene is not set in FortGenerator.");
            return;
        }
        if (EnemyScene == null)
        {
            GD.PrintErr("EnemyScene is not set in FortGenerator.");
            return;
        }
        CellSize = new Vector3(CRATE_SIZE, CRATE_SIZE, CRATE_SIZE);
        _random = new Random(GameManager.Instance.randomSeed);
    }
    public async void GenerateFort()
    {
        GameManager.Instance.FortGenerating = true;
        RemoveAllCrates();
        if (_isGenerating) return;
        _isGenerating = true;

        FortWidth = GameManager.Instance.GetMapSizeForRound();
        GameManager.Instance.MapSize = FortWidth * CRATE_SIZE;

        CalculateCratePositions();

        await SpawnCratesAsync();

        SpawnEnemies();

        _isGenerating = false;
        SignalManager.Instance.EmitSignal(nameof(SignalManager.FortGenerated));
    }

    private void CalculateCratePositions()
    {
        _cratePositions.Clear();

        for (int x = 0; x < FortWidth; x++)
        {
            for (int z = 0; z < FortDepth; z++)
            {
                int height = _random.Next(FortMinHeight, FortMaxHeight + 1);
                for (int y = 0; y < height; y++)
                {
                    Vector3 crateSpawnPosition = MapToLocal(new Vector3I(x, y, z)) + GlobalPosition;
                    _cratePositions.Enqueue(crateSpawnPosition);
                }
            }
        }
    }

    private async System.Threading.Tasks.Task SpawnCratesAsync()
    {
        _spawnedCrates.Clear();

        while (_cratePositions.Count > 0)
        {
            for (int i = 0; i < CratesPerFrame && _cratePositions.Count > 0; i++)
            {
                Vector3 position = _cratePositions.Dequeue();
                Crate crateInstance = CrateScene.Instantiate<Crate>();
                AddChild(crateInstance, true);
                crateInstance.GlobalPosition = position;
                _spawnedCrates.Add(crateInstance);
            }

            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        }
    }
    private void SpawnEnemies()
    {
        EnemyCount = GameManager.Instance.CurrentEnemyCount;
        if (_spawnedCrates.Count == 0 || EnemyCount <= 0) return;

        var availableCrates = _spawnedCrates.ToList();
        int enemiesToSpawn = Math.Min(EnemyCount, availableCrates.Count);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int randomIndex = _random.Next(availableCrates.Count);
            Crate selectedCrate = availableCrates[randomIndex];
            availableCrates.RemoveAt(randomIndex);

            var enemyInstance = EnemyScene.Instantiate<Node3D>();
            AddChild(enemyInstance, true);
            enemyInstance.GlobalPosition = selectedCrate.GlobalPosition + Vector3.Up * (CRATE_SIZE / 2);
        }
    }
    
    public void RemoveAllCrates()
    {
        foreach (var crate in _spawnedCrates)
        {
            if (IsInstanceValid(crate))
            {
                crate.QueueFree();
            }
        }
        _spawnedCrates.Clear();
        _cratePositions.Clear();
    }
}

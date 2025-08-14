using Godot;
using System;
using System.Collections.Generic;

public partial class FortGenerator : GridMap
{
    [Export(PropertyHint.Range, "0,100, 1")] public int FortWidth = 20;
    [Export(PropertyHint.Range, "0,50, 1")] public int FortDepth = 10;
    [Export(PropertyHint.Range, "0,20, 1")] public int FortMaxHeight = 5;
    [Export(PropertyHint.Range, "0,20, 1")] public int FortMinHeight = 1;
    [Export] public PackedScene CrateScene;
    [Export(PropertyHint.Range, "1,50, 1")] public int CratesPerFrame = 5;

    public const float CRATE_SIZE = 2.0f;
    private Queue<Vector3> _cratePositions = new Queue<Vector3>();
    private bool _isGenerating = false;
    public override void _Ready()
    {
        if (CrateScene == null)
        {
            GD.PrintErr("CrateScene is not set in FortGenerator.");
            return;
        }
        GameManager.Instance.MapSize = FortWidth * CRATE_SIZE;
        CellSize = new Vector3(CRATE_SIZE, CRATE_SIZE, CRATE_SIZE);
    }
    public async void GenerateFort()
    {
        if (_isGenerating) return;
        _isGenerating = true;
        
        CalculateCratePositions();
        
        await SpawnCratesAsync();
        
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
                int height = GD.RandRange(FortMinHeight, FortMaxHeight);
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
        while (_cratePositions.Count > 0)
        {
            for (int i = 0; i < CratesPerFrame && _cratePositions.Count > 0; i++)
            {
                Vector3 position = _cratePositions.Dequeue();
                Crate crateInstance = CrateScene.Instantiate<Crate>();
                AddChild(crateInstance);
                crateInstance.GlobalPosition = position;
            }
            
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        }
    }
}

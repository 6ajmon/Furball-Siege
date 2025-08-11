using Godot;
using System;
using System.Linq;

public partial class Level : Node3D
{
    private Marker3D _hamsterAnchorPoint;
    [Export(PropertyHint.File, "*.tscn")] public string _hamsterScenePath;

    public override void _Ready()
    {
        _hamsterAnchorPoint = GetTree().GetNodesInGroup("hamsterAnchor").FirstOrDefault() as Marker3D;
        SpawnHamster();
    }

    private void SpawnHamster()
    {
        if (_hamsterAnchorPoint != null)
        {
            if (string.IsNullOrEmpty(_hamsterScenePath))
            {
                GD.PrintErr("Level: Hamster scene path is not set.");
                return;
            }
            PackedScene hamsterScene = GD.Load<PackedScene>(_hamsterScenePath);
            if (hamsterScene != null)
            {
                Hamster hamsterInstance = hamsterScene.Instantiate() as Hamster;
                _hamsterAnchorPoint.AddChild(hamsterInstance);
                hamsterInstance.GlobalPosition = _hamsterAnchorPoint.GlobalPosition;
            }
            else
            {
                GD.PrintErr($"Level: Hamster scene at path '{_hamsterScenePath}' could not be loaded.");
            }
        }
        else
        {
            GD.PrintErr("Level: Hamster anchor point not found in the scene.");
        }
    }
}

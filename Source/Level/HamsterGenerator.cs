using Godot;
using System.Linq;

public partial class HamsterGenerator : Node
{
    [Export(PropertyHint.File, "*.tscn")] public string HamsterScenePath;
    private Marker3D _hamsterAnchorPoint;
    private Hamster _hamsterInstance;

    public Hamster HamsterInstance => _hamsterInstance;

    public override void _Ready()
    {
        _hamsterAnchorPoint = GetTree().GetNodesInGroup("hamsterAnchor").FirstOrDefault() as Marker3D;
    }

    public void SpawnHamster()
    {
        if (_hamsterAnchorPoint != null)
        {
            if (string.IsNullOrEmpty(HamsterScenePath))
            {
                GD.PrintErr("HamsterGenerator: Hamster scene path is not set.");
                return;
            }
            PackedScene hamsterScene = GD.Load<PackedScene>(HamsterScenePath);
            if (hamsterScene != null)
            {
                _hamsterInstance = hamsterScene.Instantiate() as Hamster;
                _hamsterAnchorPoint.AddChild(_hamsterInstance);
                _hamsterInstance.GlobalPosition = _hamsterAnchorPoint.GlobalPosition;
            }
            else
            {
                GD.PrintErr($"HamsterGenerator: Hamster scene at path '{HamsterScenePath}' could not be loaded.");
            }
        }
        else
        {
            GD.PrintErr("HamsterGenerator: Hamster anchor point not found in the scene.");
        }
    }

    public void DetachHamster()
    {
        if (_hamsterAnchorPoint != null && _hamsterInstance != null)
        {
            Vector3 globalPos = _hamsterInstance.GlobalPosition;
            Vector3 globalRot = _hamsterInstance.GlobalRotation;

            _hamsterAnchorPoint.RemoveChild(_hamsterInstance);
            GetParent().AddChild(_hamsterInstance);
            _hamsterInstance.GlobalPosition = globalPos;
            _hamsterInstance.GlobalRotation = globalRot;
            _hamsterInstance.Scale = new Vector3(1, 1, 1);
        }
    }
}
using Godot;
using System;
using System.Linq;

public partial class Level : Node3D
{
    [Export] public Slingshot _slingshot;
    private Marker3D _hamsterAnchorPoint;
    [Export(PropertyHint.File, "*.tscn")] public string _hamsterScenePath;
    private Hamster _hamsterInstance;
    [Export] public FortGenerator FortGenerator;
    private float _distanceFromFort;
    [Export] public float _distanceFromFortMultiplier = 0.7f;

    public override void _Ready()
    {
        _hamsterAnchorPoint = GetTree().GetNodesInGroup("hamsterAnchor").FirstOrDefault() as Marker3D;
        SpawnHamster();
        if (FortGenerator != null)
        {
            SetUpFortGenerator();
        }
        else
        {
            GD.PrintErr("Level: FortGenerator is not set.");
        }
        SignalManager.Instance.FortGenerated += OnFortGenerated;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.Aiming)
        {
            if (Input.IsActionPressed("Shoot"))
            {
                DetachHamster();
                TryShoot();
            }
        }
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
                _hamsterInstance = hamsterScene.Instantiate() as Hamster;
                _hamsterAnchorPoint.AddChild(_hamsterInstance);
                _hamsterInstance.GlobalPosition = _hamsterAnchorPoint.GlobalPosition;
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
    public void DetachHamster()
    {
        if (_hamsterAnchorPoint != null)
        {
            Vector3 globalPos = _hamsterInstance.GlobalPosition;
            Vector3 globalRot = _hamsterInstance.GlobalRotation;

            _hamsterAnchorPoint.RemoveChild(_hamsterInstance);
            AddChild(_hamsterInstance);
            _hamsterInstance.GlobalPosition = globalPos;
            _hamsterInstance.GlobalRotation = globalRot;
            _hamsterInstance.Scale = new Vector3(1, 1, 1);
        }
    }

    public void TryShoot()
    {
        GameManager.Instance.CurrentGameState = GameManager.GameState.Shooting;
        if (_hamsterInstance != null)
        {
            _hamsterInstance.ApplyImpulse(_slingshot.GetLaunchDirection() * _slingshot.Force);
            _hamsterInstance.GravityScale = 0.3f;
            _slingshot.PlayAnimation("SlingshotShoot");
        }
    }

    private void SetUpFortGenerator()
    {
        float FortWidth = FortGenerator.FortWidth;
        float FortDepth = FortGenerator.FortDepth;
        _distanceFromFort = FortWidth * _distanceFromFortMultiplier * FortGenerator.CRATE_SIZE;
        GameManager.Instance.FortDistance = _distanceFromFort;
        FortGenerator.Position = new Vector3(
            -FortWidth / 2f * FortGenerator.CRATE_SIZE,
            0.00001f,
            _slingshot.GlobalPosition.Z - _distanceFromFort - FortDepth * FortGenerator.CRATE_SIZE
        );
        FortGenerator.GenerateFort();
    }

    private void OnFortGenerated()
    {
        GameManager.Instance.CalculateShotsCount();
    }
}

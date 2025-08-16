using Godot;
using System;
using System.Linq;

public partial class Level : Node3D
{
    [Export] public Slingshot _slingshot;
    [Export] public FortGenerator FortGenerator;
    [Export] public HamsterGenerator HamsterGenerator;
    private float _distanceFromFort;
    [Export] public float _distanceFromFortMultiplier = 0.7f;

    public override void _Ready()
    {
        if (HamsterGenerator != null)
        {
            HamsterGenerator.SpawnHamster();
        }
        else
        {
            GD.PrintErr("Level: HamsterGenerator is not set.");
        }

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

    public void DetachHamster()
    {
        if (HamsterGenerator != null)
        {
            HamsterGenerator.DetachHamster();
        }
    }

    public void TryShoot()
    {
        GameManager.Instance.CurrentGameState = GameManager.GameState.Shooting;
        if (HamsterGenerator?.HamsterInstance != null)
        {
            HamsterGenerator.HamsterInstance.ApplyImpulse(_slingshot.GetLaunchDirection() * _slingshot.Force);
            HamsterGenerator.HamsterInstance.GravityScale = 0.3f;
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
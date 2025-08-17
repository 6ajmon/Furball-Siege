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
        SignalManager.Instance.RestartGame += OnRestartGame;
        SignalManager.Instance.NextRound += OnNextRound;
        SignalManager.Instance.Shoot += InitializeShot;

        CameraManager.Instance.RefreshCameraList();
        CameraManager.Instance.ActivateCamera(0);

        GameManager.Instance.ResetGame();
        SignalManager.Instance.EmitSignal(nameof(SignalManager.RoundNumberChanged));
    }

    private async void OnNextRound()
    {
        GameManager.Instance.ResetGame();
        GameManager.Instance.CurrentRound++;
        SignalManager.Instance.EmitSignal(nameof(SignalManager.RoundNumberChanged));
        await ToSignal(GetTree().CreateTimer(2.0f), SceneTreeTimer.SignalName.Timeout);
        HamsterGenerator._canReload = true;
        HamsterGenerator.ReloadHamster();
        SetUpFortGenerator();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.Aiming)
        {
            if (Input.IsActionPressed("Shoot"))
            {
                InitializeShot();
            }
        }
    }
    private void InitializeShot()
    {
        if (GameManager.Instance.HasShotsRemaining)
        {
            DetachHamster();
            TryShoot();
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
        GameManager.Instance.TakeShot();

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
    private void OnRestartGame()
    {
        GameManager.Instance.ResetGame();
        if (IsInstanceValid(this))
        {
            GetTree().CallDeferred(SceneTree.MethodName.ReloadCurrentScene);
        }
    }
}
using Godot;
using System;
using System.Linq;

public partial class Level : Node3D
{
    [Export] public Slingshot _slingshot;
    [Export] public FortGenerator FortGenerator;
    [Export] public HamsterGenerator HamsterGenerator;
    [Export] public PackedScene QuickTimeEvent;
    private float _distanceFromFort;
    [Export] public float _distanceFromFortMultiplier = 0.7f;
    [Export] public float failedEventForce = 0.2f;
    [Export] public NodePath QuickTimeEventPath = "Slingshot/SlingshotCamera/AimingOverlay/MarginContainer/VBoxContainer/HBoxContainer2/MarginContainer/VBoxContainer/HBoxContainer";
    private bool isQTEActive = false;

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
        SignalManager.Instance.FinishReload += OnReloadFinished;

        CameraManager.Instance.OnRestartGame();

        GameManager.Instance.ResetGame();
        SignalManager.Instance.EmitSignal(nameof(SignalManager.RoundNumberChanged));
        SignalManager.Instance.EmitSignal(nameof(SignalManager.UpdateAmmoCount));

        AudioManager.Instance.EmitSignal(nameof(AudioManager.PlayGameMusic));
    }

    private async void OnNextRound()
{
    await ToSignal(GetTree().CreateTimer(2.0f), SceneTreeTimer.SignalName.Timeout);
    
    if (!IsInstanceValid(this))
        return;
    
    SetUpFortGenerator();
}
    private void OnReloadFinished()
    {
        if (IsInstanceValid(HamsterGenerator))
        {
            HamsterGenerator._canReload = true;
            HamsterGenerator.ReloadHamster();
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.Aiming
            && !GameManager.Instance.FortGenerating
            && GameManager.Instance.finishedReloadBar
            && Input.IsActionPressed("Shoot"))
        {
            InitializeShot();
        }
    }
    private void InitializeShot()
    {
        if (GameManager.Instance.HasShotsRemaining && !isQTEActive)
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

    public async void TryShoot()
    {
        isQTEActive = true;
        GameManager.Instance.CurrentGameState = GameManager.GameState.Shooting;
        
        float randomDelay = (float)GD.RandRange(0.0, 1.0);
        await ToSignal(GetTree().CreateTimer(randomDelay), SceneTreeTimer.SignalName.Timeout);
        
        var quickTimeEventInstance = QuickTimeEvent.Instantiate<QuickTimeEvent>();
        GetNode(QuickTimeEventPath).AddChild(quickTimeEventInstance);
        
        var result = await ToSignal(SignalManager.Instance, SignalManager.SignalName.QuickTimeEventCompleted);
        bool qteSuccess = result[0].AsBool();
        
        isQTEActive = false;
        Shoot(qteSuccess);
    }

    public void Shoot(bool highForce = false)
    {
        HamsterGenerator.StartReloadCooldown();
        GameManager.Instance.TakeShot();
        SignalManager.Instance.EmitSignal(nameof(SignalManager.HamsterShot));
        if (HamsterGenerator?.HamsterInstance != null)
        {
            float forceMultiplier = highForce ? 1.0f : failedEventForce;
            HamsterGenerator.HamsterInstance.ApplyImpulse(_slingshot.GetLaunchDirection() * _slingshot.Force * forceMultiplier);
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
        GameManager.Instance.FortGenerating = false;
        GameManager.Instance.CalculateShotsCount();
    }
    private void OnRestartGame()
    {
        SignalManager.Instance.FortGenerated -= OnFortGenerated;
        SignalManager.Instance.RestartGame -= OnRestartGame;
        SignalManager.Instance.NextRound -= OnNextRound;
        SignalManager.Instance.Shoot -= InitializeShot;
        
        GameManager.Instance.ResetGame();
        if (IsInstanceValid(this))
        {
            GetTree().CallDeferred(SceneTree.MethodName.ReloadCurrentScene);
        }
    }
}
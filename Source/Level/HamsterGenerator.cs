using Godot;
using System.Linq;

public partial class HamsterGenerator : Node
{
    [Export(PropertyHint.File, "*.tscn")] public string HamsterScenePath;
    public float ReloadCooldown = GameManager.Instance.ReloadCooldown;
    
    private Marker3D _hamsterAnchorPoint;
    private Hamster _hamsterInstance;
    private Timer _reloadTimer;
    public bool _canReload = false;

    public Hamster HamsterInstance => _hamsterInstance;
    public bool CanReload => _canReload && GameManager.Instance.HasShotsRemaining;

    public override void _Ready()
    {
        _hamsterAnchorPoint = GetTree().GetNodesInGroup("hamsterAnchor").FirstOrDefault() as Marker3D;

        _reloadTimer = new Timer();
        _reloadTimer.WaitTime = ReloadCooldown;
        _reloadTimer.OneShot = true;
        _reloadTimer.Timeout += OnReloadTimerTimeout;
        AddChild(_reloadTimer);
        SignalManager.Instance.ReloadAmmo += TryReloadHamster;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("Reload"))
        {
            TryReloadHamster();
        }
        GameManager.Instance.RemainingReloadCooldown = (float)(_reloadTimer.WaitTime - _reloadTimer.TimeLeft);
        SignalManager.Instance.EmitSignal(nameof(SignalManager.UpdateReloadProgress));
    }
    private void TryReloadHamster()
    {
        if (CanReload)
        {
            ReloadHamster();
        }
    }

    public void SpawnHamster()
    {
        _hamsterAnchorPoint = GetTree().GetNodesInGroup("hamsterAnchor").FirstOrDefault() as Marker3D;
        if (IsInstanceValid(_hamsterAnchorPoint))
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
                _hamsterAnchorPoint.AddChild(_hamsterInstance, true);
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
            AddChild(_hamsterInstance);
            _hamsterInstance.GlobalPosition = globalPos;
            _hamsterInstance.GlobalRotation = globalRot;
            _hamsterInstance.Scale = new Vector3(1, 1, 1);
        }
    }

    public void StartReloadCooldown()
    {
        _canReload = false;
        if (GameManager.Instance.HasShotsRemaining)
        {
            _reloadTimer.Start();
        }
    }

    public void ReloadHamster()
    {
        if (!CanReload) return;

        RemovePreviousHamster();
        
        SpawnHamster();
        
        GameManager.Instance.CurrentGameState = GameManager.GameState.Aiming;
        
        _canReload = false;
    }

    private void RemovePreviousHamster()
    {
        if (IsInstanceValid(_hamsterInstance))
        {
            _hamsterInstance.QueueFree();
            _hamsterInstance = null;
        }
    }

    private void OnReloadTimerTimeout()
    {
        if (GameManager.Instance.HasShotsRemaining)
        {
            _canReload = true;
        }
    }
}
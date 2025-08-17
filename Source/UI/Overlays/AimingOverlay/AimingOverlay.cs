using Godot;
using System;

public partial class AimingOverlay : Control
{
    [Export] public Label RoundNumberLabel;
    [Export] public Label EnemiesRemainingLabel;
    [Export] public Label AmmoLabel;

    [Export] public ProgressBar ReloadProgressBar;

    [Export] public Button ReloadButton;
    [Export] public Button ShootButton;
    [Export] public TextureButton PauseButton;
    [Export] public TextureButton RotateLeftButton;
    [Export] public TextureButton RotateRightButton;
    [Export] public TextureButton RotateDownButton;
    [Export] public TextureButton RotateUpButton;
    [Export] public TextureButton CycleLeftButton;
    [Export] public TextureButton CycleRightButton;

    private Camera3D _parentCamera;

    private bool _isRotatingLeft = false;
    private bool _isRotatingRight = false;
    private bool _isRotatingUp = false;
    private bool _isRotatingDown = false;

    public override void _Ready()
    {
        _parentCamera = GetParent<Camera3D>();
        ReloadProgressBar.MaxValue = GameManager.Instance.ReloadCooldown;
        SignalManager.Instance.RoundNumberChanged += UpdateRoundNumber;
        SignalManager.Instance.UpdateEnemiesRemaining += UpdateEnemiesRemaining;
        SignalManager.Instance.UpdateReloadProgress += UpdateReloadProgress;
        SignalManager.Instance.UpdateAmmoCount += UpdateAmmoCount;
    }

    public override void _ExitTree()
    {
        if (SignalManager.Instance != null)
        {
            SignalManager.Instance.RoundNumberChanged -= UpdateRoundNumber;
            SignalManager.Instance.UpdateEnemiesRemaining -= UpdateEnemiesRemaining;
            SignalManager.Instance.UpdateReloadProgress -= UpdateReloadProgress;
            SignalManager.Instance.UpdateAmmoCount -= UpdateAmmoCount;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_parentCamera.Current)
        {
            Show();
            if (_isRotatingLeft)
                SignalManager.Instance.EmitSignal(nameof(SignalManager.RotateLeft));
            if (_isRotatingRight)
                SignalManager.Instance.EmitSignal(nameof(SignalManager.RotateRight));
            if (_isRotatingUp)
                SignalManager.Instance.EmitSignal(nameof(SignalManager.RotateUp));
            if (_isRotatingDown)
                SignalManager.Instance.EmitSignal(nameof(SignalManager.RotateDown));

            if (GameManager.Instance.CurrentGameState == GameManager.GameState.Aiming)
            {
                ShootButton.Show();
            }
            else
            {
                ShootButton.Hide();
            }
        }
        else
        {
            Hide();
        }
    }
    
    #region Labels
    public void UpdateRoundNumber()
    {
        if (IsInstanceValid(RoundNumberLabel))
        {
            RoundNumberLabel.Text = $"Round: {GameManager.Instance.CurrentRound}";
        }
    }
    public void UpdateEnemiesRemaining()
    {
        if (IsInstanceValid(EnemiesRemainingLabel))
        {
            EnemiesRemainingLabel.Text = $"Gnomes Left: {GameManager.Instance.CurrentEnemyCount}";
        }
    }
    public void UpdateAmmoCount()
    {
        if (IsInstanceValid(AmmoLabel))
        {
            var ammoCount = GameManager.Instance.ShotsCount - GameManager.Instance.ShotsTaken;
            AmmoLabel.Text = $"{ammoCount}/{GameManager.Instance.ShotsCount}";
        }
    }
    public void UpdateReloadProgress()
    {
        if (IsInstanceValid(ReloadProgressBar))
        {
            ReloadProgressBar.Value = GameManager.Instance.RemainingReloadCooldown;
        }
    }
    #endregion Labels

    public void OnPauseButtonPressed()
    {
        ButtonSoundEffect();
        SignalManager.Instance.EmitSignal(nameof(SignalManager.PauseGame));
    }
    public void OnReloadButtonPressed()
    {
        ButtonSoundEffect();
        SignalManager.Instance.EmitSignal(nameof(SignalManager.ReloadAmmo));
    }
    public void OnShootButtonPressed()
    {
        ButtonSoundEffect();
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.Aiming)
        {
            SignalManager.Instance.EmitSignal(nameof(SignalManager.Shoot));
        }
    }
    private void OnRotateLeftButtonDown()
    {
        ButtonSoundEffect();
        _isRotatingLeft = true;
    }
    
    private void OnRotateLeftButtonUp()
    {
        _isRotatingLeft = false;
    }
    
    private void OnRotateRightButtonDown()
    {
        ButtonSoundEffect();
        _isRotatingRight = true;
    }
    
    private void OnRotateRightButtonUp()
    {
        _isRotatingRight = false;
    }
    
    private void OnRotateUpButtonDown()
    {
        ButtonSoundEffect();
        _isRotatingUp = true;
    }
    
    private void OnRotateUpButtonUp()
    {
        _isRotatingUp = false;
    }
    
    private void OnRotateDownButtonDown()
    {
        ButtonSoundEffect();
        _isRotatingDown = true;
    }
    
    private void OnRotateDownButtonUp()
    {
        _isRotatingDown = false;
    }
    public void OnCycleLeftButtonPressed()
    {
        ButtonSoundEffect();
        SignalManager.Instance.EmitSignal(nameof(SignalManager.CycleLeft));
    }
    public void OnCycleRightButtonPressed()
    {
        ButtonSoundEffect();
        SignalManager.Instance.EmitSignal(nameof(SignalManager.CycleRight));
    }
    private void ButtonSoundEffect()
    {
        // TODO: audio
    }
}

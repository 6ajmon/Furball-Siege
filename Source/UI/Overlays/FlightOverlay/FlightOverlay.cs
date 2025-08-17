using Godot;
using System;

public partial class FlightOverlay : Control
{
    [Export] public Label RoundNumberLabel;
    [Export] public Label EnemiesRemainingLabel;
    [Export] public Label AmmoLabel;

    [Export] public TextureButton PauseButton;
    [Export] public TextureButton CycleLeftButton;
    [Export] public TextureButton CycleRightButton;

    private Camera3D _parentCamera;

    public override void _Ready()
    {
        _parentCamera = GetParent<Camera3D>();
        SignalManager.Instance.RoundNumberChanged += UpdateRoundNumber;
        SignalManager.Instance.UpdateEnemiesRemaining += UpdateEnemiesRemaining;
        SignalManager.Instance.UpdateAmmoCount += UpdateAmmoCount;
    }
    public override void _PhysicsProcess(double delta)
    {
        if (_parentCamera.Current)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    
    #region Labels
    public void UpdateRoundNumber()
    {
        RoundNumberLabel.Text = $"Round: {GameManager.Instance.CurrentRound}";
    }
    public void UpdateEnemiesRemaining()
    {
        EnemiesRemainingLabel.Text = $"Gnomes Left: {GameManager.Instance.CurrentEnemyCount}";
    }
    public void UpdateAmmoCount()
    {
        var ammoCount = GameManager.Instance.ShotsCount - GameManager.Instance.ShotsTaken;
        AmmoLabel.Text = $"{ammoCount}/{GameManager.Instance.ShotsCount}";
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

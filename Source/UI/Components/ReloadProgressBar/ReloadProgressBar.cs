using Godot;
using System;

public partial class ReloadProgressBar : ProgressBar
{
    private bool _isBeingDeleted = false;
    public override void _Ready()
    {
        MaxValue = GameManager.Instance.ReloadCooldown;
        SignalManager.Instance.HamsterShot += OnHamsterShot;
        SignalManager.Instance.UpdateReloadProgress += UpdateReloadProgress;

    }
    public override void _Notification(int what)
    {
        if (what == NotificationPredelete)
        {
            _isBeingDeleted = true;
            if (SignalManager.Instance != null)
            {
                SignalManager.Instance.UpdateReloadProgress -= UpdateReloadProgress;
                SignalManager.Instance.HamsterShot -= OnHamsterShot;
            }
        }
    }

    public void UpdateReloadProgress()
    {
        Value = GameManager.Instance.RemainingReloadCooldown;
        if (Value == MaxValue
        && !GameManager.Instance.finishedReloadBar)
        {
            GameManager.Instance.finishedReloadBar = true;
            SignalManager.Instance.EmitSignal(nameof(SignalManager.FinishReload));
        }
    }
    private void OnHamsterShot()
    {
        GameManager.Instance.finishedReloadBar = false;
    }
}

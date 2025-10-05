using Godot;
using System;

public partial class AmmoLabel : Label
{
    public override void _Ready()
    {
        UpdateAmmoCount();
        SignalManager.Instance.UpdateAmmoCount += UpdateAmmoCount;
    }
    public override void _Notification(int what)
    {
        if (what == NotificationPredelete)
        {
            if (SignalManager.Instance != null)
            {
                SignalManager.Instance.UpdateAmmoCount -= UpdateAmmoCount;
            }
        }
    }
    public void UpdateAmmoCount()
    {
        if (IsInstanceValid(this))
        {
            var ammoCount = GameManager.Instance.ShotsCount - GameManager.Instance.ShotsTaken;
            Text = $"{ammoCount}/{GameManager.Instance.ShotsCount}";
        }
    }
}

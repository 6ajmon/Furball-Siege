using Godot;
using System;

public partial class RoundLostMenu : Control
{
    public override void _Ready()
    {
        ShowMenu();
    }
    private void ShowMenu()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        GetTree().Paused = true;
        Show();
        SceneManager.Instance.IsMenuOpen = true;
    }
    private void HideMenu()
    {
        GetTree().Paused = false;
        SceneManager.Instance.IsMenuOpen = false;
        QueueFree();
    }

    public void OnTryAgainButtonPressed()
    {
        HideMenu();
        SignalManager.Instance.EmitSignal(nameof(SignalManager.RestartGame));
    }
}

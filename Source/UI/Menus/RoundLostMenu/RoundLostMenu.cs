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

    public void OnTryAgainButtonPressed()
    {
        SceneManager.Instance.IsMenuOpen = false;
        SignalManager.Instance.EmitSignal(nameof(SignalManager.RestartGame));
    }

    public void OnReturnToMainMenuButtonPressed()
    {
        SceneManager.Instance.IsMenuOpen = false;
        // Logic to return to the main menu
    }
}

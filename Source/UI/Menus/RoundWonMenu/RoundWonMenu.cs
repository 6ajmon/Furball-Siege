using Godot;
using System;

public partial class RoundWonMenu : Control
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
    public void OnNextRoundButtonPressed()
    {
        HideMenu();
        SignalManager.Instance.EmitSignal(nameof(SignalManager.NextRound));
    }
}

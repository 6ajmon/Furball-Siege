using Godot;
using System;

public partial class PauseMenu : Control
{
    public bool IsPaused { get; private set; } = false;

    public override void _Ready()
    {
        SignalManager.Instance.PauseGame += PauseGame;
    }
    public override void _PhysicsProcess(double delta)
    {
        if (!SceneManager.Instance.IsMenuOpen)
        {
            if (Input.IsActionJustPressed("PauseGame"))
            {
                if (IsPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }
    }
    public void PauseGame()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        IsPaused = true;
        GetTree().Paused = true;
        Show();
    }

    public void ResumeGame()
    {
        IsPaused = false;
        GetTree().Paused = false;
        Hide();
    }

    public void OnResumeButtonPressed()
    {
        ResumeGame();
    }

    public void OnRestartButtonPressed()
    {
        ResumeGame();
        SignalManager.Instance.EmitSignal(nameof(SignalManager.RestartGame));
    }

}

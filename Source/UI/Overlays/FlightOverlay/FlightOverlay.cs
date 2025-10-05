using Godot;
using System;

public partial class FlightOverlay : Control
{

    [Export] public TextureButton PauseButton;
    [Export] public TextureButton CycleLeftButton;
    [Export] public TextureButton CycleRightButton;

    private Camera3D _parentCamera;

    public override void _Ready()
    {
        _parentCamera = GetParent<Camera3D>();
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

    public void OnPauseButtonPressed()
    {
        ButtonSoundEffect();
        SignalManager.Instance.EmitSignal(nameof(SignalManager.PauseGame));
    }
    private void ButtonSoundEffect()
    {
        // TODO: audio
    }
}

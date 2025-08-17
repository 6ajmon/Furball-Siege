using Godot;
using System;

public partial class MainScreen : Node
{
    [Export] private Control focusControl;
    public override void _Ready()
    {
        SignalManager.Instance.ReturnButtonPressed += OnReturnButtonPressed;

        AudioManager.Instance.EmitSignal(nameof(AudioManager.PlayMainScreenMusic));
    }
    public void OnSourceCodeButtonPressed()
    {
        OS.ShellOpen("https://github.com/6ajmon/Furball-Siege");
    }

    private void OnReturnButtonPressed()
    {
        focusControl.GrabFocus();
    }
}

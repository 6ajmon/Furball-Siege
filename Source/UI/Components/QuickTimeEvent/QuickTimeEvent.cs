using Godot;
using System;

public partial class QuickTimeEvent : Control
{
    [Export] private ProgressBar progressBar;
    [Export] private Timer timer;
    [Export] public float maxTime = 1.0f;
    [Export] public float minimumSuccessThreshold = 37.0f;
    [Export] public float maximumSuccessThreshold = 64.0f;
    private bool isEventChecked = false;
    private double progressBarValue;

    public override void _Ready()
    {
        timer.WaitTime = maxTime;
        timer.Timeout += OnTimeout;
        timer.Start();
        progressBar.MaxValue = 100;
    }
    public override void _PhysicsProcess(double delta)
    {
        if (!isEventChecked)
        {
            progressBar.Value = timer.TimeLeft / maxTime * 100;
        }
        else
        {
            progressBar.Value = progressBarValue;
        }
        if (Input.IsActionJustPressed("Shoot"))
        {
            CheckEventSuccess();
        }
    }
    private void OnShootButtonPressed()
    {
        CheckEventSuccess();
    }

    private void OnTimeout()
    {
        CheckEventSuccess();
    }

    private void CheckEventSuccess()
    {
        progressBarValue = progressBar.Value;
        isEventChecked = true;
        timer.Stop();
        bool success = progressBarValue >= minimumSuccessThreshold && progressBarValue <= maximumSuccessThreshold;
        SignalManager.Instance.EmitSignal(SignalManager.SignalName.QuickTimeEventCompleted, success);
        EventEnded();
    }
    public async void EventEnded()
    {
        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
        QueueFree();
    }
}

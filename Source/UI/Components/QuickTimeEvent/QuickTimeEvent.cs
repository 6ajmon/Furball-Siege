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
    private bool isQTEActive = false;

    public override void _Ready()
    {
        // Hide the QTE initially
        Visible = false;
        
        // Start the QTE after minimum delay
        StartQTEWithDelay();
    }
    
    private async void StartQTEWithDelay()
    {
        // Wait at least 0.5 seconds
        await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
        
        // Emit UI sound 0.5 seconds before QTE appears
        AudioManager.Instance.EmitSignal(nameof(AudioManager.UIOpen));
        
        // Wait another 0.5 seconds
        await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
        
        // Now show and start the QTE
        Visible = true;
        isQTEActive = true;
        timer.WaitTime = maxTime;
        timer.Timeout += OnTimeout;
        timer.Start();
        progressBar.MaxValue = 100;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!isQTEActive) return;
        
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
        if (!isQTEActive) return;
        
        progressBarValue = progressBar.Value;
        isEventChecked = true;
        timer.Stop();
        bool success = progressBarValue >= minimumSuccessThreshold && progressBarValue <= maximumSuccessThreshold;
        
        if (success)
        {
            AudioManager.Instance.EmitSignal(nameof(AudioManager.UISelect));
        }
        else
        {
            AudioManager.Instance.EmitSignal(nameof(AudioManager.UIClose));
        }
        
        SignalManager.Instance.EmitSignal(SignalManager.SignalName.QuickTimeEventCompleted, success);
        EventEnded();
    }
    public async void EventEnded()
    {
        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
        QueueFree();
    }
}

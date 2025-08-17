using Godot;
using System;

public partial class SplashScreen : Control
{
    [Export] public PackedScene MainScreenScene;
    [Export] public float SplashDuration = 3.0f;
    [Export] public float FadeInDuration = 0.5f;
    [Export] public float FadeOutDuration = 1.0f;
    
    private Timer _splashTimer;
    private Tween _fadeTween;

    public override void _Ready()
    {
        // Start completely dark
        Modulate = Colors.Black;
        
        // Start with fade in
        FadeIn();
    }

    private void FadeIn()
    {
        _fadeTween = CreateTween();
        _fadeTween.TweenProperty(this, "modulate", Colors.White, FadeInDuration);
        _fadeTween.TweenCallback(Callable.From(StartSplashTimer));
    }

    private void StartSplashTimer()
    {
        _splashTimer = new Timer();
        _splashTimer.WaitTime = SplashDuration;
        _splashTimer.OneShot = true;
        _splashTimer.Timeout += OnSplashTimeout;
        AddChild(_splashTimer);
        
        _splashTimer.Start();
    }

    private void OnSplashTimeout()
    {
        _fadeTween = CreateTween();
        _fadeTween.TweenProperty(this, "modulate", Colors.Black, FadeOutDuration);
        _fadeTween.TweenCallback(Callable.From(TransitionToMainScreen));
    }

    private void TransitionToMainScreen()
    {
        if (MainScreenScene != null)
        {
            GetTree().ChangeSceneToPacked(MainScreenScene);
        }
        else
        {
            GD.PrintErr("MainScreenScene is not set in SplashScreen!");
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsPressed() && _splashTimer != null)
        {
            _splashTimer.Stop();
            OnSplashTimeout();
        }
    }
}

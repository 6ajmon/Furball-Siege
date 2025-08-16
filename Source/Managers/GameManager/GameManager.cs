using Godot;
using System;

public partial class GameManager : Node
{
    public static GameManager Instance => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<GameManager>("GameManager");
    public enum GameState
    {
        Aiming,
        Shooting,
        GameOver
    }
    public GameState CurrentGameState { get; set; } = GameState.Aiming;
    public const float MINIMUM_SPEED_FOR_DAMAGE = 1f;
    public float MapSize { get; set; }
    public float FortDistance { get; set; }
    public int randomSeed { get; set; } = new Random().Next();
    public int InitialEnemyCount { get; set; }
    public int ShotsCount { get; set; }
    public int ShotsTaken { get; set; } = 0;
    public int CurrentRound { get; set; }
    public int CurrentEnemyCount { get; set; }

    private Timer _lastShotTimer;
    public float lastShotTimerWaitTime = 10.0f;

    public bool HasShotsRemaining => ShotsTaken < ShotsCount;

    public override void _Ready()
    {
        _lastShotTimer = new Timer();
        _lastShotTimer.WaitTime = lastShotTimerWaitTime;
        _lastShotTimer.OneShot = true;
        _lastShotTimer.ProcessCallback = Timer.TimerProcessCallback.Physics;
        _lastShotTimer.Timeout += OnLastShotTimeout;
        AddChild(_lastShotTimer);
    }

    public void CalculateShotsCount()
    {
        ShotsCount = (int)(InitialEnemyCount * 1.2) + 2;
    }

    public void TakeShot()
    {
        if (ShotsTaken >= ShotsCount)
        {
            SignalManager.Instance.EmitSignal(nameof(SignalManager.RoundLost));
        }
        ShotsTaken++;
        if (ShotsTaken >= ShotsCount)
        {
            _lastShotTimer.Start();
        }
    }

    private void OnLastShotTimeout()
    {
        if (CurrentEnemyCount > 0)
        {
            SignalManager.Instance.EmitSignal(nameof(SignalManager.RoundLost));
        }
    }

    public void ResetGame()
    {
        ShotsTaken = 0;
        CurrentRound = 1;
        CurrentEnemyCount = InitialEnemyCount;
        CalculateShotsCount();
        CurrentGameState = GameState.Aiming;
        _lastShotTimer.Stop();
    }

    public void EnemyDied()
    {
        CurrentEnemyCount--;
        if (CurrentEnemyCount <= 0)
        {
            _lastShotTimer.Stop();
            SignalManager.Instance.EmitSignal(nameof(SignalManager.RoundWon));
        }
    }
}
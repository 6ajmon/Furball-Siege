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
    public bool FortGenerating { get; set; } = false;
    public int randomSeed { get; set; } = new Random().Next();
    public int InitialEnemyCount { get; set; } = 2;
    public int ShotsCount { get; set; }
    public int ShotsTaken { get; set; } = 0;
    public int CurrentRound { get; set; } = 0;
    public int CurrentEnemyCount { get; set; }
    public float ReloadCooldown { get; set; } = 6f;
    public float RemainingReloadCooldown { get; set; }
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
        SignalManager.Instance.EmitSignal(nameof(SignalManager.UpdateEnemiesRemaining));
        SignalManager.Instance.EmitSignal(nameof(SignalManager.UpdateAmmoCount));
    }

    public void CalculateShotsCount()
    {
        ShotsCount = (int)(CurrentEnemyCount * 1.2) + 2;
    }

    public void TakeShot()
    {
        if (ShotsTaken >= ShotsCount)
        {
            SignalManager.Instance.EmitSignal(nameof(SignalManager.RoundLost));
        }
        ShotsTaken++;
        SignalManager.Instance.EmitSignal(nameof(SignalManager.UpdateAmmoCount));
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
        SignalManager.Instance.EmitSignal(nameof(SignalManager.UpdateEnemiesRemaining));
    }
    
    public void NextRound()
    {
        CurrentRound++;
        
        CurrentEnemyCount = 2 + (CurrentRound - 1);
        
        ShotsTaken = 0;
        CalculateShotsCount();
        CurrentGameState = GameState.Aiming;
        _lastShotTimer.Stop();
        
        SignalManager.Instance.EmitSignal(nameof(SignalManager.RoundNumberChanged));
        SignalManager.Instance.EmitSignal(nameof(SignalManager.UpdateEnemiesRemaining));
        SignalManager.Instance.EmitSignal(nameof(SignalManager.UpdateAmmoCount));
    }
    
    public int GetMapSizeForRound()
    {
        return 20 + ((CurrentRound - 1) / 3);
    }

    public void EnemyDied()
    {
        CurrentEnemyCount--;
        SignalManager.Instance.EmitSignal(nameof(SignalManager.UpdateEnemiesRemaining));
        if (CurrentEnemyCount <= 0)
        {
            _lastShotTimer.Stop();
            SignalManager.Instance.EmitSignal(nameof(SignalManager.RoundWon));
        }
    }
}
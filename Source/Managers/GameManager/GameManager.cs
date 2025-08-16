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

    public bool HasShotsRemaining => ShotsTaken < ShotsCount;

    public void CalculateShotsCount()
    {
        ShotsCount = (int)(InitialEnemyCount * 1.2) + 2;
    }

    public void TakeShot()
    {
        ShotsTaken++;
    }
    public void ResetGame()
    {
        ShotsTaken = 0;
        CurrentRound = 1;
        CurrentEnemyCount = InitialEnemyCount;
        CalculateShotsCount();
        CurrentGameState = GameState.Aiming;
    }
    public void EnemyDied()
    {
        GD.Print($"Enemy died. Remaining: {CurrentEnemyCount - 1}");
        CurrentEnemyCount--;
        if (CurrentEnemyCount <= 0)
        {
            SignalManager.Instance.EmitSignal(nameof(SignalManager.RoundWon));
        }
    }
}
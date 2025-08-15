using Godot;
using System;

public partial class GameManager : Node
{
    public static GameManager Instance => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<GameManager>("GameManager");
    public enum GameState
    {
        Aiming,
        Shooting
    }
    public GameState CurrentGameState { get; set; } = GameState.Aiming;
    public const float MINIMUM_SPEED_FOR_DAMAGE = 3f;
    public float MapSize { get; set; }
    public float FortDistance { get; set; }
    public int randomSeed { get; set; } = new Random().Next();
    public int EnemyCount { get; set; }
    public int ShotsCount { get; set; }
    public void CalculateShotsCount()
    {
        ShotsCount = (int)(EnemyCount * 1.2) + 2;
    }
}

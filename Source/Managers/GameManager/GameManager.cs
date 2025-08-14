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
    public float MinimumSpeedForDamage { get; set; } = 5f;
    public float MapSize { get; set; }
    public float FortDistance { get; set; }
}

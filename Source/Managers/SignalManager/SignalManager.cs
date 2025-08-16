using Godot;
using System;

public partial class SignalManager : Node
{
    public static SignalManager Instance => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<SignalManager>("SignalManager");

    [Signal] public delegate void FortGeneratedEventHandler();
    [Signal] public delegate void GnomeDiedEventHandler();
    [Signal] public delegate void RoundWonEventHandler();
    [Signal] public delegate void RoundLostEventHandler();
    [Signal] public delegate void RestartGameEventHandler();
    public override void _Ready()
    {
        GnomeDied += OnGnomeDied;
    }

    private void OnGnomeDied()
    {
        GameManager.Instance.EnemyDied();
    }
}

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
    [Signal] public delegate void NextRoundEventHandler();

    [Signal] public delegate void PauseGameEventHandler();
    [Signal] public delegate void ReloadAmmoEventHandler();
    [Signal] public delegate void ShootEventHandler();
    [Signal] public delegate void RotateLeftEventHandler();
    [Signal] public delegate void RotateRightEventHandler();
    [Signal] public delegate void RotateDownEventHandler();
    [Signal] public delegate void RotateUpEventHandler();
    [Signal] public delegate void CycleLeftEventHandler();
    [Signal] public delegate void CycleRightEventHandler();


    [Signal] public delegate void UpdateAmmoCountEventHandler();
    [Signal] public delegate void UpdateReloadProgressEventHandler();
    [Signal] public delegate void UpdateEnemiesRemainingEventHandler();
    [Signal] public delegate void RoundNumberChangedEventHandler();

    public override void _Ready()
    {
        GnomeDied += OnGnomeDied;
    }

    private void OnGnomeDied()
    {
        GameManager.Instance.EnemyDied();
    }
}

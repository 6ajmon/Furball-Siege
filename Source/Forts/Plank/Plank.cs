using Godot;
using System;

public partial class Plank : RigidBody3D
{
    [Signal] public delegate void halfHealthReachedEventHandler(Plank plank);
    [Export] public HealthComponent HealthComponent;
    private bool _halfHealthReached = false;
    [Export] public float ContactDamage { get; set; } = 1f;

    public override void _Ready()
    {
        HealthComponent.damageTaken += OnDamageTaken;
        HealthComponent.healthDepleted += OnHealthDepleted;
        halfHealthReached += (plank) =>
        {
            _halfHealthReached = true;
        };
    }

    private void OnDamageTaken(float _damageAmount)
    {
        if (!_halfHealthReached && HealthComponent.CurrentHealth <= HealthComponent.MaxHealth / 2)
        {
            SignalManager.Instance.EmitSignal(SignalManager.SignalName.plankHalfHealthReached, this);
        }
    }

    private void OnHealthDepleted()
    {
        if (!_halfHealthReached)
        {
            SignalManager.Instance.EmitSignal(SignalManager.SignalName.plankHalfHealthReached, this);
        }
        SignalManager.Instance.EmitSignal(SignalManager.SignalName.plankHealthDepleted);
        QueueFree();
    }
}
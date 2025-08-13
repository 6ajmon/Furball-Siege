using Godot;
using System;

public partial class Plank : RigidBody3D
{
    [Signal] public delegate void halfHealthReachedEventHandler(Plank plank);
    [Export] public HealthComponent HealthComponent;
    private bool _halfHealthReached = false;

    public override void _Ready()
    {
        HealthComponent.damageTaken += OnDamageTaken;
    }

    private void OnDamageTaken(float _damageAmount)
    {
        if (!_halfHealthReached && HealthComponent.CurrentHealth <= HealthComponent.MaxHealth / 2)
        {
            EmitSignal(SignalName.halfHealthReached, this);
            _halfHealthReached = true;
        }
    }
}

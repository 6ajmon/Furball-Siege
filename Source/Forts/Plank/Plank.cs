using Godot;
using System;

public partial class Plank : RigidBody3D
{
    [Signal] public delegate void halfHealthReachedEventHandler(Plank plank);
    [Export] public HealthComponent HealthComponent;
    [Export] public HitboxComponent HitboxComponent;
    private bool _halfHealthReached = false;
    [Export] public float HitboxActivationDelay = 5.0f;

    public override void _Ready()
    {
        HealthComponent.damageTaken += OnDamageTaken;
        HealthComponent.healthDepleted += OnHealthDepleted;
        halfHealthReached += (plank) =>
        {
            _halfHealthReached = true;
        };
        if (HitboxComponent != null)
        {
            HitboxComponent.Monitorable = false;
            HitboxComponent.Monitoring = false;
        }

        Timer _hitboxActivationTimer = new Timer();
        _hitboxActivationTimer.WaitTime = GD.RandRange(HitboxActivationDelay - 1.0f, HitboxActivationDelay + 1.0f);
        _hitboxActivationTimer.OneShot = true;
        _hitboxActivationTimer.Timeout += OnHitboxActivationTimeout;
        AddChild(_hitboxActivationTimer);
        _hitboxActivationTimer.Start();
    }

    private void OnDamageTaken(float _damageAmount)
    {
        if (!_halfHealthReached && HealthComponent.CurrentHealth <= HealthComponent.MaxHealth / 2)
        {
            EmitSignal(SignalName.halfHealthReached, this);
        }
    }

    private void OnHealthDepleted()
    {
        if (!_halfHealthReached)
        {
            EmitSignal(SignalName.halfHealthReached, this);
        }
        QueueFree();
    }

    private void OnHitboxActivationTimeout()
    {
        if (HitboxComponent != null)
        {
            HitboxComponent.Monitorable = true;
            HitboxComponent.Monitoring = true;
        }
    }
}

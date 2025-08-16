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

    private void OnBodyEntered(Node body)
    {
        if (body is RigidBody3D rigidBody)
        {
            float velocity = rigidBody.LinearVelocity.Length();

            if (velocity >= GameManager.MINIMUM_SPEED_FOR_DAMAGE)
            {
                if (HealthComponent != null)
                {
                    HealthComponent.DealDamage(attack: new Attack
                    {
                        Damage = ContactDamage,
                        GlobalPosition = rigidBody.GlobalPosition,
                        SpeedForce = velocity
                    });
                }
            }
        }
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
}
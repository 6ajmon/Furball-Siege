using Godot;
using System;

public partial class HealthComponent : Node
{
    [Signal] public delegate void damageTakenEventHandler(float damageAmount);
    [Signal] public delegate void healthDepletedEventHandler();
    [Export] public float MaxHealth { get; set; } = 100;
    [Export] public DamageNumberDisplayComponent DamageNumberDisplay { get; set; }
    public float CurrentHealth { get; private set; }
    private bool _isDepleted = false;

    public override void _Ready()
    {
        CurrentHealth = MaxHealth;
        healthDepleted += () =>
        {
            _isDepleted = true;
        };
    }

    public void DealDamage(Attack attack)
    {
        float damageToDeal = attack.Damage * attack.SpeedForce;
        CurrentHealth -= damageToDeal;

        DamageNumberDisplay?.ShowDamage(damageToDeal, attack.GlobalPosition);
        EmitSignal(SignalName.damageTaken, damageToDeal);

        if (CurrentHealth <= 0)
        {
            EmitSignal(SignalName.healthDepleted);
        }
    }
}

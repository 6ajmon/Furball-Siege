using Godot;
using System;

public partial class HealthComponent : Node
{
    [Signal] public delegate void damageTakenEventHandler(float damageAmount);
    [Export] public float MaxHealth { get; set; } = 100;
    [Export] public DamageNumberDisplayComponent DamageNumberDisplay { get; set; }
    public float CurrentHealth { get; private set; }

    public override void _Ready()
    {
        CurrentHealth = MaxHealth;
    }

    public void DealDamage(Attack attack)
    {
        float damageToDeal = attack.Damage * attack.SpeedForce;
        CurrentHealth -= damageToDeal;

        DamageNumberDisplay?.ShowDamage(damageToDeal, attack.GlobalPosition);
        EmitSignal(SignalName.damageTaken, damageToDeal);
    }
}

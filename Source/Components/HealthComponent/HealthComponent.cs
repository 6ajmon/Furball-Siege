using Godot;
using System;

public partial class HealthComponent : Node
{
    [Export] public float MaxHealth { get; set; } = 100;
    [Export] public DamageNumberDisplayComponent DamageNumberDisplay { get; set; }
    private float _currentHealth;

    public override void _Ready()
    {
        _currentHealth = MaxHealth;
    }

    public void DealDamage(Attack attack)
    {
        if (attack.SpeedForce < 25)
        {
            // Ignore low-speed attacks
            return;
        }
        float damageToDeal = attack.Damage * attack.SpeedForce;
        _currentHealth -= damageToDeal;

        DamageNumberDisplay?.ShowDamage(damageToDeal, attack.GlobalPosition);
    }
}

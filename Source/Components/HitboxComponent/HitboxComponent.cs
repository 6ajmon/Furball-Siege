using Godot;
using System;

public partial class HitboxComponent : Area3D
{
    [Export] public HealthComponent HealthComponent { get; set; }
    [Export] public float ContactDamage { get; set; } = 10f;
    private RigidBody3D _parent;

    public override void _Ready()
    {
        _parent = GetParent<RigidBody3D>();
    }

    public void OnAreaEntered(Area3D area)
    {
        if (_parent.LinearVelocity.Length() < GameManager.MINIMUM_SPEED_FOR_DAMAGE) return;

        if (area is HitboxComponent hitbox)
        {
            Attack attack = new(
                ContactDamage,
                _parent.GlobalPosition,
                _parent.LinearVelocity.Length()
                );
            hitbox.HealthComponent.DealDamage(attack);
        }
    }
    
    public void OnBodyEntered(Node3D body)
    {
        bool isParentBelowMinimumSpeed = _parent.LinearVelocity.Length() < GameManager.MINIMUM_SPEED_FOR_DAMAGE;
        bool bodyIsBelowMinimumSpeed = body is RigidBody3D rigidBody && rigidBody.LinearVelocity.Length() < GameManager.MINIMUM_SPEED_FOR_DAMAGE;
        if (isParentBelowMinimumSpeed && bodyIsBelowMinimumSpeed) return;

        if (body is Plank plank)
        {
            if (!isParentBelowMinimumSpeed)
            {
                Attack attack = new(
                    ContactDamage,
                    _parent.GlobalPosition,
                    _parent.LinearVelocity.Length()
                    );
                plank.HealthComponent.DealDamage(attack);
                plank.ApplyCentralImpulse(_parent.LinearVelocity * attack.SpeedForce * attack.SpeedForce * attack.Damage);
            }
            if (!bodyIsBelowMinimumSpeed)
            {
                Attack attack = new(
                    plank.ContactDamage,
                    plank.GlobalPosition,
                    plank.LinearVelocity.Length()
                    );
                HealthComponent.DealDamage(attack);
            }
        }
    }
}

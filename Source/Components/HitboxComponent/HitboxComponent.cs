using Godot;
using System;

public partial class HitboxComponent : Area3D
{
    [Export] public HealthComponent HealthComponent { get; set; }
    [Export] public float ContactDamage { get; set; } = 10f;
    private Node3D _parent;

    public override void _Ready()
    {
        _parent = GetParent<Node3D>();
    }

    private Vector3 GetParentVelocity()
    {
        return _parent is RigidBody3D rigidBody ? rigidBody.LinearVelocity : Vector3.Zero;
    }

    private bool IsParentBelowMinimumSpeed()
    {
        return GetParentVelocity().Length() < GameManager.MINIMUM_SPEED_FOR_DAMAGE;
    }

    public void OnAreaEntered(Area3D area)
    {
        if (IsParentBelowMinimumSpeed()) return;

        if (area is HitboxComponent hitbox)
        {
            Vector3 parentVelocity = GetParentVelocity();
            Attack attack = new(
                ContactDamage,
                _parent.GlobalPosition,
                parentVelocity.Length()
                );
            hitbox.HealthComponent.DealDamage(attack);
        }
    }
    
    public void OnBodyEntered(Node3D body)
    {
        bool isParentBelowMinimumSpeed = IsParentBelowMinimumSpeed();
        bool bodyIsBelowMinimumSpeed = body is RigidBody3D rigidBody && rigidBody.LinearVelocity.Length() < GameManager.MINIMUM_SPEED_FOR_DAMAGE;
        if (isParentBelowMinimumSpeed && bodyIsBelowMinimumSpeed) return;

        if (body is Plank plank)
        {
            if (!isParentBelowMinimumSpeed)
            {
                Vector3 parentVelocity = GetParentVelocity();
                Attack attack = new(
                    ContactDamage,
                    _parent.GlobalPosition,
                    parentVelocity.Length()
                    );
                plank.HealthComponent.DealDamage(attack);
                plank.ApplyCentralImpulse(parentVelocity * attack.SpeedForce * attack.SpeedForce * attack.Damage);
            }
            if (!bodyIsBelowMinimumSpeed)
            {
                Attack attack = new(
                    plank.ContactDamage,
                    plank.GlobalPosition,
                    plank.LinearVelocity.Length()
                    );
                HealthComponent.DealDamage(attack);
                plank.HealthComponent.DealDamage(attack);
            }
        }
    }
}
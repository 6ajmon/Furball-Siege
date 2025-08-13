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
}

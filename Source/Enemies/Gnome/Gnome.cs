using Godot;
using System;

public partial class Gnome : RigidBody3D
{
    [Export] HealthComponent HealthComponent;

    public override void _Ready()
    {
        HealthComponent = GetNode<HealthComponent>("HealthComponent");
        HealthComponent.healthDepleted += OnHealthDepleted;
    }

    private void OnHealthDepleted()
    {
        QueueFree();
    }
}

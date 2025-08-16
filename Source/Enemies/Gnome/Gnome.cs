using Godot;
using System;

public partial class Gnome : RigidBody3D
{
    private HealthComponent HealthComponent;

    public override void _Ready()
    {
        HealthComponent = GetNode<HealthComponent>("HealthComponent");
        HealthComponent.healthDepleted += OnHealthDepleted;
    }

    private void OnHealthDepleted()
    {
        SignalManager.Instance.EmitSignal(nameof(SignalManager.GnomeDied));
        QueueFree();
    }
}

using Godot;
using System;

public partial class Gnome : RigidBody3D
{
    private HealthComponent HealthComponent;
    private bool _isDead = false;

    public override void _Ready()
    {
        HealthComponent = GetNode<HealthComponent>("HealthComponent");
        HealthComponent.healthDepleted += OnHealthDepleted;
    }

    private void OnHealthDepleted()
    {
        if (_isDead) return;
        
        _isDead = true;
        SignalManager.Instance.EmitSignal(SignalManager.SignalName.GnomeDied);
        QueueFree();
    }
}

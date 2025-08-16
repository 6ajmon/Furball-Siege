using Godot;
using System;

public partial class DamageNumberDisplayComponent : Node3D
{
    [Export] public PackedScene DamageNumberScene { get; set; }
    [Export] public Vector3 OffsetRange { get; set; } = new Vector3(1, 1, 0);

    public void ShowDamage(float damage, Vector3 numberPosition)
    {
        if (DamageNumberScene == null)
            return;
        
        var damageNumber = DamageNumberScene.Instantiate() as DamageNumber;
        GetTree().CurrentScene.AddChild(damageNumber, true);
        
        var randomOffset = new Vector3(
            (float)GD.RandRange(-OffsetRange.X, OffsetRange.X),
            (float)GD.RandRange(-OffsetRange.Y, OffsetRange.Y),
            (float)GD.RandRange(-OffsetRange.Z, OffsetRange.Z)
        );
        
        damageNumber.SetupDamage(damage, numberPosition + randomOffset);
    }
}
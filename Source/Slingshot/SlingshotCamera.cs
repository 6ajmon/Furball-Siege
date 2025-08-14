using Godot;
using System;

public partial class SlingshotCamera : Camera3D
{
    [Export] public float rotationSpeed = 0.03f;
    [Export] public float maxRotationAngle = Mathf.Pi / 2;
    [Export] public Slingshot slingshot;

    private float currentRotation = 0f;

    public override void _Ready()
    {
        SignalManager.Instance.FortGenerated += OnFortGenerated;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Current)
        {
            if (slingshot != null)
            {
                if (Input.IsActionPressed("Left") && currentRotation < maxRotationAngle)
                {
                    slingshot.RotateY(rotationSpeed);
                    currentRotation += rotationSpeed;
                }
                if (Input.IsActionPressed("Right") && currentRotation > -maxRotationAngle)
                {
                    slingshot.RotateY(-rotationSpeed);
                    currentRotation -= rotationSpeed;
                }
            }
        }
    }

    private void OnFortGenerated()
    {
        //
        GD.Print("Fort generated, camera adjustments can be made here if needed.");
    }

}

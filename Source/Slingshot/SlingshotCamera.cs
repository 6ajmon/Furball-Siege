using Godot;
using System;

public partial class SlingshotCamera : Camera3D
{
    [Export] public float rotationSpeed = 0.06f;
    [Export] public float maxRotationAngle = Mathf.Pi/2;
    [Export] public Slingshot slingshot;
    
    private float currentRotation = 0f;
    
    public override void _PhysicsProcess(double delta)
    {
        if (Current)
        {
            if (Input.IsActionPressed("Shoot"))
            {
                GD.Print("Shooting from slingshot!");
            }

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

}

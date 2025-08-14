using Godot;
using System;

public partial class SlingshotCamera : Camera3D
{
    [Export] public float rotationSpeed = 0.03f;
    [Export] public float maxRotationHorizontalAngle = Mathf.Pi / 2;
    [Export]
    public float maxRotationVerticalAngle = Mathf.Pi / 12;
    [Export] public Slingshot slingshot;

    private float currentHorizontalRotation = 0f;
    private float currentVerticalRotation = 0f;

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
                if (Input.IsActionPressed("Left") && currentHorizontalRotation < maxRotationHorizontalAngle)
                {
                    slingshot.RotateY(rotationSpeed);
                    currentHorizontalRotation += rotationSpeed;
                }
                if (Input.IsActionPressed("Right") && currentHorizontalRotation > -maxRotationHorizontalAngle)
                {
                    slingshot.RotateY(-rotationSpeed);
                    currentHorizontalRotation -= rotationSpeed;
                }
                if (Input.IsActionPressed("Backwards") && currentVerticalRotation < maxRotationVerticalAngle - Mathf.Pi/24)
                {
                    slingshot.RotateObjectLocal(Vector3.Right, -rotationSpeed);
                    currentVerticalRotation += rotationSpeed;
                }
                if (Input.IsActionPressed("Forward") && currentVerticalRotation > -maxRotationVerticalAngle + Mathf.Pi/16)
                {
                    slingshot.RotateObjectLocal(Vector3.Right, rotationSpeed);
                    currentVerticalRotation -= rotationSpeed;
                }
            }
        }
    }

    private void OnFortGenerated()
    {
        float fortDistance = GameManager.Instance.FortDistance;
        float fortSize = GameManager.Instance.MapSize;

        maxRotationHorizontalAngle = Mathf.Atan2(fortDistance, fortSize / 2);
    }

}
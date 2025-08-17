using Godot;
using System;

public partial class SlingshotCamera : Camera3D
{
    [Export] public float rotationSpeed = 0.03f;
    [Export] public float maxRotationHorizontalAngle = Mathf.Pi / 2;
    [Export]
    public float maxRotationVerticalAngle = Mathf.Pi / 10;
    [Export] public Slingshot slingshot;

    private float currentHorizontalRotation = 0f;
    private float currentVerticalRotation = 0f;

    public override void _Ready()
    {
        SignalManager.Instance.FortGenerated += OnFortGenerated;
        SignalManager.Instance.RotateLeft += RotateLeft;
        SignalManager.Instance.RotateRight += RotateRight;
        SignalManager.Instance.RotateUp += RotateUp;
        SignalManager.Instance.RotateDown += RotateDown;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Current)
        {
            if (slingshot != null)
            {
                if (Input.IsActionPressed("Left"))
                {
                    RotateLeft();
                }
                if (Input.IsActionPressed("Right"))
                {
                    RotateRight();
                }
                if (Input.IsActionPressed("Backwards"))
                {
                    RotateDown();
                }
                if (Input.IsActionPressed("Forward"))
                {
                    RotateUp();
                }
            }
        }
    }
    private void RotateLeft()
    {
        if (currentHorizontalRotation < maxRotationHorizontalAngle)
        {
            slingshot.RotateY(rotationSpeed);
            currentHorizontalRotation += rotationSpeed;
        }
    }

    private void RotateRight()
    {
        if (currentHorizontalRotation > -maxRotationHorizontalAngle)
        {
            slingshot.RotateY(-rotationSpeed);
            currentHorizontalRotation -= rotationSpeed;
        }
    }

    private void RotateUp()
    {
        if (currentVerticalRotation > -maxRotationVerticalAngle + Mathf.Pi/18)
        {
            slingshot.RotateObjectLocal(Vector3.Right, rotationSpeed);
            currentVerticalRotation -= rotationSpeed;
        }
    }

    private void RotateDown()
    {
        if (currentVerticalRotation < maxRotationVerticalAngle - Mathf.Pi/26)
        {
            slingshot.RotateObjectLocal(Vector3.Right, -rotationSpeed);
            currentVerticalRotation += rotationSpeed;
        }
    }

    private void OnFortGenerated()
    {
        float fortDistance = GameManager.Instance.FortDistance;
        float fortSize = GameManager.Instance.MapSize;

        maxRotationHorizontalAngle = Mathf.Atan2(fortDistance, fortSize / 2);
    }

}
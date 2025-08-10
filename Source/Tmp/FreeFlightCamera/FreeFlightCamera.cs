using Godot;
using System;

// Free flight camera for debugging and testing
public partial class FreeFlightCamera : Camera3D
{
    [Export] public float MovementSpeed = 25.0f;
    [Export] public float MouseSensitivity = 0.003f;
    [Export] public float MaxLookAngle = 90.0f;

    private float _mouseRotationX = 0.0f;
    private bool _mouseCaptured = false;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        _mouseCaptured = true;
    }

    public override void _Input(InputEvent @event)
    {
        if (Current)
        {
            if (@event is InputEventMouseMotion mouseMotion && _mouseCaptured)
            {
                RotateY(-mouseMotion.Relative.X * MouseSensitivity);

                _mouseRotationX -= mouseMotion.Relative.Y * MouseSensitivity;
                _mouseRotationX = Mathf.Clamp(_mouseRotationX, Mathf.DegToRad(-MaxLookAngle), Mathf.DegToRad(MaxLookAngle));

                RotationDegrees = new Vector3(Mathf.RadToDeg(_mouseRotationX), RotationDegrees.Y, 0);
            }

            if (@event is InputEventKey keyEvent && keyEvent.Pressed && keyEvent.Keycode == Key.Escape)
            {
                if (_mouseCaptured)
                {
                    Input.MouseMode = Input.MouseModeEnum.Visible;
                    _mouseCaptured = false;
                }
                else
                {
                    Input.MouseMode = Input.MouseModeEnum.Captured;
                    _mouseCaptured = true;
                }
            }
        }
    }

    public override void _Process(double delta)
    {
        if (Current)
        {
            HandleMovement(delta);
        }
    }

    private void HandleMovement(double delta)
    {
        var velocity = Vector3.Zero;

        var inputDir = Vector3.Zero;

        if (Input.IsActionPressed("Forward"))
            inputDir -= Transform.Basis.Z;
        if (Input.IsActionPressed("Backwards"))
            inputDir += Transform.Basis.Z;
        if (Input.IsActionPressed("Left"))
            inputDir -= Transform.Basis.X;
        if (Input.IsActionPressed("Right"))
            inputDir += Transform.Basis.X;
        if (Input.IsActionPressed("Up"))
            inputDir += Vector3.Up;
        if (Input.IsActionPressed("Down"))
            inputDir -= Vector3.Up;

        if (inputDir.Length() > 0)
        {
            inputDir = inputDir.Normalized();
            velocity = inputDir * MovementSpeed;
        }

        Position += velocity * (float)delta;
    }
}
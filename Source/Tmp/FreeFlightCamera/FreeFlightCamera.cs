using Godot;
using System;

public partial class FreeFlightCamera : Camera3D
{
    [Export] public float MovementSpeed = 25.0f;
    [Export] public float MouseSensitivity = 0.003f;
    [Export] public float MaxLookAngle = 90.0f;
    
    private float _mouseRotationX = 0.0f;
    private bool _mouseCaptured = false;
    
    public override void _Ready()
    {
        // Capture the mouse cursor
        Input.MouseMode = Input.MouseModeEnum.Captured;
        _mouseCaptured = true;
    }
    
    public override void _Input(InputEvent @event)
    {
        // Handle mouse look
        if (@event is InputEventMouseMotion mouseMotion && _mouseCaptured)
        {
            // Rotate horizontally (Y-axis) - rotate around global up
            RotateY(-mouseMotion.Relative.X * MouseSensitivity);
            
            // Rotate vertically (X-axis) with limits - rotate around local right
            _mouseRotationX -= mouseMotion.Relative.Y * MouseSensitivity;
            _mouseRotationX = Mathf.Clamp(_mouseRotationX, Mathf.DegToRad(-MaxLookAngle), Mathf.DegToRad(MaxLookAngle));
            
            // Apply vertical rotation around the local X-axis
            RotationDegrees = new Vector3(Mathf.RadToDeg(_mouseRotationX), RotationDegrees.Y, 0);
        }
        
        // Toggle mouse capture with Escape
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
    
    public override void _Process(double delta)
    {
        HandleMovement(delta);
    }
    
    private void HandleMovement(double delta)
    {
        var velocity = Vector3.Zero;
        
        // Get input strength for each direction
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
        
        // Normalize the input direction to prevent faster diagonal movement
        if (inputDir.Length() > 0)
        {
            inputDir = inputDir.Normalized();
            velocity = inputDir * MovementSpeed;
        }
        
        // Apply movement
        Position += velocity * (float)delta;
    }
}
using Godot;
using System;

public partial class Hamster : RigidBody3D
{
    [Export] public NodePath meshPath;
    [Export] public float factor = 0.08f;
    [Export] public float maxSquash = 0.5f;
    [Export] public float smoothing = 10f;

    private MeshInstance3D _mesh;
    private Vector3 _currentScale = Vector3.One;
    private Transform3D _originalTransform;
    private Vector3 _lastDirection = Vector3.Forward;

    public override void _Ready()
    {
        _mesh = GetNode<MeshInstance3D>(meshPath);
        _originalTransform = _mesh.Transform;
        CameraManager.Instance.RefreshCameraList();
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 vel = LinearVelocity;
        float speed = (float)vel.Length();

        float s = Mathf.Clamp(speed * factor, 0f, maxSquash);

        Vector3 dir = vel.Length() > 0.001f ? vel.Normalized() : _lastDirection;
        _lastDirection = _lastDirection.Lerp(dir, (float)delta * smoothing);

        Vector3 targetScale = CalculateDirectionalScale(s, _lastDirection);

        _currentScale = _currentScale.Lerp(targetScale, (float)delta * smoothing);

        ApplyMeshDeformation(_currentScale, _lastDirection);
        
    }

    private Vector3 CalculateDirectionalScale(float deformationFactor, Vector3 direction)
    {
        float main = 1f + deformationFactor;
        
        float other = Mathf.Clamp(1f - deformationFactor * 0.5f, 0.3f, 2f);

        return new Vector3(other, other, main);
    }

    private void ApplyMeshDeformation(Vector3 scale, Vector3 direction)
    {
        Transform3D rotationTransform = CreateDirectionTransform(direction);
        
        Transform3D scaleTransform = Transform3D.Identity;
        scaleTransform = scaleTransform.Scaled(scale);
        
        Transform3D finalTransform = _originalTransform * rotationTransform * scaleTransform * rotationTransform.Inverse();
        
        _mesh.Transform = finalTransform;
    }

    private Transform3D CreateDirectionTransform(Vector3 direction)
    {
        Vector3 forward = Vector3.Forward;
        
        if (direction.Dot(forward) > 0.999f)
            return Transform3D.Identity;
        
        if (direction.Dot(forward) < -0.999f)
            return Transform3D.Identity.Rotated(Vector3.Up, Mathf.Pi);
        
        Vector3 axis = forward.Cross(direction).Normalized();
        float angle = Mathf.Acos(forward.Dot(direction));
        
        return Transform3D.Identity.Rotated(axis, angle);
    }
}

using Godot;
using System;

public partial class Hamster : RigidBody3D
{
    public Camera3D Camera { get; private set; }

    public override void _Ready()
    {
        Camera = GetNode<Camera3D>("BodyParts/HeadMesh/Camera3D");
        CameraManager.Instance.RefreshCameraList();
    }
}

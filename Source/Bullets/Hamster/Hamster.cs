using Godot;
using System;

public partial class Hamster : RigidBody3D
{
    public override void _Ready()
    {
        CameraManager.Instance.RefreshCameraList();
    }
}

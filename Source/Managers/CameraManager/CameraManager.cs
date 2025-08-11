using Godot;
using System;
using System.Collections.Generic;

public partial class CameraManager : Node
{
    public static CameraManager Instance => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<CameraManager>("CameraManager");
    private int _currentCameraIndex = 0;
    private List<Camera3D> _cameras = new();

    public override void _Ready()
    {
        RefreshCameraList();
        if (_cameras.Count == 0)
        {
            GD.PrintErr("No cameras found in the scene.");
        }
        else
        {
            ActivateCamera(_currentCameraIndex);
        }
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("CycleCameraForward"))
        {
            _currentCameraIndex = (_currentCameraIndex + 1) % _cameras.Count;
            ActivateCamera(_currentCameraIndex);
        }
        else if (Input.IsActionJustPressed("CycleCameraBack"))
        {
            _currentCameraIndex = (_currentCameraIndex - 1 + _cameras.Count) % _cameras.Count;
            ActivateCamera(_currentCameraIndex);
        }
    }

    public void RefreshCameraList()
    {
        _cameras.Clear();
        foreach (Node node in GetTree().GetNodesInGroup("cameras"))
        {
            if (node is Camera3D camera)
            {
                _cameras.Add(camera);
            }
        }
    }
    public void ActivateCamera(int index)
    {
        if (index < 0 || index >= _cameras.Count)
        {
            GD.PrintErr("Camera index out of range.");
            return;
        }

        if (_currentCameraIndex >= 0 && _currentCameraIndex < _cameras.Count)
        {
            _cameras[_currentCameraIndex].Current = false;
        }

        _currentCameraIndex = index;
        _cameras[_currentCameraIndex].Current = true;
    }

}

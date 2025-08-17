using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CameraManager : Node
{
    public static CameraManager Instance => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<CameraManager>("CameraManager");
    private int _currentCameraIndex = 0;
    private List<Camera3D> _cameras = new();

    public override void _Ready()
    {
        RefreshCameraList();
        if (_cameras.Count != 0)
        {
            ActivateCamera(_currentCameraIndex);
        }
        SignalManager.Instance.CycleLeft += OnCycleLeft;
        SignalManager.Instance.CycleRight += OnCycleRight;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustPressed("CycleCameraForward"))
        {
            SignalManager.Instance.EmitSignal(nameof(SignalManager.CycleRight));
        }
        else if (Input.IsActionJustPressed("CycleCameraBack"))
        {
            SignalManager.Instance.EmitSignal(nameof(SignalManager.CycleLeft));
        }
    }

    public void RefreshCameraList()
    {
        _cameras.Clear();
        foreach (Node node in GetTree().GetNodesInGroup("cameras"))
        {
            if (node is Camera3D camera && IsInstanceValid(camera))
            {
                _cameras.Add(camera);
            }
        }

        if (_currentCameraIndex >= _cameras.Count)
        {
            _currentCameraIndex = 0;
        }
    }

    public void ActivateCamera(int index)
    {
        if (index < 0 || index >= _cameras.Count)
        {
            GD.PrintErr("Camera index out of range.");
            return;
        }

        if (_currentCameraIndex >= 0 && _currentCameraIndex < _cameras.Count && IsInstanceValid(_cameras[_currentCameraIndex]))
        {
            _cameras[_currentCameraIndex].Current = false;
        }

        _currentCameraIndex = index;
        if (IsInstanceValid(_cameras[_currentCameraIndex]))
        {
            _cameras[_currentCameraIndex].Current = true;
        }
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }
    private void OnCycleLeft()
    {
        RefreshCameraList();
        if (_cameras.Count > 0)
        {
            _currentCameraIndex = (_currentCameraIndex - 1 + _cameras.Count) % _cameras.Count;
            ActivateCamera(_currentCameraIndex);
        }
    }
    private void OnCycleRight()
    {
        RefreshCameraList();
        if (_cameras.Count > 0)
        {
            _currentCameraIndex = (_currentCameraIndex + 1) % _cameras.Count;
            ActivateCamera(_currentCameraIndex);
        }
    }
}
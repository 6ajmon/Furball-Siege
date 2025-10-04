using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CameraManager : Node
{
    public static CameraManager Instance => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<CameraManager>("CameraManager");
    private int _currentCameraIndex = 0;
    private List<Camera3D> _cameras = new();
    private const float CAMERA_SWITCH_DELAY = 0.6f;

    public override void _Ready()
    {
        RefreshCameraList();
        if (_cameras.Count > 0)
        {
            ActivateCamera(_currentCameraIndex);
        }
        SignalManager.Instance.CycleLeft += OnCycleLeft;
        SignalManager.Instance.CycleRight += OnCycleRight;
        SignalManager.Instance.HamsterShot += OnHamsterShot;
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

    private async void OnHamsterShot()
    {
        GD.Print("CameraManager: Hamster shot detected, switching camera after delay.");
        await ToSignal(GetTree().CreateTimer(CAMERA_SWITCH_DELAY), SceneTreeTimer.SignalName.Timeout);
        OnCycleRight();
    }

    public void ActivateCamera(int index)
    {
        GD.Print($"CameraManager: Activating camera at index {index}.");
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
    public void OnCycleLeft()
    {
        RefreshCameraList();
        if (_cameras.Count > 0)
        {
            var nextCameraIndex = (_currentCameraIndex - 1 + _cameras.Count) % _cameras.Count;
            ActivateCamera(nextCameraIndex);
        }
    }
    public void OnCycleRight()
    {
        GD.Print("CameraManager: Cycling right.");
        RefreshCameraList();
        if (_cameras.Count > 0)
        {
            var nextCameraIndex = (_currentCameraIndex + 1) % _cameras.Count;
            ActivateCamera(nextCameraIndex);
        }
    }
}
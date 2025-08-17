using Godot;
using System;

public partial class SceneManager : Node
{
    public static SceneManager Instance => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<SceneManager>("SceneManager");
    private string roundLostMenuScenePath = "res://Source/UI/Menus/RoundLostMenu/RoundLostMenu.tscn";
    private string roundWonMenuScenePath = "res://Source/UI/Menus/RoundWonMenu/RoundWonMenu.tscn";
    public bool IsMenuOpen { get; set; }
    public Node CurrentMenu { get; set; }
    public override void _Ready()
    {
        SignalManager.Instance.RoundLost += OnRoundLost;
        SignalManager.Instance.RoundWon += OnRoundWon;
    }
    public void ReplaceScene(string scenePath)
    {
        if (IsMenuOpen)
        {
            CurrentMenu?.QueueFree();
            CurrentMenu = null;
            IsMenuOpen = false;
        }
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile(scenePath);
    }

    public void AddChildScene(string scenePath)
    {
        PackedScene scene = ResourceLoader.Load<PackedScene>(scenePath);
        if (scene != null)
        {
            Node instance = scene.Instantiate();
            if (instance != null)
            {
                GetTree().Root.AddChild(instance);
                CurrentMenu = instance;
                IsMenuOpen = true;
            }
            else
            {
                GD.PrintErr($"Failed to instantiate scene at {scenePath}");
            }
        }
        else
        {
            GD.PrintErr($"Failed to load scene at {scenePath}");
        }
    }

    private void OnRoundLost()
    {
        if (IsMenuOpen)
            return;
        AddChildScene(roundLostMenuScenePath);
    }

    private void OnRoundWon()
    {
        if (IsMenuOpen)
            return;
        AddChildScene(roundWonMenuScenePath);
    }
}

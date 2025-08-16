using Godot;
using System;

public partial class SceneManager : Node
{
    public static SceneManager Instance => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<SceneManager>("SceneManager");
    private string roundLostMenuScenePath = "res://Source/UI/Menus/RoundLostMenu/RoundLostMenu.tscn";
    private string roundWonMenuScenePath = "res://Source/UI/Menus/RoundWonMenu/RoundWonMenu.tscn";
    public bool IsMenuOpen { get; set; }
    public override void _Ready()
    {
        SignalManager.Instance.RoundLost += OnRoundLost;
        SignalManager.Instance.RoundWon += OnRoundWon;
    }
    public void ReplaceScene(string scenePath)
    {
        GetTree().ChangeSceneToFile(scenePath);
    }

    private void OnRoundLost()
    {
        GetTree().Root.AddChild(ResourceLoader.Load<PackedScene>(roundLostMenuScenePath).Instantiate());
    }

    private void OnRoundWon()
    {
        GetTree().Root.AddChild(ResourceLoader.Load<PackedScene>(roundWonMenuScenePath).Instantiate());
    }
}

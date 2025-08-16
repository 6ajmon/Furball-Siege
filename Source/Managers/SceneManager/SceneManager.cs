using Godot;
using System;

public partial class SceneManager : Node
{
    public static SceneManager Instance => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<SceneManager>("SceneManager");

    public void ReplaceScene(string scenePath)
    {
        GetTree().ChangeSceneToFile(scenePath);
    }
}

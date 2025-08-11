using Godot;
using System;

public partial class Slingshot : Node3D
{
    [Export] public AnimationPlayer AnimationPlayer;
    private Node3D StringNode;
    private Node3D SlingNode;
    [Export] public Vector3 StringScale = new Vector3(0.224f, 1.115f, 2.085f);
    [Export] public Vector3 SlingPosition = new Vector3(0, 0, 0);

    public override void _Ready()
    {
        StringNode = GetNode<Node3D>("Sketchfab_Scene/Sketchfab_model/e030fbefb0054155b4a00186e5d83f15_fbx/RootNode/Cylinder005");
        SlingNode = GetNode<Node3D>("Sketchfab_Scene/Sketchfab_model/e030fbefb0054155b4a00186e5d83f15_fbx/RootNode/Sling/Object_4");
        CameraManager.Instance.RefreshCameraList();
    }

    public override void _Process(double delta)
    {
        if (AnimationPlayer.IsPlaying())
        {
            SlingNode.Position = SlingPosition;
            StringNode.Scale = StringScale;
            if (StringScale.Z < 0)
            {
                StringNode.Rotation = new Vector3(
                    Mathf.DegToRad(-3.4f),
                    Mathf.DegToRad(2.7f),
                    Mathf.DegToRad(-177.1f)
                );
            }
            else
            {
                StringNode.Rotation = new Vector3(
                    Mathf.DegToRad(-3.4f),
                    Mathf.DegToRad(2.7f),
                    Mathf.DegToRad(2.9f)
                );
            } 
        }
    }

    public void PlayAnimation(string animationName)
    {
        if (AnimationPlayer.HasAnimation(animationName))
        {
            AnimationPlayer.Play(animationName);
        }
        else
        {
            GD.PrintErr($"Animation '{animationName}' not found in AnimationPlayer.");
        }
    }
}

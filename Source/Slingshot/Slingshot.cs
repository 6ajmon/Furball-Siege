using Godot;
using System;

public partial class Slingshot : Node3D
{
    [Export] public AnimationPlayer AnimationPlayer;
    private Node3D StringNode;
    private Node3D SlingNode;
    [Export] public Vector3 StringScale;
    [Export] public Vector3 SlingPosition;

    public override void _Ready()
    {
        StringNode = GetNode<Node3D>("Sketchfab_Scene/Sketchfab_model/e030fbefb0054155b4a00186e5d83f15_fbx/RootNode/Cylinder005");
        SlingNode = GetNode<Node3D>("Sketchfab_Scene/Sketchfab_model/e030fbefb0054155b4a00186e5d83f15_fbx/RootNode/Sling/Object_4");

        if (AnimationPlayer != null)
        {
            AnimationPlayer.Play("SlingshotShoot");
        }
    }

    public override void _Process(double delta)
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

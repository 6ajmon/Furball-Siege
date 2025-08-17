using Godot;
using System;

public partial class Crate : Node3D
{
    private Godot.Collections.Array<Plank> _planks = new();
    private Godot.Collections.Array<HingeJoint3D> _joints = new();

    public override void _Ready()
    {
        foreach (Node child in GetChildren())
        {
            if (child is Plank plank)
            {
                _planks.Add(plank);
            }
            else if (child is HingeJoint3D joint)
            {
                _joints.Add(joint);
            }
        }
        
        SignalManager.Instance.plankHalfHealthReached += OnPlankHalfHealthReached;
    }
    public void OnPlankHalfHealthReached(Plank plank)
    {
        if (!_planks.Contains(plank))
            return;
            
        for (int i = _joints.Count - 1; i >= 0; i--)
        {
            HingeJoint3D joint = _joints[i];
            if (joint.NodeA == joint.GetPathTo(plank) || joint.NodeB == joint.GetPathTo(plank))
            {
                _joints.RemoveAt(i);
                joint.QueueFree();
            }
        }
    }
}

using Godot;
using System;

[GlobalClass]
public partial class UiButton : Button
{
    [Export] public bool ShouldTakeFocus { get; set; } = false;
    public override void _Ready()
    {
        if (ShouldTakeFocus)
        {
            GrabFocus();
        }
    }

    public void OnPressed()
    {
        // TODO add sound
    }
}

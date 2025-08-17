using Godot;
using System;

namespace FurballSiege.Source.UI.Components.Buttons.UIButton;

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


    public virtual void OnPressed()
    {
        AudioManager.Instance.EmitSignal(nameof(AudioManager.UISelect));
    }
    public virtual void OnMouseEntered()
    {
        AudioManager.Instance.EmitSignal(nameof(AudioManager.UIFocus));
    }
}

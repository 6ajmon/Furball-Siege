using FurballSiege.Source.UI.Components.Buttons.UIButton;
using Godot;
using System;

[GlobalClass]
public partial class ReturnButton : UiButton
{
    public override void _Ready()
    {
        base._Ready();
    }
    public override void OnPressed()
    {
        base.OnPressed();
        SceneManager.Instance.CurrentMenu?.QueueFree();
        SceneManager.Instance.CurrentMenu = null;
        SceneManager.Instance.IsMenuOpen = false;
    }
}

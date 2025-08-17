using FurballSiege.Source.UI.Components.Buttons.UIButton;
using Godot;
using System;

[GlobalClass]
public partial class AddChildSceneButton : UiButton
{
    [Export(PropertyHint.File, "*.tscn")]
    public string ScenePath { get; set; }
    public override void _Ready()
    {
        base._Ready();
    }

    public override void OnPressed()
    {
        base.OnPressed();
        if (!string.IsNullOrEmpty(ScenePath))
        {
            SceneManager.Instance.AddChildScene(ScenePath);
        }
    }
}

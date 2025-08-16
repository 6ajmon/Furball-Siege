using Godot;
using System;
using FurballSiege.Source.UI.Components.Buttons.UIButton;

public partial class ReplaceSceneButton : UiButton
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
            SceneManager.Instance.ReplaceScene(ScenePath);
        }
    }
}

using Godot;
using System;

public partial class DamageNumber : Control
{
    [Export] public float Duration { get; set; } = 1.5f;
    [Export] public float MoveDistance { get; set; } = 50.0f;
    [Export] public int TextSize { get; set; } = 24;
    [Export] public float MinDamageForRed { get; set; } = 50.0f;
    [Export] public float MaxSizeMultiplier { get; set; } = 2.0f;

    private Label _label;

    public override void _Ready()
    {
        _label = GetChild<Label>(0);
    }

    public void SetupDamage(float damage, Vector3 worldPosition)
    {
        _label.Text = ((int)damage).ToString();
        
        var damageRatio = Mathf.Clamp(damage / MinDamageForRed, 0.0f, 1.0f);
        var sizeMultiplier = Mathf.Lerp(1.0f, MaxSizeMultiplier, damageRatio);
        var finalTextSize = (int)(TextSize * sizeMultiplier);
        _label.AddThemeFontSizeOverride("font_size", finalTextSize);
        
        var color = Colors.White.Lerp(Colors.Red, damageRatio);
        _label.Modulate = color;
        
        var camera = GetViewport().GetCamera3D();
        if (camera != null)
        {
            var screenPos = camera.UnprojectPosition(worldPosition);
            Position = screenPos;
        }
        
        AnimateDamageNumber();
    }

    private void AnimateDamageNumber()
    {
        var tween = CreateTween();
        tween.SetParallel(true);
        
        tween.TweenProperty(this, "position:y", Position.Y - MoveDistance, Duration);
        
        tween.TweenProperty(this, "modulate:a", 0.0f, Duration);
        
        tween.TweenProperty(this, "scale", new Vector2(1.2f, 1.2f), 0.2f);
        tween.TweenProperty(this, "scale", new Vector2(1.0f, 1.0f), 0.3f).SetDelay(0.2f);
        
        tween.TweenCallback(Callable.From(QueueFree)).SetDelay(Duration);
    }
}
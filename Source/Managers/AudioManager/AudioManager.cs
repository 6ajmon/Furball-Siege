using Godot;
using System;

public partial class AudioManager : Node
{
    public static AudioManager Instance => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<AudioManager>("AudioManager");

    [Signal] public delegate void SlingShotFiredEventHandler();
    [Signal] public delegate void PlayMainScreenMusicEventHandler();
    [Signal] public delegate void PlayGameMusicEventHandler();
    [Signal] public delegate void UICancelEventHandler();
    [Signal] public delegate void UIOpenEventHandler();
    [Signal] public delegate void UICloseEventHandler();
    [Signal] public delegate void UIFocusEventHandler();
    [Signal] public delegate void UISelectEventHandler();
    public override void _Ready()
    {
        SignalManager.Instance.GnomeDied += OnGnomeDied;
        SignalManager.Instance.plankHealthDepleted += OnPlankDamaged;
        SignalManager.Instance.plankHalfHealthReached += OnPlankDamaged;
        SlingShotFired += OnSlingShotFired;
        PlayMainScreenMusic += OnPlayMainScreenMusic;
        PlayGameMusic += OnPlayGameMusic;
        UICancel += OnUICancel;
        UIOpen += OnUIOpen;
        UIClose += OnUIClose;
        UIFocus += OnUIFocus;
        UISelect += OnUISelect;
    }

    private void OnGnomeDied()
    {
        var gnomeDeathSFX = GetNode<AudioStreamPlayer>("SFX/GnomeDeath");
        gnomeDeathSFX.Play();
    }

    private void OnPlankDamaged(Plank _plank)
    {
        OnPlankDamaged();
    }

    private void OnPlankDamaged()
    {
        var plankDamageSFX = GetNode<AudioStreamPlayer>("SFX/PlankImpact");
        plankDamageSFX.Play();
    }

    private void OnSlingShotFired()
    {
        var slingshotShootSFX = GetNode<AudioStreamPlayer>("SFX/SlingshotShoot");
        slingshotShootSFX.Play();
    }

    private void StopAllMusic()
    {
        var mainScreenMusic = GetNode<AudioStreamPlayer>("Music/MainScreenMusic");
        var gameMusic = GetNode<AudioStreamPlayer>("Music/GameMusic");

        mainScreenMusic.Stop();
        gameMusic.Stop();
    }

    private void OnPlayMainScreenMusic()
    {
        StopAllMusic();
        var mainScreenMusic = GetNode<AudioStreamPlayer>("Music/MainScreenMusic");
        mainScreenMusic.Play();
    }

    private void OnPlayGameMusic()
    {
        StopAllMusic();
        var gameMusic = GetNode<AudioStreamPlayer>("Music/GameMusic");
        gameMusic.Play();
    }
    private void OnUICancel()
    {
        var uiCancelSFX = GetNode<AudioStreamPlayer>("SFX/UICancel");
        uiCancelSFX.Play();
    }

    private void OnUIOpen()
    {
        var uiOpenSFX = GetNode<AudioStreamPlayer>("SFX/UIOpen");
        uiOpenSFX.Play();
    }

    private void OnUIClose()
    {
        var uiCloseSFX = GetNode<AudioStreamPlayer>("SFX/UIClose");
        uiCloseSFX.Play();
    }

    private void OnUIFocus()
    {
        var uiFocusSFX = GetNode<AudioStreamPlayer>("SFX/UIFocus");
        uiFocusSFX.Play();
    }

    private void OnUISelect()
    {
        var uiSelectSFX = GetNode<AudioStreamPlayer>("SFX/UISelect");
        uiSelectSFX.Play();
    }
}


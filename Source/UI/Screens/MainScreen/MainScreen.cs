using Godot;
using System;

public partial class MainScreen : Control
{
    public void OnSourceCodeButtonPressed()
    {
        OS.ShellOpen("https://github.com/6ajmon/Furball-Siege");
    }
}

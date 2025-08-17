using Godot;
using System;

public partial class MainScreen : Node
{
    public void OnSourceCodeButtonPressed()
    {
        OS.ShellOpen("https://github.com/6ajmon/Furball-Siege");
    }
}

using Godot;
using System;

public partial class Music : AudioStreamPlayer
{
    public override void _ExitTree()
    {
        base._ExitTree();
        
        Stop();
    }
}

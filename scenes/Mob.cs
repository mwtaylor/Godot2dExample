using Godot;
using System;

public partial class Mob : RigidBody2D
{
    public override void _Ready()
    {
        base._Ready();
        
        var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        var mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
        animatedSprite2D.Play(mobTypes[GD.Randi() % mobTypes.Length]);
    }

    private void OnVisibleOnScreenNotifier2DScreenExited()
    {
        QueueFree();
    }
}

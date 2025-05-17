using Godot;

namespace DodgeTheCreeps;

public partial class Mob : RigidBody2D
{
    [Export]
    public float MaximumRandomDirectionChange { get; set; } = Mathf.Pi / 4;
    [Export]
    public double MinimumSpeed { get; set; } = 100.0;
    [Export]
    public double MaximumSpeed { get; set; } = 250.0;
    
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
using Godot;
using Vector2 = Godot.Vector2;

namespace DodgeTheCreeps;

public partial class Player : Area2D
{
    [Signal]
    public delegate void HitEventHandler();
    
    [Export]
    public int Speed { get; set; } = 400;

    private Vector2 _screenSize;
    private CollisionShape2D _collisionShape;
    private AnimatedSprite2D _animatedSprite2D;
    
    public override void _Ready()
    {
        base._Ready();
        
        _screenSize = GetViewportRect().Size;

        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        
        Hide();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        
        var velocity = Vector2.Zero;

        if (Input.IsActionPressed("move_up"))
        {
            velocity += Vector2.Up;
        }
        
        if (Input.IsActionPressed("move_down"))
        {
            velocity += Vector2.Down;
        }
        
        if (Input.IsActionPressed("move_left"))
        {
            velocity += Vector2.Left;
        }
        
        if (Input.IsActionPressed("move_right"))
        {
            velocity += Vector2.Right;
        }

        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * Speed;
            _animatedSprite2D.Play();
        }
        else
        {
            _animatedSprite2D.Stop();
        }
        
        Position += velocity * (float)delta;
        Position = new Vector2(
            x: Mathf.Clamp(Position.X, 0, _screenSize.X),
            y: Mathf.Clamp(Position.Y, 0, _screenSize.Y)
        );

        if (velocity.X != 0)
        {
            _animatedSprite2D.Animation = "walk";
            _animatedSprite2D.FlipV = false;
            _animatedSprite2D.FlipH = velocity.X < 0;
        }
        else if (velocity.Y != 0)
        {
            _animatedSprite2D.Animation = "up";
            _animatedSprite2D.FlipV = velocity.Y > 0;
            _animatedSprite2D.FlipH = false;
        }
    }

    public void Start(Vector2 position)
    {
        Position = position;
        Show();
        _collisionShape.Disabled = false;
    }

    private void OnBodyEntered(Node2D body)
    {
        Hide();
        EmitSignalHit();
        _collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }
}
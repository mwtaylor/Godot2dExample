using Godot;
using System;

public partial class Main : Node
{
    [Export]
    public PackedScene MobScene { get; set; }

    private int _score;

    private void NewGame()
    {
        _score = 0;
        
        GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
        
        var player = GetNode<Player>("Player");
        var startPosition = GetNode<Marker2D>("StartPosition");
        player.Start(startPosition.Position);
        
        GetNode<Timer>("StartTimer").Start();
        
        var hud = GetNode<Hud>("HUD");
        hud.UpdateScore(_score);
        hud.ShowMessage("Get Ready!");
    }

    private void GameOver()
    {
        GetNode<Timer>("MobTimer").Stop();
        GetNode<Timer>("ScoreTimer").Stop();
        GetNode<Hud>("HUD").ShowGameOver();
        GetNode<AudioStreamPlayer>("DeathSound").Play();
    }

    private void OnMobTimerTimeout()
    {
        var mob = MobScene.Instantiate<Mob>();

        var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
        mobSpawnLocation.ProgressRatio = GD.Randf();

        var direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;
        
        mob.Position = mobSpawnLocation.Position;
        
        direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        mob.Rotation = direction;

        var velocity = new Vector2((float)GD.RandRange(50.0, 250.0), 0);
        mob.LinearVelocity = velocity.Rotated(direction);
        
        mob.AddToGroup("mobs");
        AddChild(mob);
    }

    private void OnScoreTimerTimeout()
    {
        _score++;
        GetNode<Hud>("HUD").UpdateScore(_score);
    }

    private void OnStartTimerTimeout()
    {
        GetNode<Timer>("MobTimer").Start();
        GetNode<Timer>("ScoreTimer").Start();
    }
}

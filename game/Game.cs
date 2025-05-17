using Godot;

namespace DodgeTheCreeps;

public partial class Game : Node
{
    private const string MobsGroupName = "mobs";

    [Export]
    public PackedScene MobScene { get; set; }

    private int _score;
    private Player _player;
    private Marker2D _startPosition;
    private Timer _mobTimer;
    private Timer _scoreTimer;
    private Timer _startTimer;
    private Hud _hud;
    private AudioStreamPlayer _deathSound;
    private PathFollow2D _mobSpawnLocation;
    
    public override void _Ready()
    {
        base._Ready();
        
        _player = GetNode<Player>("Player");
        _startPosition = GetNode<Marker2D>("StartPosition");
        _mobTimer = GetNode<Timer>("MobTimer");
        _scoreTimer = GetNode<Timer>("ScoreTimer");
        _startTimer = GetNode<Timer>("StartTimer");
        _hud = GetNode<Hud>("HUD");
        _deathSound = GetNode<AudioStreamPlayer>("DeathSound");
        _mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
    }

    private void NewGame()
    {
        _score = 0;
        
        GetTree().CallGroup(MobsGroupName, Node.MethodName.QueueFree);
        
        _player.Start(_startPosition.Position);
        
        _startTimer.Start();
        
        _hud.UpdateScore(_score);
        _hud.ShowMessage(TranslationServer.Translate("Get Ready!"));
    }

    private void GameOver()
    {
        _mobTimer.Stop();
        _scoreTimer.Stop();
        _hud.ShowGameOver();
        _deathSound.Play();
    }

    private void OnMobTimerTimeout()
    {
        var mob = MobScene.Instantiate<Mob>();
        
        _mobSpawnLocation.ProgressRatio = GD.Randf();
        mob.Position = _mobSpawnLocation.Position;
        
        var direction = _mobSpawnLocation.Rotation + Mathf.Pi / 2;
        direction += (float)GD.RandRange(-mob.MaximumRandomDirectionChange, mob.MaximumRandomDirectionChange);
        mob.Rotation = direction;

        var velocity = new Vector2((float)GD.RandRange(mob.MinimumSpeed, mob.MaximumSpeed), 0);
        mob.LinearVelocity = velocity.Rotated(direction);
        
        mob.AddToGroup(MobsGroupName);
        AddChild(mob);
    }

    private void OnScoreTimerTimeout()
    {
        _score++;
        _hud.UpdateScore(_score);
    }

    private void OnStartTimerTimeout()
    {
        _mobTimer.Start();
        _scoreTimer.Start();
    }
}

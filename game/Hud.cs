using Godot;

namespace DodgeTheCreeps;

public partial class Hud : CanvasLayer
{
    [Signal]
    public delegate void StartGameEventHandler();
    
    private Button _startButton;
    private Label _message;
    private Label _scoreLabel;
    private Timer _messageTimer;
    private Timer _gameOverResetTimer;
    private Timer _resetStartButtonTimer;
    
    public override void _Ready()
    {
        base._Ready();
        
        _startButton = GetNode<Button>("StartButton");
        _message = GetNode<Label>("Message");
        _scoreLabel = GetNode<Label>("ScoreLabel");
        _messageTimer = GetNode<Timer>("MessageTimer");
        _gameOverResetTimer = GetNode<Timer>("GameOverResetTimer");
        _resetStartButtonTimer = GetNode<Timer>("ResetStartButtonTimer");
    }
    
    private void OnStartButtonPressed()
    {
        _startButton.Hide();
        EmitSignalStartGame();
    }

    private void OnMessageTimerTimeout()
    {
        _message.Hide();
    }
    
    public void UpdateScore(int score)
    {
        _scoreLabel.Text = score.ToString();
    }
    
    public void ShowMessage(string text, bool startTimer = true)
    {
        _message.Text = text;
        _message.Show();

        if (startTimer)
        {
            _messageTimer.Start();
        }
    }
    
    public void ShowGameOver()
    {
        ShowMessage(TranslationServer.Translate("Game Over"));
        _gameOverResetTimer.Start();
    }
    
    private void OnGameOverResetTimerTimeout()
    {
        ShowMessage(TranslationServer.Translate("Dodge the Creeps!"), startTimer: false);
        _resetStartButtonTimer.Start();
    }

    private void OnResetStartButtonTimerTimeout()
    {
        _startButton.Show();
    }
}
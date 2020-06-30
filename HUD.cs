using Godot;

public class HUD : CanvasLayer
{
	[Signal]
	public delegate void StartGame();

	private Timer _messageTimer;
	private Label _messageLabel;

	private Button _startButton;

	public override void _Ready()
	{
		_messageTimer = GetNode<Timer>("MessageTimer");
		_messageLabel = GetNode<Label>("MessageLabel");

		_startButton = GetNode<Button>("StartButton");
	}

	public void ShowMessage(string text)
	{
		_messageLabel.Text = text;
		_messageLabel.Show();

		_messageTimer.Start();
	}

	async public void ShowGameOver()
	{
		ShowMessage("Game over");

		await ToSignal(_messageTimer, "timeout");

		_messageLabel.Text = "Dodge the\nCreeps!";
		_messageLabel.Show();

		_startButton.Show();
	}

	public void UpdateScore(int score)
	{
		GetNode<Label>("ScoreLabel").Text = score.ToString();
	}
	
	public void OnStartButtonPressed()
	{
		_startButton.Hide();
		EmitSignal("StartGame");
	}
	
	public void OnMessageTimerTimeout()
	{
		_messageLabel.Hide();
	}
}





using Godot;

public partial class GameUi : Control
{
	[Export] private Label _debugLabel;
	[Export] private Label _exitLabel;
	[Export] private Label _scoreLabel;
	[Export] private Label _timeLabel;
	[Export] private Label _gameOverLabel;
	[Export] private ColorRect _gameOverRect;

	private float _time = 0.0f;
	private bool _gameOver = false;

	public override void _Ready()
	{
		SignalManager.Instance.OnDebugLabel += OnDebugLabel;
	}

	private void OnDebugLabel(string str)
	{
		_debugLabel.Text = str;
	}


	public override void _Process(double delta)
	{
		if (!_gameOver)
		{
			_time += (float)delta;
			_timeLabel.Text = $"Time: {_time:F1}s";
		}
		else if (Input.IsActionJustPressed("space"))
		{
			GameManager.LoadMainScene();
		}
	}
	public void UpdateScore(int act, int target)
	{
		_scoreLabel.Text = $"{act}/{target}";
	}
}

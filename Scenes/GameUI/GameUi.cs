
using Godot;

public partial class GameUi : Control
{
	[Export] private Label _exitLabel;
	[Export] private Label _scoreLabel;
	[Export] private Label _timeLabel;
	[Export] private Label _gameOverLabel;
	[Export] private ColorRect _gameOverRect;

	private float _time = 0.0f;
	private bool _gameOver = false;

	public override void _Ready()
	{
		SignalManager.Instance.OnShowExit += OnShowExit;
		SignalManager.Instance.OnExitFound += OnExitFound;
		SignalManager.Instance.OnGameOver += OnGameOver;
	}

	public override void _ExitTree()
	{
		SignalManager.Instance.OnShowExit -= OnShowExit;
		SignalManager.Instance.OnExitFound -= OnExitFound;
		SignalManager.Instance.OnGameOver -= OnGameOver;
	}

	private void StopGame()
	{
		GetTree().Paused = true;
		_gameOver = true;
		_gameOverRect.Show();
	}

	private void OnExitFound()
	{
		StopGame();
		_gameOverLabel.Text = $"Well done! You escaped! Time taken: {_time:F1} seconds.";
	}

	private void OnGameOver()
	{
		StopGame();
		_gameOverLabel.Text = "You died! What rotten luck!";
	}

	private void OnShowExit()
	{
		_exitLabel.Show();
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

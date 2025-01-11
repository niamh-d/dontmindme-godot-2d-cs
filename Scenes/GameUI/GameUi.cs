
using Godot;

public partial class GameUi : Control
{
	[Export] private Label _debugLabel;

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
	}
}

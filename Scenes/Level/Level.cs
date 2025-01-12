using Godot;

public partial class Level : Node2D
{
    [Export] private Node _pickups;
    [Export] private GameUi _gameUi;

    private int _pickupsCount = 0;
    private int _pickupsCollectedCount = 0;

    public override void _Ready()
    {
        GetTree().Paused = false;
        _pickupsCount = _pickups.GetChildCount();
        SignalManager.Instance.OnPickUp += OnPickUp;
        UpdateScore();
    }

    private void OnPickUp()
    {
        _pickupsCollectedCount++;
        UpdateScore();
        if (_pickupsCollectedCount >= _pickupsCount)
        {
            SignalManager.EmitOnShowExit();
        }
    }

    private void UpdateScore()
    {
        _gameUi.UpdateScore(_pickupsCollectedCount, _pickupsCount);
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("quit"))
        {
            GameManager.LoadMainScene();
        }
    }
}

using Godot;

public partial class Bullet : Area2D
{
	private static readonly PackedScene _boomScene = GD.Load<PackedScene>("res://Scenes/Boom/Boom.tscn");

	[Export] private float _speed = 250.0f;
	[Export] private Timer _timer;

	public override void _Ready()
	{
		var pr = GetTree().GetFirstNodeInGroup(Player.GroupName) as Player;
		if (pr != null)
		{
			LookAt(pr.GlobalPosition);
		}
		BodyEntered += OnBodyEntered;
		_timer.Timeout += OnTimeout;
	}

	public override void _Process(double delta)
	{
		MoveLocalX((float)delta * _speed);
	}

	private void CreateBoom()
	{
		var boomInstace = _boomScene.Instantiate<Boom>();
		GetTree().Root.AddChild(boomInstace);
		boomInstace.GlobalPosition = GlobalPosition;
		QueueFree();
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			_timer.Stop();
			SignalManager.EmitOnGameOver();
		}
		CreateBoom();
	}

	private void OnTimeout()
	{
		CreateBoom();
	}

}

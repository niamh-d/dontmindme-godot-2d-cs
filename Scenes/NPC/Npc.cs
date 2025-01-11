using Godot;

public partial class Npc : CharacterBody2D
{
	[Export] private NavigationAgent2D _navAgent;
	[Export] private float _moveSpeed = 100.0f;

	public override void _Ready()
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("test"))
		{
			_navAgent.TargetPosition = GetGlobalMousePosition();
		}
		UpdateNavigation();
		UpdateDebugLabel();
	}

	private void UpdateDebugLabel()
	{
		string str = "";
		str += $"IsTargetReached: {_navAgent.IsTargetReached()}\n";
		str += $"IsTargetReachable: {_navAgent.IsTargetReachable()}\n";
		str += $"IsNavigationFinished: {_navAgent.IsNavigationFinished()}\n";
		str += $"Target: {_navAgent.TargetPosition}";
		SignalManager.EmitOnDebugLabel(str);
	}

	private void UpdateNavigation()
	{
		if (!_navAgent.IsNavigationFinished())
		{
			var nextPathPos = _navAgent.GetNextPathPosition();
			LookAt(nextPathPos);
			Velocity = Position.DirectionTo(nextPathPos) * _moveSpeed;
			MoveAndSlide();
		}
	}
}

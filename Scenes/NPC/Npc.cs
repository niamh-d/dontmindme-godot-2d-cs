using System;
using System.Collections.Generic;
using Godot;

public partial class Npc : CharacterBody2D
{
	[Export] private NavigationAgent2D _navAgent;
	[Export] private float _moveSpeed = 100.0f;
	[Export] private Node2D _patrolPoints;
	[Export] private Node2D _playerDetect;
	[Export] private RayCast2D _rayCast;

	private enum EnemyState { Patrolling, Chasing, Searching }
	private EnemyState _state = EnemyState.Patrolling;
	private int _currentWp = 0;
	private List<Vector2> _waypoints = new();
	Player _playerRef;

	public override void _Ready()
	{
		_playerRef = GetTree().GetFirstNodeInGroup(Player.GroupName) as Player;
		SetPhysicsProcess(false);
		CreateWaypoints();
		CallDeferred(MethodName.LateSetup);
	}

	private async void LateSetup()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		SetPhysicsProcess(true);
	}

	private void CreateWaypoints()
	{
		foreach (Node2D child in _patrolPoints.GetChildren())
		{
			_waypoints.Add(child.GlobalPosition);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("test"))
		{
			_navAgent.TargetPosition = GetGlobalMousePosition();
		}
		RayCastToPlayer();
		UpdateMovement();
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

	private void RayCastToPlayer()
	{
		_playerDetect.LookAt(_playerRef.GlobalPosition);
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

	private void SetNextWaypoint()
	{
		_currentWp %= _waypoints.Count;
		_navAgent.TargetPosition = _waypoints[_currentWp];
		_currentWp++;
	}

	private void UpdateMovement()
	{
		switch (_state)
		{
			case EnemyState.Patrolling:
				ProcessPatrolling();
				break;
			case EnemyState.Chasing:
				ProcessChasing();
				break;
			case EnemyState.Searching:
				ProcessSearching();
				break;
			default:
				ProcessPatrolling();
				break;
		}
	}

	private void ProcessPatrolling()
	{
		if (_navAgent.IsNavigationFinished())
		{
			SetNextWaypoint();
			_state = EnemyState.Patrolling;
		}
	}

	private void ProcessChasing()
	{
		throw new NotImplementedException();
	}

	private void ProcessSearching()
	{
		throw new NotImplementedException();
	}
}

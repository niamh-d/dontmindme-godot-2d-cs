using System;
using System.Collections.Generic;
using Godot;

public partial class Npc : CharacterBody2D
{
	[Export] private NavigationAgent2D _navAgent;
	[Export] private float _moveSpeed = 80.0f;
	[Export] private Node2D _patrolPoints;
	[Export] private Node2D _playerDetect;
	[Export] private RayCast2D _rayCast;
	[Export] private Sprite2D _warning;
	[Export] private AudioStreamPlayer2D _sound;
	[Export] private AnimationPlayer _animationPlayer;
	[Export] private Timer _shootTimer;

	private enum EnemyState { Patrolling, Chasing, Searching }
	private EnemyState _state = EnemyState.Patrolling;
	private int _currentWp = 0;
	private List<Vector2> _waypoints = new();
	Player _playerRef;
	private static readonly PackedScene _bulletScene = GD.Load<PackedScene>("res://Scenes/Bullet/Bullet.tscn");

	public override void _Ready()
	{
		_playerRef = GetTree().GetFirstNodeInGroup(Player.GroupName) as Player;
		SetPhysicsProcess(false);
		CreateWaypoints();
		CallDeferred(MethodName.LateSetup);
		_shootTimer.Timeout += OnShootTimerTimeout;
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
		UpdateState();
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
		str += $"Target: {_navAgent.TargetPosition}\n";
		str += $"Target: {GetFovAngle():F2}; InFOV: {PlayerIsInFov()}\n";
		str += $"PlayerDetected: {PlayerDetected()}\n";
		str += $"CanSeePlayer: {CanSeePlayer()}\n";
		str += $"State: {_state}\n";
		SignalManager.EmitOnDebugLabel(str);
	}

	private float GetFovAngle()
	{
		var dir = GlobalPosition.DirectionTo(_playerRef.GlobalPosition);
		var dotProduct = dir.Dot(Velocity.Normalized());
		if (dotProduct >= -1.0f && dotProduct <= 1.0f)
		{
			return Mathf.RadToDeg(Mathf.Acos(dotProduct));
		}
		return 0.0f;
	}

	private bool PlayerIsInFov()
	{
		return GetFovAngle() <= 60.0f;
	}


	private void RayCastToPlayer()
	{
		_playerDetect.LookAt(_playerRef.GlobalPosition);
	}

	private bool PlayerDetected()
	{
		var collider = _rayCast.GetCollider();
		return collider != null && (collider is Player);
	}

	private bool CanSeePlayer()
	{
		return PlayerDetected() && PlayerIsInFov();
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

	private void SetState(EnemyState newState)
	{
		if (_state == newState) return;

		if (newState == EnemyState.Patrolling)
		{
			_warning.Hide();
			SetNextWaypoint();
		}

		else if (newState == EnemyState.Searching)
		{
			_warning.Show();
			_animationPlayer.Play("RESET");
		}

		else if (newState == EnemyState.Chasing)
		{
			_warning.Hide();
			SoundManager.PlayGasp(_sound);
			_animationPlayer.Play("flash");
		}
		_state = newState;
	}

	private void Shoot()
	{
		var bullet = _bulletScene.Instantiate<Bullet>();
		bullet.GlobalPosition = GlobalPosition;
		GetTree().Root.AddChild(bullet);
		SoundManager.PlayLaser(_sound);
	}

	private void UpdateState()
	{
		var newState = _state;
		var canSee = CanSeePlayer();

		if (canSee)
		{
			newState = EnemyState.Chasing;
		}
		else if (!canSee && _state == EnemyState.Chasing)
		{
			newState = EnemyState.Searching;
		}
		SetState(newState);
	}

	private void ProcessPatrolling()
	{
		if (_navAgent.IsNavigationFinished())
		{
			SetNextWaypoint();
		}
	}

	private void ProcessChasing()
	{
		_navAgent.TargetPosition = _playerRef.GlobalPosition;
	}

	private void ProcessSearching()
	{
		if (_navAgent.IsNavigationFinished())
		{
			SetState(EnemyState.Patrolling);
		}
	}

	private void OnShootTimerTimeout()
	{
		if (_state != EnemyState.Chasing) return;
		Shoot();
	}
}

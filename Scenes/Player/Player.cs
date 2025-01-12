using Godot;

public partial class Player : CharacterBody2D
{
	public const string GroupName = "Player";
	[Export] private float _moveSpeed = 120.0f;

	public override void _EnterTree()
	{
		AddToGroup(GroupName);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 newVel = Vector2.Zero;
		newVel.X = Input.GetActionStrength("right") - Input.GetActionStrength("left");
		newVel.Y = Input.GetActionStrength("down") - Input.GetActionStrength("up");
		Velocity = newVel.Normalized() * _moveSpeed;
		Rotation = Velocity.Angle();
		MoveAndSlide();
	}
}

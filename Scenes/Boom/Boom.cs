using Godot;
using System;

public partial class Boom : AnimatedSprite2D
{
    [Export] private AudioStreamPlayer2D _sound;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        _sound.Finished += () => QueueFree();
	}
}

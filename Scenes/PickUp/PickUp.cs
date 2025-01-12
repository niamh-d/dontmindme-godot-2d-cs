using Godot;

public partial class PickUp : Area2D
{
	[Export] private AudioStreamPlayer2D _sound;
	[Export] private AnimationPlayer _animationPlayer;

	public override void _Ready()
	{
		_sound.Finished += OnSoundFinished;
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D _body)
	{
		_sound.Stream = SoundManager.GetRandomPickupSound();
		_sound.Play();
		SignalManager.EmitOnPickUp();
		_animationPlayer.Play("vanish");
		SignalManager.EmitOnShowExit();
	}

	private void OnSoundFinished()
	{
		QueueFree();
	}
}

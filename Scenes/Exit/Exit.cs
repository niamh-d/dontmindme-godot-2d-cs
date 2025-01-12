using Godot;

public partial class Exit : Area2D
{
	public override void _Ready()
	{
		Hide();
		SignalManager.Instance.OnShowExit += OnShowExit;
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		GD.Print("Exit.OnBodyEntered");
		SignalManager.EmitOnExitFound();
	}

	private void OnShowExit()
	{
		GD.Print("Exit.OnShowExit");
		Show();
		SetDeferred(Area2D.PropertyName.Monitoring, true);
	}
}

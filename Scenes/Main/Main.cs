using Godot;

public partial class Main : Control
{
    public override void _Ready()
    {
        GetTree().Paused = false;
    }


    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("space"))
        {
            GameManager.LoadGameScene();
        }
    }
}

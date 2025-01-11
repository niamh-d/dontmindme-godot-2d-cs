using Godot;

public partial class GameManager : Node
{
    readonly PackedScene _gameScene = GD.Load<PackedScene>("res://Scenes/Level/Level.tscn");
    readonly PackedScene _mainScene = GD.Load<PackedScene>("res://Scenes/Main/Main.tscn");

    public static GameManager Instance { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance = this;
    }

    public static void LoadGameScene()
    {
        Instance.GetTree().ChangeSceneToPacked(Instance._gameScene);
    }

    public static void LoadMainScene()
    {
        Instance.GetTree().ChangeSceneToPacked(Instance._mainScene);
    }
}

using Godot;
using System;

public partial class SignalManager : Node
{
    [Signal] public delegate void OnPickUpEventHandler();
    [Signal] public delegate void OnShowExitEventHandler();
    [Signal] public delegate void OnExitFoundEventHandler();
    [Signal] public delegate void OnGameOverEventHandler();

    public static SignalManager Instance;

    public override void _EnterTree()
    {
        Instance = this;
    }

    public static void EmitOnPickUp()
    {
        Instance.EmitSignal(SignalName.OnPickUp);
    }

    public static void EmitOnShowExit()
    {
        Instance.EmitSignal(SignalName.OnShowExit);
    }

    public static void EmitOnExitFound()
    {
        Instance.EmitSignal(SignalName.OnExitFound);
    }

    public static void EmitOnGameOver()
    {
        Instance.EmitSignal(SignalName.OnGameOver);
    }
}

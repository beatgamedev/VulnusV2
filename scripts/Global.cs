using Godot;
using System;
using System.Collections.Generic;

public class Global : Node
{
	public static Global Instance;
	public static Discord.Wrapper Discord;
	public Node CurrentScene { get; private set; }
	public Control Overlay { get; private set; }
	public Dictionary<string, Control> Overlays { get; private set; }
	public override void _Ready()
	{
		Instance = this;
		Discord = new Discord.Wrapper();
		SquirrelW.Run();
		Viewport root = GetTree().Root;
		CurrentScene = root.GetChild(root.GetChildCount() - 1);
		var overlayScene = (PackedScene)GD.Load("res://scenes/Overlay.tscn");
		Overlay = (Control)overlayScene.Instance();
		Overlays = new Dictionary<string, Control>();
		CallDeferred(nameof(AddOverlay));
	}
	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);
		Discord.RunCallbacks();
	}
	private void AddOverlay()
	{
		GetTree().Root.AddChild(Overlay);
		GetTree().Root.MoveChild(Overlay, 1);
		foreach (Control overlay in Overlay.GetChildren())
		{
			Overlays.Add(overlay.Name, overlay);
		}
	}
	public void GotoScene(string path)
	{
		CallDeferred(nameof(DeferredGotoScene), path);
	}
	private void DeferredGotoScene(string path)
	{
		CurrentScene.Free();
		var nextScene = (PackedScene)GD.Load(path);
		CurrentScene = nextScene.Instance();
		GetTree().Root.AddChild(CurrentScene);
		GetTree().Root.MoveChild(CurrentScene, 0);
		GetTree().CurrentScene = CurrentScene;
	}
}

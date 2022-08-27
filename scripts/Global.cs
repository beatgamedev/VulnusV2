using Godot;
using System;
using System.Collections.Generic;

public class Global : Node
{
	public static Global Instance;
	public static Discord.DiscordW Discord;
	public static StreamTexture Matt;
	public Node CurrentScene { get; private set; }
	public Control Overlay { get; private set; }
	public Dictionary<string, Control> Overlays { get; private set; }
	public Global() : base()
	{
		Instance = this;
		SquirrelW.Run();
		Discord = new Discord.DiscordW();
		Discord.SetActivity(new Discord.ActivityW());
	}
	public override void _Ready()
	{
		Input.SetUseAccumulatedInput(false);
		Matt = (StreamTexture)GD.Load("res://assets/images/matt.jpg");
		Viewport root = GetTree().Root;
		CurrentScene = root.GetChild(root.GetChildCount() - 1);
	}
	public override void _PhysicsProcess(float delta)
	{
		if (Input.IsActionJustPressed("fullscreen"))
			OS.WindowFullscreen = !OS.WindowFullscreen;
		Discord.RunCallbacks();
	}
	public void AddOverlay()
	{
		CallDeferred(nameof(DeferredAddOverlay));
	}
	private void DeferredAddOverlay()
	{
		var overlayScene = (PackedScene)GD.Load("res://scenes/Overlay.tscn");
		Overlay = (Control)overlayScene.Instance();
		Overlays = new Dictionary<string, Control>();
		GetTree().Root.AddChild(Overlay);
		GetTree().Root.MoveChild(Overlay, 2);
		foreach (Control overlay in Overlay.GetChildren())
		{
			Overlays.Add(overlay.Name, overlay);
		}
	}
	public void GotoScene(string path, Action<Node> callback = null)
	{
		CallDeferred(nameof(DeferredGotoScene), path, callback);
	}
	private void DeferredGotoScene(string path, Action<Node> callback = null)
	{
		CurrentScene.Free();
		var nextScene = (PackedScene)GD.Load(path);
		CurrentScene = nextScene.Instance();
		GetTree().Root.AddChild(CurrentScene);
		GetTree().Root.MoveChild(CurrentScene, 1);
		GetTree().CurrentScene = CurrentScene;
		callback?.Invoke(CurrentScene);
	}
}

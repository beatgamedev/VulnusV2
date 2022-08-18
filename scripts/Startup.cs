using Godot;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class Startup : Node
{
	private Label label;
	private string text;
	private float counter;
	private static bool hasRun = false;
	private static Task task;
	public override void _Ready()
	{
		label = GetNode<Label>("Label");
		if (hasRun)
			return;
		hasRun = true;
		Connect(nameof(StageReached), this, nameof(OnStageReached));
		task = Task.Run(Run);
	}
	public void OnStageReached(string stage, bool end)
	{
		if (end)
		{
			Global.Instance.GotoScene("res://scenes/MainMenu.tscn");
		}
		text = stage;
		counter = 0;
	}
	[Signal]
	public delegate void StageReached(string stage, bool end);
	public void Run()
	{
		if (OS.HasFeature("Android"))
		{
			EmitSignal(nameof(StageReached), "Request permissions", false);
			OS.RequestPermissions();
		}
		EmitSignal(nameof(StageReached), "Loading settings", false);
		Settings.LoadSettings();
		EmitSignal(nameof(StageReached), "Loading maps", false);
		LoadMaps();
		EmitSignal(nameof(StageReached), "All done", true);
	}
	public override void _Process(float delta)
	{
		base._Process(delta);
		counter += delta;
		var dots = (int)(counter * 3) % 4;
		label.Text = text + new string('.', dots);
		if (task.IsFaulted)
			throw task.Exception;
	}
	public void LoadMaps()
	{
		var start = OS.GetTicksUsec();
		bool loaded = MapLoader.LoadMapsFromDirectory(OS.GetUserDataDir().PlusFile("maps"));
		var end = OS.GetTicksUsec();
		if (!loaded)
		{
			GD.PrintErr($"Failed to load maps after {(end - start) / 1000}ms");
			return;
		}
		GD.Print($"Took {(end - start) / 1000}ms to load maps");
	}
}

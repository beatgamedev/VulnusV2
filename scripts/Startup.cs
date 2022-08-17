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
	private static bool ready = false;
	private static Dictionary<string, Task> tasks = new Dictionary<string, Task>();
	public override void _Ready()
	{
		if (hasRun)
			return;
		hasRun = true;
		Run();
	}
	public void Run()
	{
		label = GetNode<Label>("Label");
		text = "Loading maps";
		Task LoadMapsTask = new Task(() => LoadMaps());
		tasks.Add("maps", LoadMapsTask);
		LoadMapsTask.Start();
		ready = true;
	}
	public override void _Process(float delta)
	{
		base._Process(delta);
		counter += delta;
		var dots = (int)(counter * 3) % 4;
		label.Text = text + new string('.', dots);
	}
	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);
		if (ready)
		{
			bool allDone = true;
			foreach (KeyValuePair<string, Task> pair in tasks)
			{
				var task = pair.Value;
				if (pair.Key == "maps" && !task.IsCompleted)
				{
					text = $"Loading maps ({MapLoader.LoadedMaps.Count})";
				}
				if (task.IsFaulted)
				{
					GD.Print(task.Exception);
				}
				allDone = allDone && task.IsCompleted;
				if (!allDone)
					break;
			}
			if (!allDone)
				return;
			ready = false;
			tasks.Clear();
			Global.Instance.GotoScene("res://scenes/MainMenu.tscn");
		}
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

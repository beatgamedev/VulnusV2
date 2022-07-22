using Godot;
using System;

public class Startup : Node
{
	private Label label;
	private string text;
	private float counter;
	private static bool hasRun = false;
	public override void _Ready()
	{
		if (!hasRun)
		{
			hasRun = true;
		}
		else
		{
			GD.Print("Already loaded!");
			return;
		}
		label = GetNode<Label>("Label");
		text = "Loading maps";
		LoadMaps();
		Global.Instance.GotoScene("res://scenes/MainMenu.tscn");
	}
	public override void _Process(float delta)
	{
		base._Process(delta);
		counter += delta;
		var dots = (int)(counter * 3) % 4;
		label.Text = text + new string('.', dots);
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

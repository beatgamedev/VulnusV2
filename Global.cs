using Godot;
using System;

public class Global : Node
{
	public static Global Instance;
	public Node CurrentScene { get; set; }

	public override void _Ready()
	{
		Instance = this;
		Viewport root = GetTree().Root;
		CurrentScene = root.GetChild(root.GetChildCount() - 1);

		var start = OS.GetTicksUsec();
		MapLoader.LoadMapsFromDirectory(OS.GetUserDataDir().PlusFile("maps"));
		var end = OS.GetTicksUsec();
		GD.Print("Took " + (end - start) / 1000 + "ms to load maps");
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
		GetTree().CurrentScene = CurrentScene;
	}
}

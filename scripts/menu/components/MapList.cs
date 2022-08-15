using Godot;
using System;

public class MapList : Node
{
	public override void _Ready()
	{
		var content = GetNode<VBoxContainer>("Content");
		foreach (Map map in MapLoader.LoadedMaps)
		{
			var btn = MapButton.Create();
			btn.Map = map;
			btn.ManualUpdate();
			content.AddChild(btn);
		}
	}
}

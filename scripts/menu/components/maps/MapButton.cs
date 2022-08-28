using Godot;
using System;

public class MapButton : Button
{
	public BeatmapSet Map;
	private Tween tween;
	public void ManualUpdate()
	{
		if (Map == null)
			return;
		GetNode<TextureRect>("Cover").Texture = Map.LoadCover();
		GetNode<Label>("Title").Text = Map.Name;
		GetNode<Label>("Mapper").Text = Map.Mappers;
	}
	public static MapButton Create()
	{
		var btn = (PackedScene)GD.Load("res://prefabs/menu/MapButton.tscn");
		return (MapButton)btn.Instance();
	}
}

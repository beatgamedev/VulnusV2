using Godot;
using System;

public class Results : View
{
	public override void _Ready()
	{
		if (Game.Score == null)
			return;
		var info = GetNode<Control>("Info");
		var mapInfo = info.GetNode<Control>("Map");
		mapInfo.GetNode<TextureRect>("Cover").Texture = Game.LoadedMapset.LoadCover();
		mapInfo.GetNode<Label>("Title").Text = Game.LoadedMapset.Name;
		mapInfo.GetNode<Label>("Mappers").Text = Game.LoadedMapset.Mappers;
		mapInfo.GetNode<Label>("Difficulty").Text = Game.LoadedMap.Name;
	}
}
